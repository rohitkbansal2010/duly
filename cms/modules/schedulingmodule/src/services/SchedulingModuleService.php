<?php

/**
 * Scheduling module for Craft CMS 3.x
 *
 * Allows for extended management of the scheduling section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\schedulingmodule\services;

use Craft;
use craft\base\Component;
use craft\elements\Category;
use craft\elements\db\EntryQuery;
use craft\elements\Entry;
use craft\elements\MatrixBlock;
use DateTime;
use ether\simplemap\services\GeoService;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\models\PatientUser;
use modules\physiciansmodule\forms\PhysicianSearchForm;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\forms\CalendarForm;
use modules\schedulingmodule\SchedulingModule;
use yii\db\ArrayExpression;
use yii\db\Expression;
use yii\db\Query;

/**
 * @author    Wipfli Digital
 * @package   SchedulingModule
 * @since     1.0.0
 */
class SchedulingModuleService extends Component
{
    // 1 hour
    public static $VIDEO_VISIT_EXTERNAL_VISIT_TYPE_ID = "2990";

    // Public Methods
    // =========================================================================

    /**
     * Returns a list of entry titles containing the search criteria for given entry type
     *
     * @param string $search A search term used for listing services, procedures, and conditions based on their names
     * @return array
     */
    public function getMatchingEntries(string $search = null): array
    {
        // if search term relates to a condition or procedure
        $conditionsAndProcedures = Entry::find()
            ->section(['conditions', 'procedures'])
            ->search("title:*" . $search . "*");

        // only want the parent service
        $relatedIds = Entry::find()
            ->section('services')
            ->relatedTo([
                'targetElement' => $conditionsAndProcedures
            ])
            ->unique()
            ->ids();

        // if search term relates to the actual service
        $services = Entry::find()
            ->section('services')
            ->where(['like', 'title', $search])
            ->orWhere(['like', 'field_alternativeSearchServiceName', $search])
            ->unique()
            ->all();

        $serviceIds = [];

        // should display parents if service is a subservice that doesn't allow online scheduling
        foreach ($services as $singleService) {
            $parent = $singleService->getParent();
            $allowsOnlineScheduling = $singleService->allowOnlineScheduling;

            if ($parent != null && $allowsOnlineScheduling == false) {
                \array_push($serviceIds, $parent->id);
            } else {
                \array_push($serviceIds, $singleService->id);
            }
        }

        $allIds = array_merge($relatedIds, $serviceIds);

        // $allIds represent services along with any related conditions or procedures
        $servicesProceduresConditions = Entry::find()
            ->section('services')
            ->id($allIds)
            ->unique()
            ->all();

        // sort the services by title
        \usort(
            $servicesProceduresConditions,
            function ($a, $b) {
                return $a['title'] > $b['title'];
            }
        );

        return $servicesProceduresConditions;
    }

    /**
     * Returns a bool representing if physician is assigned to reason for visit selected
     *
     * @return bool
     */
    public function showReasonErrorModalForPhysician(int $physicianId, int $reasonId): bool
    {
        $reason = Entry::find()
            ->section('serviceReasonsForVisit')
            ->id($reasonId)
            ->one();

        $physician = Entry::find()
            ->section('physicians')
            ->id($physicianId)
            ->relatedTo(['sourceElement' => $reason])
            ->one();

        if ($physician != null) {
            return false;
        }

        return true;
    }

    /**
     * Returns a bool representing if a location is assigned to reason for visit selected
     *
     * @return bool
     */
    public function showReasonErrorModalForLocation(int $locationId, int $reasonId): bool
    {
        $reason = Entry::find()
            ->section('serviceReasonsForVisit')
            ->id($reasonId)
            ->one();

        // find suites of the selected parent location ID
        $suiteIds = Entry::find()
            ->section('locations')
            ->descendantOf($locationId)
            ->ids();

        $location = Entry::find()
            ->section('locations')
            ->type('suite')
            ->id($locationId)
            ->relatedTo(['sourceElement' => $reason])
            ->id($suiteIds)
            ->one();

        if ($location != null) {
            return false;
        }

        return true;
    }

