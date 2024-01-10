<?php

namespace modules\schedulingmodule\forms;

use \DateTime;
use Craft;
use craft\elements\Entry;
use modules\DupageCoreModule\DupageCoreModule;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\base\Model;
use yii\db\Expression;

final class RecommendedPhysicians extends Model
{
    /**
     * @var string $startTime
     */
    public $date;

    /**
     * @var Entry $selectedPhysician
     */
    public $selectedPhysician;

    /**
     * @var string $chosenVisitTypeCode
     */
    public $chosenVisitTypeCode;

    /**
     * @var string $user
     */
    public $user;

    // processing all of the returned appintments data can create very large loops
    // we must avoid unnecessary DB calls as much as possible
    // every unique query ought to be executed only once, and the results should be stored in memory**
    //
    // **
    // using Redis was attempted, but the approach was not effective due to potentially
    // having to store entire Entry objects in Redis
    // storing only IDs in redis would result in having to do the query all over again

    /**
     * @var [] $suiteForExternalDepartmentID
     */
    private $suiteForExternalDepartmentID = [];

    /**
     * @var [] $physiciansAtLocations
     */
    private $physiciansAtLocations = [];

    /**
     * @var [] $physicianDetails
     */
    private $physicianDetails = [];

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            ['chosenVisitTypeCode', 'number'],
            ['selectedPhysician', 'validatePhysician'],
            ['user', 'validateUser'],
            ['date', 'date', 'format' => 'php:D M j g:ia'],
            [['chosenVisitTypeCode', 'selectedPhysician', 'date', 'user'], 'safe'],
            [['chosenVisitTypeCode', 'selectedPhysician', 'date', 'user'], 'required'],
        ];
    }

    /**
     * Validates physician
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validatePhysician($attribute)
    {
        return is_a($this->$attribute, 'modules\DupageCoreModule\models\PatientUser') && $this->$attribute->section === 'physicians';
    }

    /**
     * Validates user
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateUser($attribute)
    {
        return is_a($this->$attribute, 'modules\DupageCoreModule\models\PatientUser');
    }

    /**
     * Returns a list of recommended providers. This is the main helper method encapsulating the entire logic as defined by other methods.
     *
     * Providers adhere to the same requirements as the Provider in view, regarding:
     *   - Entry Type
     *   - Specialty currently being scheduled for - practices what I'm scheduling for
     *   - Age rule-out(s) - see's my age
     *   - Reason for Visit rule-out(s) - see's my condition when applicable
     *   - Matching the office location (within 5 miles from the office addresses listed) availability
     *
     *
     *   Example response:
     *   [
     *       "Lombard" => [
     *           "title" => "Kathleen M. Kicsak, MD",
     *           "services" => [
     *               "Internal Medicine",
     *               "Pediatrics"
     *           ],
     *           "ratings" => [
     *               "overall" => 4.9,
     *               "count" => 360
     *           ],
     *           "externalDepartmentId" => "12345",
     *           "id" => "67890"
     *       ]
     *   ]
     *
     * @return array - list of recommended providers grouped by city names
     */
    public function getPhysicians()
    {
        $validForm = $this->validate();

        if (!$validForm || !$this->user->eligible_physicians) {
            Craft::warning(new PsrMessage('Unable to find recommended physicians.', [
                'errors' => $this->getErrors(),
                'eligible_physicians' => $this->user->eligible_physicians
            ]), get_class($this) . '::' . __METHOD__);
            return [];
        }

        // gather physicians that match user's selections from previous scheduling steps (insurance, reasons for visit, etc.)
        $physicians = Entry::find()->id(\array_diff($this->user->eligible_physicians, [$this->selectedPhysician->id]));

        // find locations where the currently-selected physician offers appointments
        $locations = $this->getLocations();

        // assign physicians to each location
        // $this->physiciansAtLocations = $this->groupPhysiciansByLocations($physicians, $locations);

        $physicians = $physicians->relatedTo($locations)->all();

        // filter out physicians who do not have cached appointments at any of these locations for given date
        $physicians = $this->duplicatePhysiciansForTheirUniqueLocations($physicians, $locations);

        // filter out physicians who do not have cached appointments at any of these locations for given date
        $physicians = $this->removePhysiciansWithoutAppointments($physicians);

        // map physician Entry objects to simple objects with only necessary data
        $physicians = $this->simplifyPhysicianObjects($physicians);

        return $physicians;
    }

    /**
     * Returns a list of location Entries associated with the selected physician.
     * The list also included offices within a 5 mile radius from the main physician's locations.
     *
     * @return array
     */
    private function getLocations()
    {
        $locations = [];
        $selectedPhysicianSuites = $this->selectedPhysician->physicianLocations->type('suite')->all();
        foreach ($selectedPhysicianSuites as $selectedPhysicianSuite) {
            if (!is_string($selectedPhysicianSuite->address)) {
                $cityName = $selectedPhysicianSuite->address->parts->city;
            } else {
                $cityName = \json_decode($selectedPhysicianSuite->address, true)['parts']['city'];
            }
            $nearbyLocations = $this->findNearbyLocations($selectedPhysicianSuite);
            $locations = \array_merge($locations, [$selectedPhysicianSuite, ...$nearbyLocations]);
        }

        return $locations;
    }

    /**
     * Returns a list of location Entries within a $miles radius where the center point is the $location.
     *
     * @param Entry $location - Craft Entry location used as a center point
     * @param int $miles - radius within which moer locations should be found
     * @return array
     */
    private function findNearbyLocations($primaryLocation, $miles = 5)
    {
        $distanceExpression = new Expression(
            PhysiciansModule::getInstance()
                ->physiciansModuleService
                ->sqlDistanceBetweenTwoGivenPoints(
                    \json_decode($primaryLocation->address, true)['lng'],
                    \json_decode($primaryLocation->address, true)['lat'],
                    "JSON_EXTRACT(field_address, '$.lng')",
                    "JSON_EXTRACT(field_address, '$.lat')"
                )
        );

        $locations = Entry::find()
            ->section('locations')
            ->type('location')
            ->where(['<', $distanceExpression, $miles])
            ->all();

        $suites = [];
        foreach ($locations as $location) {
            $suites = \array_merge($suites, $location->getChildren()->all());
        }

        return $suites;
    }

    /**
     * Returns a list of physicians and their multiple locations grouped by the common city name.
     * If a physician has multiple offices in multiple cities, those physicians will be included for those multiple cities.
     *
     * @param EntryQuery $physicians - Craft entry query reperesenting the query for valid physicians.
     * @param array $locations - list of locations
     * @return array
     */
    private function groupPhysiciansByLocations($physicians, $locations)
    {
        $cities = [];
        foreach ($locations as $locationName => $locationEntries) {
            foreach ($locationEntries as $location) {
                $query = clone $physicians;

                // filter out physicians who do not offer appointments at this location
                $query->relatedTo([
                    'targetElement' => [
                        'or',
                        $location,
                        $location->children
                    ]
                ]);

                $cities[] = $locationName;

                // skip if this city key was already populated
                if (isset($this->physiciansAtLocations[$locationName]['complete'])) {
                    continue;
                }

                // associate physicians with their location name
                if (isset($this->physiciansAtLocations[$locationName])) {
                    $this->physiciansAtLocations[$locationName]['locations'][] = $location;
                    $this->physiciansAtLocations[$locationName]['physicians'] = \array_merge($this->physiciansAtLocations[$locationName]['physicians'], $query->all());
                } else {
                    $this->physiciansAtLocations[$locationName] = [
                        'locations' => [$location],
                        'physicians' => $query->all()
                    ];
                }
            }
        }

        // if this form executed multiple times for the same set of physicians and locations
        // we can re-use the results
        foreach ($cities as $cityName) {
            $this->physiciansAtLocations[$cityName]['complete'] = true;
        }

        return $this->physiciansAtLocations;
    }

    private function duplicatePhysiciansForTheirUniqueLocations($physicians, $validLocations)
    {
        $newList = [];
        foreach ($physicians as $physician) {
            $schedulingServiceIds = $this->user->getAppointmentServiceIds();
            // get external department IDs for this physician out of found locations
            $externalDepartmentIds = $this->getExternalDepartmentIDsForPhysicianFromLocations($physician, $schedulingServiceIds, $validLocations);

            foreach ($externalDepartmentIds as $externalDepartmentId) {
                $newList[] = [
                    'externalDepartmentId' => $externalDepartmentId,
                    'physician' => clone $physician
                ];
            }
        }

        return $newList;
    }

    /**
     * This method removes physicians who do not have cached appointment times for given date.
     * The "recommended physicians" module should not render physicians who do not have appointment times for given date.
     *
     * @param array $physicians - list of physicians and their multiple locations grouped by the common city name
     * @return array - modified list of physicians and their multiple locations grouped by the common city name
     */
    private function removePhysiciansWithoutAppointments($physicians)
    {
        // remove any physicians who do not have cached appointment times for given date
        // this will remove the noise and only leave physicians who can truly be recommended as alternatives
        $physicians = \array_values(\array_filter(
            $physicians,
            function ($physician) {
                $externalDepartmentId = $physician['externalDepartmentId'];
                $physician = $physician['physician'];

                $date = DateTime::createFromFormat('D M j g:ia', $this->date)->setTime(0, 0, 0, 0);
                $physicianCachedAppointmentTimes = PhysiciansModule::getInstance()
                    ->physiciansModuleService
                    ->getCachedAppointmentTimesForPhysicianFromDate(
                        $physician,
                        $date
                    );

                $nextAvailableAppointmentDateTime = $this->endOfDay($date, $physicianCachedAppointmentTimes, $externalDepartmentId);

                if ($nextAvailableAppointmentDateTime == false) {
                    return false;
                } else {
                    // $nextAvailableAppointmentDateTime Object;
                    // $this->physicianDetails[$physician->id] = $this->physicianDetails[$physician->id] ?? [];
                    // $this->physicianDetails[$physician->id]['nextAppointmentDates'][$externalDepartmentId] = $nextAvailableAppointmentDateTime->format("Y-m-d");
                    // add DateTime object to sort physicians
                    $physician->setDirtyAttributes([
                        'dt' => $nextAvailableAppointmentDateTime->format("Y-m-d H:i:s")
                    ]);

                    return true;
                }
            }
        ));

        // var_dump(count($physicians)); die;
        $physicians = $this->sortPhysiciansByDate($physicians);
        $physicians = \array_unique($physicians, SORT_REGULAR);
        $physicians = \array_slice($physicians, 0, 15);
        $physicians = \array_values($physicians);

        // $physicians[$location] = $details;

        // remove locations where all physicians were removed
        // $physicians = \array_filter(
        //     $physicians,
        //     function ($details) {
        //         return \count($details['physicians']) > 0;
        //     }
        // );

        return $physicians;
    }

    /**
     * Returns sorted Physicians by date
     *
     * @param array $detailsPhysicians
     *
     * @return array
     */
    private function sortPhysiciansByDate($detailsPhysicians)
    {
        $holder = [];

        foreach ($detailsPhysicians as $physician) {
            $physicianObject = $physician['physician'];

            // check to see if physician has DateTime object of their earliest time.
            if ($physicianObject->getDirtyAttributes()[0]) {
                if (empty($holder)) {
                    $holder[] = $physician;
                    continue;
                }

                // physician time 
                $currentPhysiciansDateTime = DateTime::createFromFormat('Y-m-d H:i:s', $physicianObject->getDirtyAttributes()[0]);


                for ($i = 0; $i < count($holder); $i++) {
                    // physician time from holder tank at possition $i
                    $storedPhysicians = DateTime::createFromFormat('Y-m-d H:i:s', $holder[$i]['physician']->getDirtyAttributes()[0]);

                    // Don't duplicate physician
                    if ($holder[$i]['physician']->title === $physicianObject->title && $storedPhysicians == $currentPhysiciansDateTime) {
                        break;
                    }

                    $holder = $this->cleanLoop($holder, $physician);
                    break;
                }
            }
        }

        return $holder;
    }

    /**
     * Re-order the list, looks like currrent physician has a date before the physicians inside holder
     *
     * @param array $holder - physicians already ordered
     * @param object $physician - physician object being compared
     *
     * @return array $temp - re-ordered physicians
     */
    private function cleanLoop($holder, $physician)
    {
        $physicianObject = $physician['physician'];
        $temp = [];
        $addPhysician = true;

        // loop through holder tank.
        for ($i = 0; $i < count($holder); $i++) {
            $physiciansTime = DateTime::createFromFormat('Y-m-d H:i:s', $physicianObject->getDirtyAttributes()[0]);
            $holderPhysiciansTime = DateTime::createFromFormat('Y-m-d H:i:s', $holder[$i]['physician']->getDirtyAttributes()[0]);

            // add holder physicians because they have an earlier time than $physician.
            if ($holderPhysiciansTime < $physiciansTime) {
                $temp[] = $holder[$i];
                continue;
            }

            // check DateTime are the same.
            if ($holderPhysiciansTime == $physiciansTime) {
                // Don not include duplicate physician.
                if ($physicianObject->id === $holder[$i]['physician']->id) {
                    break;
                }

                // tie breaker using dupageMedicalGroupStartDate.
                if (isset($physicianObject->dupageMedicalGroupStartDate) && isset($holder[$i]['physician']->dupageMedicalGroupStartDate)) {
                    $currentPhysicianDupageDate = DateTime::createFromFormat('m/d/Y', $physicianObject->dupageMedicalGroupStartDate);
                    $storedPhysicianDupageDate = DateTime::createFromFormat('m/d/Y', $holder[$i]['physician']->dupageMedicalGroupStartDate);

                    if ($currentPhysicianDupageDate < $storedPhysicianDupageDate) {
                        $temp[] = $physician;
                        $addPhysician = false;
                    } else {
                        $temp[] = $holder[$i];
                        continue;
                    }
                }
            }

            // add physician if they haven't been already added.
            if ($addPhysician) {
                $temp[] = $physician;
                $addPhysician = false;
            }
            // add physiciand who have later date than physician.
            $temp[] = $holder[$i];
        }

        // if both arrays are the same then we need to add the physician at the end
        // becuase they have the oldes date out of the list.
        if (count($temp) === count($holder)) {
            $temp[] = $physician;
        }

        return $temp;
    }

    private function endOfDay($date, $physicianCachedAppointmentTimes, $externalDepartmentId)
    {
        $lastDay = (DateTime::createFromFormat("Y-m-d", date('Y-m-d'))
            ->add(new \DateInterval("P28D"))
            ->setTime(0, 0, 0, 0));

        if ($date >= $lastDay) {
            return false;
        }

        foreach ($physicianCachedAppointmentTimes as $key => $times) {
            if (preg_match('/.*_appointment_times_rt_' . $date->format("Y-m-d") . '/', $key)) {
                if (!empty($times)) {
                    foreach ($times as $time) {
                        if (
                            $time['VisitType']['ID'] == $this->chosenVisitTypeCode
                            && $time['Department']['ID'] == $externalDepartmentId
                        ) {
                            return $time['Time'];
                        }
                    }
                }
                continue;
            }
        }

        $date = $date
            ->add(new \DateInterval("P1D"))
            ->setTime(0, 0, 0, 0);

        return $this->endOfDay($date, $physicianCachedAppointmentTimes, $externalDepartmentId);
    }

    /**
     * This method prepares the recommended physician objects.
     * The end-result physician objects include physician title, list of services, ratings,
     * external department ID, national provider identifier, CMS entry ID and physician's image.
     *
     * @param array $physicians - list of physicians and their multiple locations grouped by the common city name
     * @return array
     */
    private function simplifyPhysicianObjects($physicians)
    {
        $view = Craft::$app->getView();
        return \array_map(
            function ($physician) use ($view) {
                $externalDepartmentId = $physician['externalDepartmentId'];
                $physician = $physician['physician'];

                $ratings = $view->renderTemplate('_physician-stars.twig', [
                    'physician' => $physician,
                    'includeResponseCount' => true,
                    'responseCountFormat' => '({responseCountTotal})'
                ]);

                // remove ancillary services
                $validServices = \array_filter(
                    $physician->physicianSpeciality->all(),
                    function ($physician) {
                        if ($physician->type != "ancillaryServices") {
                            return $physician;
                        }
                    }
                );

                $services = \array_map(fn ($service) => $service->title, $validServices);
                \sort($services);

                $image = $physician->physicianHeadshot->one();

                $address = null;
                $locationEntry = SchedulingModule::getInstance()
                    ->schedulingModuleService
                    ->findSuiteEntryForGivenExternalDepartmentID($externalDepartmentId);

                if ($locationEntry !== null) {
                    $addressParts = $locationEntry->parent->address->parts;
                    $address = [
                        'addressLine' => "{$addressParts->number} {$addressParts->address}",
                        'cityStateAndZip' => "{$addressParts->city}, {$addressParts->state} {$addressParts->postcode}",
                    ];
                }

                $this->physicianDetails[$physician->id] = [
                    'title' => $physician->title,
                    'services' => $services,
                    'ratings' => $ratings,
                    'address' => $address,
                    'externalDepartmentId' => $externalDepartmentId,
                    'nextAppointmentDate' =>  DateTime::createFromFormat('Y-m-d H:i:s', $physician->getDirtyAttributes()[0])->format('Y-m-d'),
                    'nationalProviderIdentifier' => $physician->nationalProviderIdentifier,
                    'id' => $physician->id,
                    'image' => $image ? DupageCoreModule::getInstance()
                        ->dupageCoreModuleService
                        ->getOptimizedImage($image, 'webp', false, [
                            ['settings' => ['gravity:sm', 'resize:fill:120:120:1:1'], 'css' => '(min-width: 200px)']
                        ]) : '<div class="image default-headshot thumbnail no-margin"></div>'
                ];

                return $this->physicianDetails[$physician->id];
            },
            $physicians
        );
    }

    /**
     * This method returns a list of external department IDs from given locations, for given physician and scheduling services.
     *
     * For example, if a physician X is associated to location Y, and location Y has a suite Y1,
     * and suite Y1 offers service Z with the external department ID 12345,
     * then this function would return [12345] for given X, Y and Z.
     *
     * @param array $locations - list of locations
     * @param Entry $physicianphysician - chosen physician Craft Entry
     * @param array $schedulingServices - list of services chosen for the current scheduling flow
     * @return array
     */
    private function getExternalDepartmentIDsForPhysicianFromLocations($physician, $schedulingServiceIds, $validLocations)
    {
        $externalDepartmentIds = [];
        $physicianSuites = $physician->physicianLocations->with(['suite','suiteServices'])->all();

        $validLocationIds = \array_map(fn ($location) => $location->id, $validLocations);

        foreach ($physicianSuites as $suite) {
            if (!\in_array($suite->id, $validLocationIds)) {
                continue;
            }
            foreach ($suite['suiteServices'] as $suiteService) {
                if (!$suiteService->serviceType) {
                    continue;
                }
                if (
                    \in_array($suiteService->serviceType->one()->id, $schedulingServiceIds)
                    && !\in_array($suiteService->externalDepartmentId, $externalDepartmentIds)
                ) {
                    $externalDepartmentIds[] = $suiteService->externalDepartmentId;
                    break;
                }
            }
        }
        return $externalDepartmentIds;
    }
}
