<?php

namespace modules\apimodule\forms;

use Craft;
use DateTime;
use League\Geotools\Coordinate\Coordinate;
use League\Geotools\Geotools;
use craft\elements\Category;
use craft\elements\Entry;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\models\Locations;
use modules\schedulingmodule\services\SchedulingModuleEpicService;
use modules\apimodule\ApiModule;
use modules\locationsmodule\LocationsModule;
use modules\physiciansmodule\PhysiciansModule;
use yii\base\Model;
use yii\data\ArrayDataProvider;
use yii\data\Sort;
use yii\web\UrlManager;

/**
 * Physician Form
 *
 * This form wraps the logic for requesting a list of physicians based on provided parameters.
 *
 * This form is tailored for use in /api endpoints.
 */
final class PhysicianForm extends Model
{
    /**
     * @var int[] $service_id
     */
    public $service_id;

    /**
     * @var int $zip_code
     */
    public $zip_code;

    /**
     * @var float $latitude
     */
    public $latitude;

    /**
     * @var float $longitude
     */
    public $longitude;

    /**
     * @var int $pageSize
     */
    public $pageSize = 15;

    /**
     * @var int $visit_reason_id
     */
    public $visit_reason_id;

    /**
     * @var int $is_new_patient
     */
    public $is_new_patient;

    /**
     * @var Entry[] $serviceEntries
     */
    private $serviceEntries = [];