    /**
     * Returns a list of services
     *
     * @return array
     */
    public function getServices(): array
    {
        // client wants all primary services to show, regardless of if online scheduling is enabled or not
        $services = Entry::find()
            ->section('services')
            ->level(1)
            ->type(['services', 'ancillaryServices', 'externalService', 'cosmetic'])
            ->all();

        // client only wants subservices to show if they have online scheduling enabled
        $subServices = Entry::find()
            ->section('services')
            ->level(2)
            ->where(['field_allowOnlineScheduling' => true])
            ->all();

        // client only wants tertiary services to show if they have online scheduling enabled
        $tertiaryServices = Entry::find()
            ->section('services')
            ->level(3)
            ->where(['field_allowOnlineScheduling' => true])
            ->all();

        $allServices = \array_merge($services, $subServices, $tertiaryServices);

        // sort the services by title
        \usort(
            $allServices,
            function ($a, $b) {
                return $a['title'] > $b['title'];
            }
        );

        return $allServices;
    }

    /**
     * Returns eligble visit type codes for each appointment type.
     * All visit types are configurable within the CP.
     *
     * @param int $serviceId service ID
     * @return array
     */
    public function getSchedulingVisitTypeCodesForService(int $serviceId): array
    {
        $service = Entry::find()->section('services')->id($serviceId)->one();

        $visitTypeCodes = [
            'newPatient' => null,
            'establishedPatient' => null,
            'followUpVisit' => null,
            'followUpVisitNewPatient' => null,
            'followUpVisitMedicare' => null,
            'veinClinic' => null,
            'fullBodySkinExam' => null
        ];

        // if invalid service, return nulls
        if (!$service) {
            return $visitTypeCodes;
        }

        $globalVisitTypeCodes = $this->getGlobalSchedulingVisitTypeCodes();

        $visitTypeCodes['newPatient'] = $globalVisitTypeCodes['newPatient']['default'];
        foreach ($globalVisitTypeCodes['newPatient']['byService'] as $globalVisitTypeCode) {
            if (isset($globalVisitTypeCode['serviceIds']) && \in_array($serviceId, $globalVisitTypeCode['serviceIds'])) {
                $visitTypeCodes['newPatient'] = $globalVisitTypeCode['visitTypeId'];
                break;
            }
        }

        $visitTypeCodes['establishedPatient'] = $globalVisitTypeCodes['establishedPatient']['default'];
        foreach ($globalVisitTypeCodes['establishedPatient']['byService'] as $globalVisitTypeCode) {
            if (isset($globalVisitTypeCode['serviceIds']) && \in_array($serviceId, $globalVisitTypeCode['serviceIds'])) {
                $visitTypeCodes['establishedPatient'] = $globalVisitTypeCode['visitTypeId'];
                break;
            }
        }

        $visitTypeCodes['followUpVisit'] =  $service->allowFollowUpVisits ? $globalVisitTypeCodes['followUpVisit'] : null;
        $visitTypeCodes['followUpVisitNewPatient'] =  $service->allowFollowUpVisits ? $globalVisitTypeCodes['followUpVisitNewPatient'] : null;
        $visitTypeCodes['followUpVisitMedicare'] = $service->allowFollowUpVisits ? $globalVisitTypeCodes['followUpVisitMedicare'] : null;
        $visitTypeCodes['veinClinic'] = $globalVisitTypeCodes['veinClinicCode'];
        $visitTypeCodes['fullBodySkinExam'] = $globalVisitTypeCodes['fullBodySkinExamCode'];
        // toggle results for videos on services

        return $visitTypeCodes;
    }

