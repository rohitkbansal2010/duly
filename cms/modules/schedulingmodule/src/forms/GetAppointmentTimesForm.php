<?php

namespace modules\schedulingmodule\forms;

use Craft;
use craft\elements\Category;
use craft\elements\Entry;
use DateInterval;
use DatePeriod;
use DateTime;
use modules\physiciansmodule\PhysiciansModule;
use modules\physiciansmodule\forms\PhysicianSearchForm;
use modules\DupageCoreModule\DupageCoreModule;
use modules\schedulingmodule\SchedulingModule;
use modules\schedulingmodule\forms\RecommendedPhysicians;
use samdark\log\PsrMessage;
use yii\base\Model;

use function PHPUnit\Framework\returnSelf;

final class GetAppointmentTimesForm extends Model
{
    /**
     * @var string $physicianId
     */
    public $physicianId;

    /**
     * @var string $startDate
     */
    public $startDate;

    /**
     * @var string $daysInterval
     */
    public $daysInterval;

    /**
     * @var string $selectedServiceId
     */
    public $selectedServiceId;

    /**
     * @var string $newPatient
     */
    public $newPatient;

    /**
     * @var string $followUpVisit
     */
    public $followUpVisit;

    /**
     * @var string $veinClinicVisit
     */
    public $veinClinicVisit;

    /**
     * @var string $fullBodySkinExamVisit
     */
    public $fullBodySkinExamVisit;

    /**
     * @var string $videoVisitTypeOnly
     */
    public $videoVisitTypeOnly;

    /**
     * @var Entry $selectedService
     */
    private $selectedService;