    /**
     * @inheritdoc
     */
    public function load($data, $formName = null)
    {
        if (!parent::load($data, $formName)) {
            return false;
        }

        if (isset($this->service_id)) {
            if (!\is_array($this->service_id)) {
                $this->service_id = [$this->service_id];
            }
            $services = [];
            foreach ($this->service_id as $serviceId) {
                $services[] = Entry::find()->id($serviceId)->one();
            }
            $this->setServiceEntries($services);
        }

        if (isset($this->is_new_patient)) {
            $this->is_new_patient = $this->is_new_patient === 'true' ? true: false;
        }

        return true;
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['latitude', 'longitude', 'zip_code', 'visit_reason_id'], 'number'],
            [['service_id'], 'each', 'rule' => ['number']],
            [['is_new_patient'], 'boolean'],
            [['visit_reason_id'], 'required', 'when' => fn ($model) => $this->serviceRequiresVisitReasonId($model)],
            [['service_id'], 'required'],
            [['service_id'], 'validateServiceId'],
            [['visit_reason_id'], 'validateVisitReasonId'],
            [['service_id', 'latitude', 'longitude', 'zip_code', 'visit_reason_id', 'is_new_patient'], 'safe']
        ];
    }

    /**
     * Determines if the visit reason parameter is required.
     *
     * Certain specialties will require a reason for visit.
     * This enforces that when such a service is supplied, a reason for visit is supplied as well.
     *
     * @return boolean
     */
    private function serviceRequiresVisitReasonId($model)
    {
        if (count($this->getServiceEntries()) == 0) {
            return false;
        }

        foreach ($this->getServiceEntries() as $service) {
            if (
                ($service->appointmentSchedulingReasonsForVisit->count()) > 0
                && !isset($model->visit_reason_id)
            ) {
                $this->addError('visit_reason_id', 'Visit Reason Id is required for this service');
                return true;
            }
        }

        return false;
    }

    /**
     * Validates the provided visit reason id.
     *
     * Certain specialties will require a reason for visit.
     * This enforces that when such a service is supplied, it is valid.
     *
     * @return boolean
     */
    public function validateServiceId()
    {
        foreach ($this->getServiceEntries() as $service) {
            if ($service && !$service->allowOnlineScheduling) {
                $this->addError('service_id', 'This service does not offer scheduling.');
                return false;
            }
        }
            
        return true;
    }

    /**
     * Validates the provided visit reason id.
     *
     * Certain specialties will require a reason for visit.
     * This enforces that when such a service is supplied, it is valid.
     *
     * @return boolean
     */
    public function validateVisitReasonId()
    {
        foreach ($this->getServiceEntries() as $service) {
            if (
                $this->visit_reason_id
                && !\in_array($this->visit_reason_id, $service->appointmentSchedulingReasonsForVisit->ids())
            ) {
                $this->addError('visit_reason_id', 'Invalid Visit Reason Id for this service.');
                return false;
            }
        }

        return true;
    }

    /**
     * This method returns a list of location objects and their data.
     *
     * @return array
     */
    public function getPhysicians()
    {
        if (!$this->validate()) {
            return [];
        }

        $physicians = Entry::find()
            ->section('physicians');

        // get physicians related to this service and accepting this visit reason (if applicable)
        $physicians->relatedTo(\array_merge(['or'], $this->getServiceEntries()));

        if (isset($this->visit_reason_id)) {
            $physicians->andRelatedTo($this->visit_reason_id);
        }

        // get suites offering this service, and which are assigned to these physicians
        // $locationsForPhysicians = Locations::getLocationsForPhysiciansQuery($physicians->ids());
        $locationsForPhysicians = Entry::find()->section('locations')->type('suite')->relatedTo($physicians->ids());

        $attributes = $this->getAttributes();
        unset($attributes['zip_code']);
        unset($attributes['latitude']);
        unset($attributes['longitude']);
        $hashSlot = hash('sha512', 'api_form__physicians__ ' . igbinary_serialize($attributes));
        $finalPhysiciansList = Craft::$app->cache->getOrSet($hashSlot, function () use ($locationsForPhysicians, $physicians) {
            $finalPhysiciansList = [];
            foreach ($locationsForPhysicians->distinct()->each() as $suite) {
                $result = [];

                if ($suite === null) {
                    continue;
                }

                $parentLocation = $suite->parent;
                
                // populate location information from the suite's parent
                $result['address'] = [
                    'number' => $parentLocation->address->parts->number,
                    'street' => $parentLocation->address->parts->address,
                    'city' => $parentLocation->address->parts->city,
                    'postcode' => $parentLocation->address->parts->postcode,
                    'county' => $parentLocation->address->parts->country,
                    'state' => $parentLocation->address->parts->state,
                    'suite' => $suite->title,
                    'lat' => (float)$parentLocation->address->lat,
                    'lng' => (float)$parentLocation->address->lng,
                ];

                // generate a query to find physicians for this suite
                $locationPhysiciansQuery = clone $physicians;
                $locationPhysiciansQuery->relatedTo(\array_merge(['or'], $this->getServiceEntries()))
                    ->andRelatedTo($suite->id);

                if (isset($this->visit_reason_id)) {
                    $locationPhysiciansQuery->andRelatedTo($this->visit_reason_id);
                }

                // pre-load physicianSpeciality data
                $locationPhysiciansQuery = $locationPhysiciansQuery->with(['physicianSpeciality']);

                foreach ($locationPhysiciansQuery->unique()->each() as $physician) {
                    // if physician was already processed, skip
                    if (array_search($physician->id, array_column($finalPhysiciansList, 'id')) !== false) {
                        continue;
                    }

                    $appointmentService = null;
                    $externalDepartmentID = null;
                    if ($suite->suiteServices === null) {
                        continue;
                    }

                    // Since we support multiple services on this endpoint, we need to find the correct external department ID.
                    // For given suite, and for given physician, we have to identify the specialty that applies to both,
                    // find the correct suite servivce at that suite, and then grab that external department ID.
                    //
                    // Not doing this can result in displaying appointment times for a service for this physician,
                    // but from a wrong location, which is inaccurate data.
                    foreach ($suite->suiteServices as $suiteServices) {
                        $service = $suiteServices->serviceType->one();
                        if ($service === null) {
                            continue;
                        } elseif (\in_array($service->id, array_map(fn ($s) => $s->id, $physician->physicianSpeciality))) {
                            $appointmentService = $service;
                            $externalDepartmentID = $suiteServices->externalDepartmentId;
                        }
                    }

                    if ($appointmentService === null) {
                        continue;
                    }

                    $slots = $this->getAppointmentSlots($physician, $appointmentService, $externalDepartmentID);
                    if (count($slots) === 0) {
                        continue;
                    }

                    $rating = DupageCoreModule::getInstance()->dupageCoreModuleService->getProviderRatingAndComments($physician);

                    // If there are no slots for this physician don't bother showing anything
                    if (count($slots) === 0) {
                        continue;
                    }

                    $providerIdAndIdType = SchedulingModuleEpicService::getProviderIdAndIdType($physician);

                    $result['id'] = $physician->id;
                    $result['title'] = $physician->title;
                    $result['provider_id'] = $providerIdAndIdType['providerID'];
                    $result['provider_id_type'] = $providerIdAndIdType['providerIDType'];
                    $result['appointment_slots'] = $slots;
                    $result['url'] = $physician->url;
                    $result['rating'] = $rating['overallRating']['value'] ?? null;
                    $result['distance'] = 0;
                    $result['service_id'] = $appointmentService->id;
                    $finalPhysiciansList[] = $result;
                }
            }

            return $finalPhysiciansList;
        }, 900);

        // convert user's zip code into a lat & long pair
        if ($this->zip_code) {
            $coordinates = LocationsModule::getInstance()
                ->locationsModuleService
                ->getLatLngForAddress($this->zip_code);
            $this->latitude = $coordinates['lat'];
            $this->longitude = $coordinates['lng'];
        }

        if ($this->latitude !== null && $this->longitude !== null) {
            $geotools = new Geotools();
            $origin   = new Coordinate([$this->latitude, $this->longitude]);
            // calculate physician's location distance from the user
            foreach ($finalPhysiciansList as &$element) {
                $physicianCoordinates = new Coordinate([$element['address']['lat'], $element['address']['lng']]);
                $distance = $geotools->distance()
                    ->setFrom($physicianCoordinates)
                    ->setTo($origin)
                    ->in('mi')
                    ->haversine();

                $element['distance'] = $distance;
            }
        }

        // create an array data provider to manage the final list of physicians
        // this takes care of the pagination for us
        $dataProvider = new ArrayDataProvider([
            'allModels' => $finalPhysiciansList,
            'sort' => new Sort([
                'defaultOrder' => [
                    'distance' => SORT_ASC
                ],
                'attributes' => [
                    'distance'
                ]
            ]),
            'pagination' => [
                'pageSize' => $this->pageSize,
                'totalCount' => count($finalPhysiciansList)
            ]
        ]);

        // convert physician Models into simple objects with only supported keys
        return $dataProvider;
    }

    /**
     * This method returns cached appointment times for given physician.
     * It takes into account any additionally supplied form parameters and tailors the results.
     */
    private function getAppointmentSlots(Entry &$physician, $appointmentService, $externalDepartmentID)
    {
        $slots = [];

        // get cached appointment times for this service at this location for this external resource ID
        $appointmentSlotsPerDay = PhysiciansModule::getInstance()
                ->physiciansModuleService
                ->getCachedAppointmentTimesForPhysicianFromDate($physician, new DateTime());

        $visitTypeCode = ApiModule::getInstance()
                ->apiModuleService
                ->getVisitTypeCodeForService($appointmentService->id, $this->is_new_patient ?? false);

        foreach ($appointmentSlotsPerDay as $appointmentSlots) {
            // if there are no appointments for this day key, skip
            if (!$appointmentSlots) {
                continue;
            }
            foreach ($appointmentSlots as $appointmentSlot) {
                // skip appointment slots with non-matching visit type code
                if ($appointmentSlot['VisitType']['ID'] != $visitTypeCode) {
                    continue;
                }

                // skip appointment slots with non-matching department ID
                if ($appointmentSlot['Department']['ID'] != $externalDepartmentID) {
                    continue;
                }

                // skip appointment slots older than now; potential side-effect of using cached data
                if ($appointmentSlot['Time'] < new DateTime()) {
                    continue;
                }

                // skip appointment slots if we already have 27
                if (count($slots) > 27) {
                    break;
                }

                // if all else, add the time slot
                $slots[] = $appointmentSlot['Time']->format('c');
            }
        }

        return $slots;
    }

    /**
     * Get $serviceEntries
     *
     * @return  Entry[]
     */
    public function getServiceEntries()
    {
        return $this->serviceEntries;
    }

    /**
     * Set $serviceEntries
     *
     * @param  Entry[]  $serviceEntries  $serviceEntries
     *
     * @return  self
     */
    public function setServiceEntries(array $serviceEntries)
    {
        $this->serviceEntries = $serviceEntries;

        return $this;
    }
}