    public function getGlobalSchedulingVisitTypeCodes()
    {
        return Craft::$app->cache->getOrSet("scheduling_visit_type_codes", function () {
            // get global settings
            $appointmentSchedulingVisitTypeCodes = Craft::$app->globals->getSetByHandle('appointmentSchedulingVisitTypeCodes');

            // begin with default codes for new and established patients
            $visitTypeCodes['newPatient']['default'] = $appointmentSchedulingVisitTypeCodes->newPatientDefaultVisitTypeCode;
            $visitTypeCodes['newPatient']['byService'] = [];
            $visitTypeCodes['establishedPatient']['default'] = $appointmentSchedulingVisitTypeCodes->establishedPatientDefaultVisitTypeCode;
            $visitTypeCodes['establishedPatient']['byService'] = [];
            $visitTypeCodes['veinClinicCode'] = $appointmentSchedulingVisitTypeCodes->veinClinicCode;
            $visitTypeCodes['fullBodySkinExamCode'] = $appointmentSchedulingVisitTypeCodes->fullBodySkinExamCode;

            // if service allows folow-up visits, use the defaults as well
            $visitTypeCodes['followUpVisit'] = $appointmentSchedulingVisitTypeCodes->annualPhysicalVisitTypeCode;
            $visitTypeCodes['followUpVisitNewPatient'] = $appointmentSchedulingVisitTypeCodes->annualPhysicalNewPatientsVisitTypeCode;
            $visitTypeCodes['followUpVisitMedicare'] = $appointmentSchedulingVisitTypeCodes->annualPhysicalVisitTypeCodeMedicarePlans;


            // figure out if given service has a special "new visit" type code, or if we should use the default
            foreach ($appointmentSchedulingVisitTypeCodes->newPatientServiceSpecificVisitTypeCodes->all() as $newPatientServiceBlock) {
                $currentCodeAndServices = [];

                $currentCodeAndServices['visitTypeId'] = $newPatientServiceBlock->code;
                foreach ($newPatientServiceBlock->services->ids() as $id) {
                    $currentCodeAndServices['serviceIds'][] = $id;
                }

                $visitTypeCodes['newPatient']['byService'][] = $currentCodeAndServices;
            }

            // figure out if given service has a special "new established" type code, or if we should use the default
            foreach ($appointmentSchedulingVisitTypeCodes->establishedPatientServiceSpecificVisitTypeCodes->all() as $establishedPatientServiceBlock) {
                $currentCodeAndServices = [];

                $currentCodeAndServices['visitTypeId'] = $establishedPatientServiceBlock->code;
                foreach ($establishedPatientServiceBlock->services->ids() as $id) {
                    $currentCodeAndServices['serviceIds'][] = $id;
                }

                $visitTypeCodes['establishedPatient']['byService'][] = $currentCodeAndServices;
            }

            return $visitTypeCodes;
        }, 3600 * 24);
    }

    /**
     * Returns false if given patient has already seen a doctor for chosen service.
     *
     * @return bool
     */
    public function shouldAskIfNewPatient(): bool
    {
        $user = Craft::$app->patient_user->identity;

        if (isset($user->appointment_new_patient_visit)) {
            // user already indicated whether new or not
            return false;
        }

        if ($user->scenario == PatientUser::SCENARIO_ANONYMOUS) {
            // anonymous user, might be a new patient
            return true;
        }

        $currentPatientEPI = $user->appointment_current_epi;

        if (!isset($currentPatientEPI) || $currentPatientEPI == "-1") {
            // new user, might be a new patient
            return true;
        }

        $previouslySeenPhysicainIDs = $user->getPatients()[$currentPatientEPI]['pastAppointmentsPhysicianIDs'];
        $currentlySelectedServices = $user->getAppointmentServiceIds();

        if (!\is_array($previouslySeenPhysicainIDs) || count($previouslySeenPhysicainIDs) == 0) {
            // patient with no recorded appointments, might be a new patient
            return true;
        }

        $previousAttendedServices = Entry::find()
            ->section('services')
            ->relatedTo($previouslySeenPhysicainIDs)
            ->ids();

        // if at least one currently-selected service is NOT offfered by previously seen doctors,
        // we cannot assume that they are / are not a new patient,
        // and thus we should show the "new patient" prompt.
        $shouldAskIfNewPatient = count(\array_diff($currentlySelectedServices, $previousAttendedServices)) !== 0;
        if (!$shouldAskIfNewPatient) {
            $user->appointment_new_patient_visit = "0";
            $user->save();
        }
        return $shouldAskIfNewPatient;
    }