    /**
     * @var Entry $selectedPhysician
     */
    private $selectedPhysician;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['physicianId', 'startDate', 'daysInterval', 'selectedServiceId', 'newPatient', 'followUpVisit', 'veinClinicVisit', 'videoVisitTypeOnly', 'fullBodySkinExamVisit'], 'string'],
            ['selectedServiceId', 'validateService'],
            ['physicianId', 'validatePhysician'],
            ['daysInterval', 'number', 'max' => 120],
            ['startDate', 'date', 'format' => 'php:Y-m-d'],
            [['physicianId', 'startDate'], 'required'],
            [['physicianId', 'startDate', 'daysInterval', 'selectedServiceId', 'newPatient', 'followUpVisit', 'veinClinicVisit', 'videoVisitTypeOnly', 'fullBodySkinExamVisit'], 'safe']
        ];
    }

    /**
     * Validates service
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateService($attribute, $param)
    {
        $result = Entry::find()
            ->section('services')
            ->id($this->$attribute)
            ->one();

        $this->selectedService = $result;

        return true;
    }

    /**
     * Validates physician
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validatePhysician($attribute, $param)
    {
        $result = Entry::find()
            ->section('physicians')
            ->id($this->$attribute)
            ->one();


        $this->selectedPhysician = $result;

        return true;
    }

    /**
     * Returns selected physician Entry object.
     *
     * @return boolean
     */
    public function getSelectedPhysician()
    {
        return $this->selectedPhysician;
    }

    /**
     * Returns selected service Entry object.
     *
     * @return object
     */
    public function getSelectedService()
    {
        $selectedService = $this->selectedService;

        if ($selectedService) {
            return $selectedService;
        } elseif (isset(Craft::$app->patient_user)) {
            $appointmentServiceIds = Craft::$app->patient_user->identity->getAppointmentServiceIds();
            if (!empty($appointmentServiceIds)) {
                return Entry::find()
                    ->section('services')
                    ->id($appointmentServiceIds[0])
                    ->one();
            }

            return null;
        }

        return null;
    }

    /**
     * Get $newPatient
     *
     * @return  string
     */
    public function isNewPatient()
    {
        return $this->newPatient ?? Craft::$app->patient_user->identity->appointment_new_patient_visit ?? false;
    }

    /**
     * Get $followUpVisit
     *
     * @return  string
     */
    public function isFollowUpVisit()
    {
        return $this->followUpVisit ?? Craft::$app->patient_user->identity->appointment_follow_up_visit ?? false;
    }

    /**
     * Get $veinClinicVisit
     *
     * @return  string
     */
    public function isVeinClinicVisit()
    {
        return $this->veinClinicVisit ?? Craft::$app->patient_user->identity->appointment_vein_clinic_visit ?? false;
    }

    /**
     * Get $fullBodySkinExamVisit
     *
     * @return  string
     */
    public function isFullBodySkinExamVisit()
    {
        return $this->fullBodySkinExamVisit ?? Craft::$app->patient_user->identity->appointment_full_body_skin_exam_visit ?? false;
    }

    /**
     * Returns a list of appointment times.
     */
    public function getAppointmentTimes($providers, $physicianCmsEntry = null, $startDate = null, $endDate = null)
    {
        Craft::info(new PsrMessage('time.', [
            'time' => round(microtime(true) * 1000),
        ]), get_class($this) . '::' . __METHOD__);
        $userSelectedServiceID = $this->getSelectedService()->id ?? null;

        // get all valid visit type IDs for this physician
        $epicVisitTypeIDs = SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getAllVisitTypeIDs($physicianCmsEntry);

        Craft::info(new PsrMessage('time.', [
            'time' => round(microtime(true) * 1000),
        ]), get_class($this) . '::' . __METHOD__);

        // start and end dates
        $startDate = $startDate ?? DateTime::createFromFormat("Y-m-d", $this->startDate);
        $endDate = $endDate ?? $this->generateEndDate($startDate, $this->daysInterval);

        Craft::info(new PsrMessage('epicVisitTypeIDs.', [
            'epicVisitTypeIDs' => $epicVisitTypeIDs,
            'startDate' => $startDate,
            'endDate' => $endDate,
        ]), get_class($this) . '::' . __METHOD__);

        $data = SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getPhysicianAppointmentTimes(
                $startDate,
                $endDate,
                $providers,
                $epicVisitTypeIDs,
                $physicianCmsEntry
            );

        Craft::info(new PsrMessage('time.', [
            'time' => round(microtime(true) * 1000),
        ]), get_class($this) . '::' . __METHOD__);

        return $data;
    }

    public function processAppointmentTimes($data, $user, $selectedServiceId, $selectedPhysician, $externalResrouceProvider = null)
    {
        $service = Entry::find()->id($selectedServiceId)->one();

        if ($service === null) {
            Craft::error(new PsrMessage('Unable to process available appointment times. Invalid service ID.', [
                'service_id' => $selectedServiceId
            ]));
            return [];
        }

        // start and end dates
        $startDate = DateTime::createFromFormat("Y-m-d", $this->startDate);
        $endDate = $this->generateEndDate($startDate, $this->daysInterval);
        $endDate = $endDate->add(new DateInterval("P1D"));

        if ($user->appointment_new_patient_visit == "1") {
            // set varaiable to show recomended providers if it is set in this service for new patients
            $showRecommendedProviders = $service->recommendedProvidersNewPatients ? 1 : null;
        } else {
            // set varable to show recommended providers if it is set in this service for established patients
            $showRecommendedProviders = $service->recommendedProvidersEstablishedPatients ? 1 : null;
        }

        $chosenVisitTypeCodes = SchedulingModule::getInstance()->schedulingModuleService->getUsersValidVisitTypeIDs($selectedServiceId);

        // provider ID and type
        if ($externalResrouceProvider) {
            $providerID = strpos($externalResrouceProvider, ',') > 0 ? array_map('trim', explode(',', $externalResrouceProvider)) : [$externalResrouceProvider];
            $providerIDType = 'External';
        } elseif ($selectedPhysician) {
            $providerID = [SchedulingModule::getInstance()->schedulingModuleEpicService->getProviderIdAndIdType($selectedPhysician)['providerID']];
            $providerIDType = SchedulingModule::getInstance()->schedulingModuleEpicService->getProviderIdAndIdType($selectedPhysician)['providerIDType'];
        } else {
            Craft::error(new PsrMessage('Unable to process available appointment times. No external resource ID or physician set.', [
                'appointment_visit_type_id' => $user->appointment_visit_type_id,
                'service_id' => $selectedServiceId
            ]));
            return [];
        }

        $interval = DateInterval::createFromDateString('1 day');
        $period = new DatePeriod($startDate, $interval, $endDate);

        // bootstrap response object
        $response = [
            'currentAppointments' => [],
            'futureAppointments' => [],
            'recommendedPhysicians' => []
        ];
        $firstDayFromPeriod = null;

        $post = Craft::$app->request->post();

        $findOtherRecommendedPhysicians = isset($post['findOtherRecommendedPhysicians']) ? $post['findOtherRecommendedPhysicians'] : false;

        if ($findOtherRecommendedPhysicians) {
            $user->setAppointmentServiceIds([$selectedServiceId]);
            $user->eligible_physicians =  SchedulingModule::getInstance()->schedulingModuleService->queryEligiblePhysicians(new PhysicianSearchForm(), $user->getAppointmentServiceIds(), null, $user->appointment_reason_for_visit_id, $user->appointment_new_patient_visit, $user->location_id, $user->appointment_insurance_plan_medicare_advantage === '1');
            $user->save();    
        }

        $recommendedPhysiciansForm = new RecommendedPhysicians();
        foreach ($period as $dt) {
            // recommended physicians should be selected based on the first date from the range,
            // and they should not change for the rest of the returned days, per AC
            $firstDayFromPeriod = $firstDayFromPeriod ?? $dt;
            $response['currentAppointments'][$dt->format("Y-m-d")] = [];
            $response['futureAppointments'][$dt->format("Y-m-d")] = [];

            // TODO: refactor for later, kept the format for backwards-compatibility
            // Starting now, recommended providers will be a single set, and not different sets per day
            // As such, we don't need to re-calculate everything on every loop iteration
            if (isset($response['recommendedPhysicians'][$firstDayFromPeriod->format("Y-m-d")])) {
                // If we already have the results, we just re-use them for every date key
                $response['recommendedPhysicians'][$dt->format("Y-m-d")] =  $response['recommendedPhysicians'][$firstDayFromPeriod->format("Y-m-d")];
            } else {
                // If we don't yet have it, get recommended providers
                if ($findOtherRecommendedPhysicians) {
                    $response['recommendedPhysicians'][$dt->format("Y-m-d")] = $this->findOtherRecommendedPhysicians($firstDayFromPeriod, $selectedPhysician, $data, $chosenVisitTypeCodes['main'], $showRecommendedProviders, $recommendedPhysiciansForm);
                } else {
                    $response['recommendedPhysicians'][$dt->format("Y-m-d")] = [];
                }
            }
        }

        // processing all of the returned appintments data can create very large loops
        // we must avoid unnecessary DB calls as much as possible
        // every unique query ought to be executed only once, and the results should be stored in memory**
        //
        // **
        // using Redis was attempted, but the approach was not effective due to potentially
        // having to store entire Entry objects in Redis
        // storing only IDs in redis would result in having to do the query all over again
        $suiteForExternalDepartmentID = [];
        $availableTimesFound = [];

        // process available appointment times within given date range
        foreach ($providerID as $pID) {
            if (isset($data[$pID])) {
                foreach ($data[$pID] as $availableTime) {
                    $date = $availableTime['Time']->format("Y-m-d");
                    $availableTime['Time'] = $availableTime['Time']->format('c');

                    $locationKey = $availableTime['Department']['ID'];

                    $availableTimeVisitType = (int)$availableTime['VisitType']['ID'];

                    $isNewPatient = $user->appointment_new_patient_visit == "1" ? true : false;
                    $isFollowUp = $user->appointment_follow_up_visit == "1" ? true : false;
                    $allowNewVideo = $service->videoVisitsNewPatients && $isNewPatient ? true : false;
                    $allowEstablishedVideo = $service->videoVisitsEstablishedPatients && !$isNewPatient ? true : false;

                    if ($isFollowUp) { //its a follow up
                        if ($availableTimeVisitType !== $chosenVisitTypeCodes['main'] && $isNewPatient) { // ignore anything that isn't the follow up for new patients
                            continue; //ignore it
                        } elseif ($availableTimeVisitType !== $chosenVisitTypeCodes['main'] && !$isNewPatient) { // ignore anything that isn't established follow up code
                            continue; //ignore it
                        }
                    } elseif (!$isFollowUp) { // not a follow up visit
                        if ($availableTimeVisitType === 2990) { //this slot is a video type
                            if ($allowNewVideo) { //video visits for new patients allowed
                                $locationKey = "video";
                            } elseif ($allowEstablishedVideo) { //video for established patients allowed
                                $locationKey = "video";
                            } else {
                                continue; // ignore it
                            }
                        } elseif ($chosenVisitTypeCodes['main'] !== $availableTimeVisitType) { //not a follow up, not a video then it has to match delared visit or it is ignored
                            continue; //ignore it
                        }
                    }

                    $response['currentAppointments'][$date][$locationKey] = $response['currentAppointments'][$date][$locationKey] ?? [];

                    // utilize the memoized results
                    $suiteForExternalDepartmentID[$locationKey] = $suiteForExternalDepartmentID[$locationKey] ?? SchedulingModule::getInstance()->schedulingModuleService->findSuiteEntryForGivenExternalDepartmentID($locationKey);
                    $suite = $suiteForExternalDepartmentID[$locationKey];

                    $location = $suite->parent ?? null;

                    if ($location) {
                        $availableTime['Department']['address'] = [
                            'address-line' => $location->address->parts->number . " " . $location->address->parts->address,
                            'city' => $location->address->parts->city,
                            'state' => $location->address->parts->state,
                            'zip' => $location->address->parts->postcode,
                            'lat' => $location->address->lat,
                            'lng' => $location->address->lng,
                        ];
                    }

                    $availableTimesFound[$locationKey] = $availableTimesFound[$locationKey] ?? [];
                    if (!in_array($availableTime['Time'], $availableTimesFound[$locationKey])) {
                        \array_push($availableTimesFound[$locationKey], $availableTime['Time']);
                        \array_push($response['currentAppointments'][$date][$locationKey], $availableTime);
                        \usort($response['currentAppointments'][$date][$locationKey], fn($a, $b) => $a['Time'] <=> $b['Time']);
                    }
                }
            }
        }

        // if given location/video for a given date has no available times
        // we need to look through any cached available appointment dates to try to find the next available date!
        foreach ($response['futureAppointments'] as $date => $times) {
            foreach ($providerID as $pID) {
                $physicianLocations = Craft::$app->cache->get("physician_{$pID}_external_department_ids");

                $isNewPatient = $user->appointment_new_patient_visit == "1" ? true : false;
                $isFollowUp = $user->appointment_follow_up_visit == "1" ? true : false;
                $allowNewVideo = $service->videoVisitsNewPatients && $isNewPatient ? true : false;
                $allowEstablishedVideo = $service->videoVisitsEstablishedPatients && !$isNewPatient ? true : false;

                if ($allowEstablishedVideo || $allowNewVideo) {
                    $physicianLocations[] = 'video';
                }
                if ($externalResrouceProvider !== null) {
                    $suiteEntry = SchedulingModule::getInstance()
                        ->schedulingModuleService
                        ->findSuiteEntryForGivenExternalProviderResourceID($externalResrouceProvider);
                    if ($suiteEntry) {
                        $physicianLocations = [[
                            'ID' => $suiteEntry->id,
                            'Type' => 'External'
                        ]];
                    }
                }
                foreach ($physicianLocations as $physicianLocation) {
                    $isVideo = $physicianLocation == 'video';
                    $physicianLocationId = !$isVideo ? $physicianLocation['ID'] : $physicianLocation;

                    // if there are appointment times for givevn location at this date, continue
                    $skipLocation = isset($response['currentAppointments'][$date][$physicianLocationId]);
                    // also skip video visits got annual physisical appointment visits
                    $skipLocation = $skipLocation || $isVideo && $user->appointment_follow_up_visit == "1";

                    if ($externalResrouceProvider !== null && count($response['currentAppointments'][$date]) > 0) {
                        $skipLocation = true;
                    }

                    if ($skipLocation) {
                        continue;
                    }

                    // find the next available date for this physician, at this location, for selected visit type ID, starting the next day
                    $nextDayDate = (DateTime::createFromFormat("Y-m-d", $date))
                        ->add(new DateInterval("P1D"))
                        ->setTime(0, 0, 0, 0);

                    $nextAvailableDate = null;

                    if ($externalResrouceProvider === null) {
                        $nextAvailableDate = PhysiciansModule::getInstance()
                            ->physiciansModuleService
                            ->getNextAppointmentForPhysicianLocationAndVisitTypeIDFromDate(
                                $selectedPhysician,
                                $physicianLocationId,
                                $physicianLocation != 'video' ? $chosenVisitTypeCodes['main'] : $chosenVisitTypeCodes['video'],
                                $nextDayDate
                            );
                    } else {
                        $nextAvailableDate = PhysiciansModule::getInstance()
                            ->physiciansModuleService
                            ->getNextAppointmentForExternalResourceLocationAndVisitTypeIDFromDate(
                                $pID,
                                $physicianLocation != 'video' ? $chosenVisitTypeCodes['main'] : $chosenVisitTypeCodes['video'],
                                $nextDayDate
                            );
                    }

                    if (!$nextAvailableDate) {
                        continue;
                    }

                    // utilize the memoized results
                    if ($externalResrouceProvider === null) {
                        $suiteForExternalDepartmentID[$physicianLocationId] = $suiteForExternalDepartmentID[$physicianLocationId] ?? SchedulingModule::getInstance()->schedulingModuleService->findSuiteEntryForGivenExternalDepartmentID($physicianLocationId);
                        $suite = $suiteForExternalDepartmentID[$physicianLocationId];
                    } else {
                        $suite = $isVideo ? null : Entry::find()->id($physicianLocation['ID'])->one();
                    }

                    $location = $suite->parent ?? null;

                    if ($location) {
                        $nextAvailableDate['Department']['address'] = [
                            'address-line' => $location->address->parts->number . " " . $location->address->parts->address,
                            'city' => $location->address->parts->city,
                            'state' => $location->address->parts->state,
                            'zip' => $location->address->parts->postcode,
                            'lat' => $location->address->lat,
                            'lng' => $location->address->lng,
                        ];
                    }

                    $response['futureAppointments'][$date][$physicianLocationId] = $nextAvailableDate;
                }
            }
        }

        if ($selectedPhysician) {
            $response['physicianDetails'] = $this->getPhysicianDetails($selectedPhysician);
        }

        return $response;
    }

    private function getPhysicianDetails($physician)
    {
        $ratingsAndComments = DupageCoreModule::getInstance()
            ->dupageCoreModuleService
            ->getProviderRatingAndComments($physician);

        $ratings = [];
        if ($ratingsAndComments) {
            $ratings = [
                'overall' => $ratingsAndComments['overallRating']['value'] ?? '',
                'count' => $ratingsAndComments['overallRating']['categoryResponseCount'] ?? ''
            ];
        }

        // remove ancillary services
        $validServices = \array_filter(
            $physician->physicianSpeciality->all(),
            function ($service) {
                if ($service->type != "ancillaryServices") {
                    return $service;
                }
            }
        );

        $services = \array_map(fn ($service) => $service->title, $validServices);
        \sort($services);

        $image = $physician->physicianHeadshot->one();

        return  [
            'title' => $physician->title,
            'services' => $services,
            'ratings' => $ratings,
            'nationalProviderIdentifier' => $physician->nationalProviderIdentifier,
            'id' => $physician->id,
            'image' => $image ? DupageCoreModule::getInstance()
                ->dupageCoreModuleService
                ->getOptimizedImage($image, 'webp', false, [
                    ['settings' => ['gravity:sm', 'resize:fill:120:120:1:1'], 'css' => '(min-width: 200px)']
                ]) : '<div class="image default-headshot thumbnail no-margin"></div>'
        ];
    }

    /**
     * Generates a list of recommended physicians.
     */
    private function findOtherRecommendedPhysicians($date, $selectedPhysician, $appointmentTimes, $chosenVisitTypeCode, $showRecommendedProviders, $recommendedPhysiciansForm)
    {
        // DMG-2063: Add Feature Flag to Recommended Providers LOGGED_IN_COOKIE
        // added a check for the show recommended providers if it is false it returns empty
        if (!\Craft::$app->getGlobals()->getSetByHandle('generalSiteConfig')['showRecommendedPhysicians'] || $showRecommendedProviders == false) {
            return [];
        }

        // DMG-2181: Disables recommended providers for clearstep physicians
        if (Craft::$app->patient_user->identity->deeplinked_session !== null) {
            return [];
        }

        $recommendedPhysiciansForm->load([
            'RecommendedPhysicians' => [
                'date' => $date->format('D M j g:ia'),
                'user' => Craft::$app->patient_user->identity,
                'selectedPhysician' => $selectedPhysician,
                'chosenVisitTypeCode' => $chosenVisitTypeCode
            ]
        ]);

        return $recommendedPhysiciansForm->getPhysicians();
    }

    private function generateEndDate($startDate, $daysInterval)
    {
        $endDate = clone $startDate;
        if ($daysInterval) {
            $daysInterval = (int)$daysInterval - 1;
            $endDate->add(new DateInterval("P{$daysInterval}D"));
        } else {
            $endDate->add(new DateInterval("P6D"));
        }
        return $endDate;
    }
}