    /**
    * Returns a list of schedulable services offered by given physician.
    *
    * @param Entry $physician
    * @return array
    */
    public function getSchedulableServicesForPhysician(Entry $physician, Int $limit = null): array
    {
        return \array_values(
            \array_filter(
                $physician
                    ->physicianSpeciality
                    ->all(),
                function ($service) {
                    return $service->allowOnlineScheduling;
                }
            )
        );
    }

    /**
     * Returns a future start date if applicable
     *
     * @return mixed
     */
    public function determineStartDate()
    {
        $session = Craft::$app->session;

        if ($session != null) {
            if ($session->get('last_menstrual_cycle_year') != null && $session->get('last_menstrual_cycle_month') != null && $session->get('last_menstrual_cycle_day') != null) {
                $lastCycle = DateTime::createFromFormat('Y-m-d', $session->get('last_menstrual_cycle_year') . "-" . $session->get('last_menstrual_cycle_month') . "-" . $session->get('last_menstrual_cycle_day'));
                $today = new DateTime();
                $dateDifference = $today->diff($lastCycle);
                $timePassed = $dateDifference->days;

                // tomorrow's date results in 0
                // any day after tomorrow is negative
                // if timePassed is equal or less than 0, the logic below is bypassed and null is returned
                if ($timePassed >= 0) {
                    // 224 days == 32 weeks
                    if ($timePassed <= 223) {
                        // passes 32 week window

                        // 56 days == 8 weeks
                        if ($timePassed < 56) {
                            // get time difference between the max of 56 days and the submitted date
                            $startAvailableTimesOnDate = $lastCycle->modify("+56 days");
                            return $startAvailableTimesOnDate;
                        } else {
                            // if date submitted + 8 weeks is still in the past, default to today's date
                            return $today;
                        }
                    }
                } else {
                    // display error on front end since future date was submitted
                    $session->set('invalid_menstrual_cycle_date', true);
                }
            }
        }

        return null;
    }

    /**
     * Returns an instance of EntryQuery that queries services according to their age restrictions
     *
     * @param int[] $serviceIds
     * @param int $age
     * @return EntryQuery
     */
    public function verifyServicesByAge(array $serviceIds, int $age)
    {
        // filter out services by age restrictions
        $ids = Entry::find()
            ->section('services')
            ->id($serviceIds)
            ->innerJoin('matrixblocks', 'matrixblocks.ownerId = elements.id')
            ->innerJoin('matrixcontent_serviceagerestrictions', 'matrixcontent_serviceagerestrictions.elementId = matrixblocks.id')
            ->andWhere([
                '<=',
                'matrixcontent_serviceagerestrictions.field_restriction_minimumAge',
                $age
            ])
            ->andWhere([
                '>=',
                'matrixcontent_serviceagerestrictions.field_restriction_maximumAge',
                $age
            ])
            ->ids();

        return count($ids) > 0 ? $ids : null;
    }


    /**
     * Returns a "suite" Entry that contains a Service block with given external department ID
     *
     * @param string $externalDepartmentID EPIC External Department ID
     * @return Entry|null
     */
    public function findSuiteEntryForGivenExternalDepartmentID(?string $externalDepartmentID): ?Entry
    {
        return Entry::find()
            ->section('locations')
            ->type('suite')
            ->innerJoin('matrixblocks', 'matrixblocks.ownerId = elements.id')
            ->innerJoin('matrixcontent_suiteservices', 'matrixcontent_suiteservices.elementId = matrixblocks.id')
            ->where([
                '=',
                'matrixcontent_suiteservices.field_service_externalDepartmentId',
                $externalDepartmentID
            ])
            ->one();
    }


    /**
     * Returns a "suite" Entry that contains a Service block with given external provider resource ID
     *
     * @param string $externalProviderResourceID EPIC External Provider Resource ID
     * @return Entry|null
     */
    public function findSuiteEntryForGivenExternalProviderResourceID(?string $externalProviderResourceID): ?Entry
    {
        return Entry::find()
            ->section('locations')
            ->type('suite')
            ->innerJoin('matrixblocks', 'matrixblocks.ownerId = elements.id')
            ->innerJoin('matrixcontent_suiteservices', 'matrixcontent_suiteservices.elementId = matrixblocks.id')
            ->where([
                '=',
                'matrixcontent_suiteservices.field_service_externalProviderResourceId',
                $externalProviderResourceID
            ])
            ->one();
    }

    /**
     * Returns true/false if a service selected is requires an additional prompt
     *
     * @param array $serviceIds
     * @param string $handle
     * @return bool
     */
    public function determineIfAdditionalPromptNeeded(array $serviceIds, string $handle)
    {
        $serviceCategoriesIds = [];

        $serviceCategories = Category::find()
            ->group($handle)
            ->all();

        // look up all ids for services
        foreach ($serviceCategories as $category) {
            if (isset($category->assignedService) && ($category->assignedService->one() != null)) {
                \array_push($serviceCategoriesIds, $category->assignedService->one()->id);
            }
        }

        if ($serviceCategoriesIds) {
            // iterate through services selected, if one is present, return true
            foreach ($serviceCategoriesIds as $serviceId) {
                if (\in_array($serviceId, $serviceIds)) {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * Returns a user's age in years
     *
     * Returns an integer declaring user's age in years
     * @param DateTime $dob
     * @return int
     */
    public function determineAgeInYears(DateTime $dob)
    {
        $today = new DateTime();
        $dateDifference = $today->diff($dob);
        $age = $dateDifference->y;

        if ($age == 0) {
            // 0 - 12 months olds are technically 0 years old, which does not meet physician requirements of a 0 minAge
            return 1;
        }

        return $age;
    }

    /**
     * Returns an array of eligible physician IDs based on age and service age requirements.
     *
     * @param PhysicianSearchForm $form Form that validates and contains supported query parameters
     * @param string $serviceID Id representing current service
     * @param string $age current age of registrant
     * @param string $reasonForVisitId
     * @param string $newPatientVisit
     * @return array
     */
    public function queryEligiblePhysicians(PhysicianSearchForm $form, array $serviceId, $age, string $reasonForVisitId = null, bool $newPatientVisit, string $locationId = null, bool $userHasMedicareAdvantageInsurancePlan): array
    {
        $query = Entry::find()
            ->section('physicians')
            ->innerJoin('matrixblocks', 'matrixblocks.ownerId = elements.id')
            ->innerJoin('matrixcontent_agerestrictions', 'matrixcontent_agerestrictions.elementId = matrixblocks.id');
    
        if ($age) {
            $query = $query->where([
                '<=',
                'matrixcontent_agerestrictions.field_restriction_minimumAge',
                $age
            ])
            ->andWhere([
                '>=',
                'matrixcontent_agerestrictions.field_restriction_maximumAge',
                $age
            ]);
        }

        $query = $query->andWhere([
            '=',
            'field_allowsOnlineScheduling',
            true
        ]);

        if ($newPatientVisit) {
            $query->andWhere([
                '=',
                'field_acceptsNewPatients',
                true
            ]);
        }

        if (!$userHasMedicareAdvantageInsurancePlan) {
            $query->andWhere(
                [
                    'or',
                    [
                        'field_onlyAcceptsMedicareAdvantage' => null
                    ],
                    [
                        'field_onlyAcceptsMedicareAdvantage' => 0
                    ]
                ]
            );
        }

        if ($form->search_physician_attribute) {
            // physicians can have alternative names
            // i.e Jacob Smith can be "Jake"
            $query->andWhere(['or', ['like', 'title', $form->search_physician_attribute], ['like', 'field_alternativeSearchName', $form->search_physician_attribute]]);
        }

        $servicesQuery = Entry::find()
            ->section('services')
            ->id($serviceId);

        $relatedTo = [
            'and',
            ['targetElement' => $servicesQuery]
        ];

        //filter out physicians if reason is selected during scheduling process
        if ($reasonForVisitId) {
            $reason = Entry::find()
                ->section('serviceReasonsForVisit')
                ->id($reasonForVisitId);

            \array_push(
                $relatedTo,
                [
                    'sourceElement' => $reason
                ]
            );
        }

        //filter out physicians if a location is already assigned
        if ($locationId) {
            // locationId is always the parent;
            // need to find suites assigned
            $location = Entry::find()
                ->section('locations')
                ->descendantOf($locationId);

            \array_push(
                $relatedTo,
                [
                    'field' => 'physicianLocations',
                    'targetElement' => $location
                ]
            );
        }

        $query->relatedTo($relatedTo);

        return $query->ids();
    }

    /**
     * Returns an instance of EntryQuery that queries physicians with provided form parameters.
     *
     * @param PhysicianSearchForm $form Form that validates and contains supported query parameters
     * @param array $eligiblePhysicianIds represents IDs of physicians filtered out by age requirements
     * @return EntryQuery
     */
    public function queryPhysicians(PhysicianSearchForm $form, array $eligiblePhysicianIds): EntryQuery
    {
        $query = Entry::find()
            ->section('physicians')
            ->id($eligiblePhysicianIds);

        $relatedTo = ['and'];

        PhysiciansModule::getInstance()->physiciansModuleService->relatedToAgeSeenFilter($form, $query);
        PhysiciansModule::getInstance()->physiciansModuleService->relatedToAgeSeenRange($form, $query);
        PhysiciansModule::getInstance()->physiciansModuleService->relatedToLanguages($form, $relatedTo);
        PhysiciansModule::getInstance()->physiciansModuleService->relatedToGenders($form, $query);
        PhysiciansModule::getInstance()->physiciansModuleService->relatedToLgbtqiaResource($form, $query);
        PhysiciansModule::getInstance()->physiciansModuleService->relatedToHospitals($form, $relatedTo);

        if ($form->city) {
            PhysiciansModule::getInstance()->physiciansModuleService->relatedToCities($form, $relatedTo);
        }

        if (($form->address || ($form->latitude && $form->longitude)) && $form->order_by == 'proximity') {
            PhysiciansModule::getInstance()->physiciansModuleService->orderPhysiciansByDistance($form, $query);
        }

        if (count($relatedTo) > 1) {
            $query->relatedTo($relatedTo);
        }

        if ($form->search_physician_attribute) {
            $query->search("*" . $form->search_physician_attribute . "*");
        }

        if ($form->order_by == 'availability') {
            $query->orderBY(new Expression(
                'field_physicianNextAvailableAppointmentTime IS NULL ASC, field_physicianNextAvailableAppointmentTime ASC'
            ));
        }

        if ($form->rating) {
            PhysiciansModule::getInstance()->physiciansModuleService->orderPhysiciansByRating($query, $form->rating);
        }

        if ($form->availability) {
            if ($form->availability == 'Today') {
                $startDate = new DateTime();
                $tomorrow = $startDate->modify('+1 days')->format('Y-m-d');
                $query->andWhere([
                    '<=',
                    'field_physicianNextAvailableAppointmentTime',
                    $tomorrow
                ]);
            };

            if ($form->availability == 'Next 3 Days') {
                $startDate = new DateTime();
                $threeDays = $startDate->modify('+3 days');
                $query->andWhere([
                    '<=',
                    'field_physicianNextAvailableAppointmentTime',
                    $threeDays->format('Y-m-d')
                ]);
            }
        }

        return $query;
    }

    /**
     * Determines the name of the doctor performing specified service.
     * If service belongs to a group, it looks up the grouping name from the Scheduling Landing Page
     *
     * @param int[] $serviceIds
     * @return ?string the name of the service doctor
     */
    public function getServiceDoctorName(array $serviceIds = [], $onlyServiceName = false): ?string
    {
        if (count($serviceIds) == 0) {
            return null;
        } elseif (count($serviceIds) == 1) {
            $service = Entry::find()->id($serviceIds[0])->one();

            if ($onlyServiceName) {
                return $service->title ?? null;
            } else {
                return $service->serviceDoctorType ?? null;
            }
        } else {
            $schedulingLandingPage = Entry::find()->section('schedulingLandingPage');

            // find a featured service containing given service IDs
            foreach ($schedulingLandingPage->one()->featuredService->all() as $featuredService) {
                if (!\array_diff($serviceIds, $featuredService->featuredService->ids())) {
                    if ($onlyServiceName) {
                        return $featuredService->serviceTitle;
                    } else {
                        return "{$featuredService->serviceTitle} Physician";
                    }
                }
            }

            // if not found within the featured services, find a service tile containing given service IDs
            foreach ($schedulingLandingPage->one()->serviceTile->all() as $serviceTile) {
                if (!\array_diff($serviceIds, $serviceTile->service->ids())) {
                    if ($onlyServiceName) {
                        return $serviceTile->serviceTitle;
                    } else {
                        return "{$serviceTile->serviceTitle} Physician";
                    }
                }
            }

            // if not found, give up
            return null;
        }
    }

    /**
     * Determines valid EPIC External Visit Type IDs for given use scheduling session flow.
     * This function assumes that the PatientUser object contains the answer to required questions
     * such as "Are you a new patient?" or "Is this an annual physician appointment?"
     *
     * @param string $selectedServiceId
     * @return [] valid visit type IDs for this user scheduling flow session
     */
    public function getUsersValidVisitTypeIDs(string $selectedServiceId, bool $videoOnly = false)
    {
        $user = Craft::$app->patient_user->identity;

        if ($user === null) {
            return [];
        }

        // visit type code
        $visitTypeCodes = SchedulingModule::getInstance()->schedulingModuleService->getSchedulingVisitTypeCodesForService($selectedServiceId);

        $chosenVisitTypeCode = null;
        // set option to false
        $showRecommendedProviders = false;
        //new patients
        if ($user->appointment_new_patient_visit == "1") {
            $chosenVisitTypeCode = $user->appointment_follow_up_visit == "1" ? $visitTypeCodes['followUpVisitNewPatient'] : $visitTypeCodes['newPatient'];
        } else {
            $chosenVisitTypeCode = $user->appointment_follow_up_visit == "1" ? $visitTypeCodes['followUpVisit'] : $visitTypeCodes['establishedPatient'];
        }

        if ($user->appointment_vein_clinic_visit == "1") {
            $chosenVisitTypeCode = $visitTypeCodes['veinClinic'];
        }

        if ($user->appointment_full_body_skin_exam_visit == "1") {
            $chosenVisitTypeCode = $visitTypeCodes['fullBodySkinExam'];
        }

        $codes = [
            'main' => $chosenVisitTypeCode,
            'video' => self::$VIDEO_VISIT_EXTERNAL_VISIT_TYPE_ID
        ];

        if ($videoOnly == true) {
            unset($codes['main']);
        }

        return $codes;
    }
}
