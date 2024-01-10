<?php

/**
 * Scheduling module for Craft CMS 3.x
 *
 * Allows for extended management of the scheduling section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\schedulingmodule\controllers;

use Craft;
use craft\elements\Category;
use craft\elements\db\EntryQuery;
use craft\elements\Entry;
use craft\elements\MatrixBlock;
use craft\web\Controller;
use craft\web\Response;
use DateInterval;
use DatePeriod;
use DateTime;
use modules\apimodule\forms\SchedulingTokenForm;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\fileextensions\ICS;
use modules\DupageCoreModule\forms\LoginForm;
use modules\DupageCoreModule\forms\SiteWideSearchForm;
use modules\DupageCoreModule\models\PatientUser;
use modules\DupageCoreModule\queue\EmailJob;
use modules\locationsmodule\forms\LocationSearchForm;
use modules\physiciansmodule\forms\PhysicianSearchForm;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\forms\CalendarForm;
use modules\schedulingmodule\forms\RecommendedPhysicians;
use modules\schedulingmodule\forms\GetAppointmentTimesForm;
use modules\schedulingmodule\forms\PatientInformationForm;
use modules\schedulingmodule\forms\SchedulingServiceSearchForm;
use modules\schedulingmodule\forms\ServiceSelection;
use modules\schedulingmodule\forms\ShareEmailForm;
use modules\schedulingmodule\SchedulingModule;
use modules\schedulingmodule\services\SchedulingModuleService;
use samdark\log\PsrMessage;
use yii\data\ActiveDataProvider;
use yii\data\ArrayDataProvider;
use yii\db\Expression;
use yii\filters\AccessControl;
use yii\filters\Cors;
use yii\filters\RateLimiter;
use yii\filters\VerbFilter;
use yii\web\HttpException;
use yii\web\UrlManager;
use yii\widgets\LinkPager;
use modules\schedulingmodule\services\MicrositesAuthenticationService;

/**
 * @author    Wipfli Digital
 * @package   SchedulingModule
 * @since     1.0.0
 */
class DefaultController extends Controller
{
    /**
     * @var    bool|array Allows anonymous access to this controller's actions.
     *         The actions must be in 'kebab-case'
     * @access protected
     */
    protected $allowAnonymous = self::ALLOW_ANONYMOUS_LIVE;

    public function behaviors()
    {
        $behaviors = parent::behaviors();

        $behaviors['access'] = [
            'class' => AccessControl::class,
            'rules' => [
                [
                    'allow' => true,
                    'actions' => ['default', 'get-services', 'auto-suggestions', 'schedule-deeplink'],
                    'roles' => ['?', '@'],
                ],
                [
                    'allow' => true,
                    'actions' => ['scheduling-from-physician-page', 'set-current-patient', 'insurance', 'preliminary', 'visit-reason', 'select-physician', 'physician-auto-suggestions', 'select-appointment', 'select-location', 'get-appointment-times', 'book', 'share', 'share-email', 'cancel-appointment', 'calendar'],
                    'roles' => ['@'],
                ],
                [
                    'allow' => true,
                    'actions' => ['get-locations'],
                    'roles' => ['?'],
                ],
            ],
            'user' => 'patient_user',
            'denyCallback' => function ($rule, $action) {
                return $this->redirect('schedule');
            }
        ];

        $behaviors['corsFilter'] = [
            'class' => Cors::class,
            'cors' => [
                'Origin' => array_merge([getenv('DEFAULT_SITE_URL')], explode(',',getenv('MICROSITE_URLS'))),
                'Access-Control-Request-Method' => ['GET', 'POST'],
                'Access-Control-Request-Headers' => ['*'],
            ]
        ];

        $behaviors['verbs'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'default'  => ['GET', 'POST'],
                'insurance'  => ['GET', 'POST'],
                'preliminary'  => ['GET', 'POST'],
                'visit-reason'  => ['GET', 'POST'],
                'get-services'  => ['GET', 'POST'],
                'auto-suggestions'  => ['GET', 'POST'],
                'select-physician'  => ['GET', 'POST'],
                'physician-auto-suggestions'  => ['GET', 'POST'],
                'select-appointment'  => ['GET', 'POST'],
                'book'  => ['GET', 'POST'],
                'share'  => ['GET', 'POST'],
                'get-appointment-times'  => ['POST'],
                'share-email' => ['GET'],
                'calendar' => ['GET'],
                'cancel-appointment'  => ['POST'],
                'scheduling-from-physician-page'  => ['POST'],
                'set-current-patient'  => ['POST'],
                'schedule-deeplink' => ['GET'],
                'get-locations' => ['GET'],

            ],
        ];

        $behaviors['rateLimiter'] = [
            'class' => RateLimiter::class,
            'user' => Craft::$app->patient_user->identity,
        ];

        return $behaviors;
    }

    /**
     * handles redirecting users to a specific URL from the CMS during epic outages
     */
    public function beforeAction($action)
    {
        $schedulingLoginRedirect = Craft::$app->globals->getSetByHandle('generalSiteConfig')['schedulingLoginRedirectUrl'];
        if (!empty($schedulingLoginRedirect)) {
            return $this->redirect($schedulingLoginRedirect, 302);
        }
        return parent::beforeAction($action);
    }

    /**
     * @return mixed
     */
    public function actionSchedulingFromPhysicianPage()
    {
        $user = Craft::$app->patient_user->identity;

        // If a user chooses an appointment time from a physician details page,
        // they are ffectively starting a new scheduling flow,
        // thus we should reset.
        $user->resetUserData();

        $user->physician_selected_outside_of_scheduling = "1";
        $user->apointment_time_selected_outside_of_scheduling = "1";

        $user->load([
            'PatientUser' => Craft::$app->request->post()
        ]);

        Craft::$app->response->format = \yii\web\Response::FORMAT_JSON;
        if ($user->save()) {
            Craft::$app->patient_user->setReturnUrl(getenv('DEFAULT_SITE_URL') . '/schedule/insurance');
            return [
                'success' => true,
                'error' => null
            ];
        } else {
            return [
                'success' => false,
                'error' => Craft::t(
                    'scheduling-module',
                    'Something went wrong. Please try again.'
                )
            ];
        }
    }

    /**
     * @return mixed
     */
    public function actionSetCurrentPatient()
    {
        $user = Craft::$app->patient_user->identity;
        $user->load([
            'PatientUser' => Craft::$app->request->post()
        ]);

        if ($user->save()) {
            if (isset($user->appointment_current_epi)) {
                return $this->redirect('/schedule/insurance');
            }
        }

        return $this->redirect('/schedule');
    }

    /**
     * @return mixed
     */
    public function actionDefault()
    {
        $user = Craft::$app->patient_user->identity;
        if (!$user) {
            $user = PatientUser::anonymousUser();
            $result = Craft::$app->patient_user->login($user, (60 * 20));
        }

        $error = null;
        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            if ($user->save()) {
                // analytics
                $service = SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($user->getAppointmentServiceIds(), true);
                Craft::$app->session->setFlash('appointment-service-chosen', $service);

                if (!isset(Craft::$app->request->post()['appointment_current_epi'])) {
                    // The user picked a service, but they may want to authenticate with mychart, or continue as guest.
                    // In either case, ther next step will be the insurance page
                    Craft::$app->patient_user->setReturnUrl(getenv('DEFAULT_SITE_URL') . '/schedule/insurance');
                    if (Craft::$app->patient_user->identity->anonymous) {
                        // if anonymous, always go to the login page
                        return $this->redirect('login-portal?return_uri=' . getenv('DEFAULT_SITE_URL') . '/schedule/insurance');
                    } else {
                        // if authenticated with mychart, skip the login page
                        return $this->redirect('schedule/insurance');
                    }
                }
            } else {
                foreach ($user->errors as $singleError) {
                    $error .= \implode(" ", $singleError) . " ";
                }
            }
        } else {
            $user->resetUserData();
        }

        // If the user clicks out to the login page without picking a service,
        // we want to return them back to this page after successful authentication
        Craft::$app->patient_user->setReturnUrl(getenv('DEFAULT_SITE_URL') . '/schedule');

        return $this->renderTemplate('schedule.twig', [
            'error' => $error
        ]);
    }

    /**
     * @return mixed
     */
    public function actionInsurance()
    {
        $user = Craft::$app->patient_user->identity;

        // this is needed to store sensitive HIPPA information
        $session = Craft::$app->session;
        $dob = $user->getDob();
        $serviceIds = $user->getAppointmentServiceIds();
        $error = null;
        $showAgeServiceRestrictionError = false;
        $showAgePhysicianRestrictionError = false;
        $shouldAskWhenLastMenstrualCycle = false;
        $showHospitalFollowUpVisit = false;
        $showCongratsModal = false;
        $futureDateSubmitted = false;
        $shouldAskIfPregnant = SchedulingModule::getInstance()->schedulingModuleService->determineIfAdditionalPromptNeeded($serviceIds, 'obstetricsGynecologyServices');
        $serviceDoctorName = SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($serviceIds ?? []);
        $visitTypes = [];
        if (!empty($serviceIds)) {
            $visitTypes = SchedulingModule::getInstance()->schedulingModuleService->getSchedulingVisitTypeCodesForService($serviceIds[0]);
        }

        if (count($serviceIds) == 0) {
            return $this->redirect('schedule');
        }

        if (Craft::$app->request->isPost) {
            $request = Craft::$app->request->post();

            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            $dob = $user->getDob();
            $physicianId = $user->appointment_physician_id;

            if ($dob && $dob instanceof DateTime && $physicianId) {
                $age = SchedulingModule::getInstance()->schedulingModuleService->determineAgeInYears($dob);
                $selectedPhysician = Entry::find()->section('physicians')->id($physicianId)->one();

                if ($selectedPhysician != null) {
                    $minAge = $selectedPhysician->ageRestrictions->one()->minimumAge;
                    $maxAge = $selectedPhysician->ageRestrictions->one()->maximumAge;
                    if ($age < $minAge || $age > $maxAge) {
                        // allows to redirect to select-appointment view
                        $user->physician_selected_outside_of_scheduling = '0';
                        $user->apointment_time_selected_outside_of_scheduling = '0';
                        $user->appointment_time = null;
                        $user->appointment_physician_id = null;
                        $user->setAppointmentServiceIds($user->getAppointmentServiceIds());
                        $showAgePhysicianRestrictionError = true;
                    }
                    if ($selectedPhysician->onlyAcceptsMedicareAdvantage && $user->appointment_insurance_plan_medicare_advantage === '0') {
                        // allows user to select a new physician if previously selected physician does not accept user's insurance plan
                        $user->physician_selected_outside_of_scheduling = '0';
                        $user->apointment_time_selected_outside_of_scheduling = '0';
                        $user->appointment_time = null;
                        $user->appointment_physician_id = null;
                        $user->setAppointmentServiceIds($user->getAppointmentServiceIds());
                        $user->save();
                        return $this->redirect('schedule/select-physician');
                    }
                }
            }

            if (isset($request['patient_is_pregnant'])) {
                $session->set('patient_is_pregnant', $request['patient_is_pregnant']);
                $shouldAskWhenLastMenstrualCycle = true;
            }

            if (isset($request['last_menstrual_cycle_day']) && isset($request['last_menstrual_cycle_month']) && isset($request['last_menstrual_cycle_year'])) {
                $session->set('last_menstrual_cycle_day', $request['last_menstrual_cycle_day']);
                $session->set('last_menstrual_cycle_month', $request['last_menstrual_cycle_month']);
                $session->set('last_menstrual_cycle_year', $request['last_menstrual_cycle_year']);

                $startDate = SchedulingModule::getInstance()->schedulingModuleService->determineStartDate();

                // already asked so this changes to false
                $shouldAskWhenLastMenstrualCycle = false;
                $shouldAskIfPregnant = false;

                // null date means patient is too far along in their pregnancy, show congrats modal
                if ($startDate == null) {
                    // but if user enters a date in the future, show different message
                    if ($session->get('invalid_menstrual_cycle_date') != null) {
                        $futureDateSubmitted = true;
                    } else {
                        $showCongratsModal = true;
                    }
                }
            }

            if ($user->save()) {
                if ($user->appointment_hospital_follow_up_visit) {
                    $showHospitalFollowUpVisit = false;
                }

                // proceed to /preliminary only after user fully declares the visit type
                if (
                    !isset($request['appointment_new_patient_visit'])
                    && !isset($request['appointment_follow_up_visit'])
                    && !isset($request['appointment_hospital_follow_up_visit'])
                    && !isset($request['appointmentServiceIds'])
                    && !isset($request['last_menstrual_cycle_month'])
                    && !isset($request['last_menstrual_cycle_day'])
                    && !isset($request['last_menstrual_cycle_year'])
                    && !isset($request['patient_is_pregnant'])
                ) {
                    if ($dob && $dob instanceof DateTime) {
                        $age = SchedulingModule::getInstance()->schedulingModuleService->determineAgeInYears($dob);
                        $validServices = SchedulingModule::getInstance()->schedulingModuleService->verifyServicesByAge($serviceIds, $age);

                        if ($validServices != null && $showAgePhysicianRestrictionError == false) {
                            return $this->redirect('schedule/preliminary');
                        } else {
                            $showAgeServiceRestrictionError = true;
                        }
                    }
                }
                // refresh after a POST
                $serviceIds = $user->getAppointmentServiceIds();
            } else {
                foreach ($user->errors as $singleError) {
                    $error .= \implode(" ", $singleError) . " ";
                }
            }
        }

        // If loged-in user is scheduling an appointment for a patient who already had an appointment for this service at DMG,
        // we should NOT prompt them with the "Have you seen a DMG {serviceDoctorName} before?" modal.
        $shouldAskIfNewPatient = true;
        if (count($user->getAppointmentServiceIds()) > 1 && !$serviceDoctorName) {
            // If scheduling for a physician who offers 2+ schedulable services,
            // and those services do not belong to one group (e.g. Family Care),
            // we need to ask the user to pick a service first.
            $shouldAskIfNewPatient = false;
        } else {
            // Figure out if patient has seen a doctor for selected service
            $shouldAskIfNewPatient = SchedulingModule::getInstance()->schedulingModuleService->shouldAskIfNewPatient();
        }

        // do not overlap error modals
        $shouldAskIfNewPatient = $shouldAskIfNewPatient && !$showAgeServiceRestrictionError && !$showAgePhysicianRestrictionError;

        if ($user->appointment_hospital_follow_up_visit == null && $user->physician_selected_outside_of_scheduling != "1") {
            $isPrimaryCareService = SchedulingModule::getInstance()->schedulingModuleService->determineIfAdditionalPromptNeeded($serviceIds, 'primaryCareServices');

            if ($isPrimaryCareService && $user->appointment_new_patient_visit == '0') {
                $showHospitalFollowUpVisit = true;
            }
        }

        // User can trigger a scheduling flow from a service, location or a physician page.
        // If they do so while authenticated, they will be asked to choose a patient on that origin page.
        // If they do so while unauthenticated, they will proceed to the login before before coming here (insurance page).
        // For the second case, instead of taking them back to the origin page and asking for a patient on that page,
        // we simply show the patient picker on the insurance page.
        $showPatientPicker = Craft::$app->session->getFlash('show-patient-picker');
        if ($showPatientPicker) {
            // If showing the patient picker, do not display the DOB, which by default will be that of the primary account holder.
            $dob = null;
        }

        return $this->renderTemplate('_scheduling/insurance/_main.twig', [
            'error' => $error,
            'ageServiceRestrictionError' => $showAgeServiceRestrictionError,
            'agePhysicianRestrictionError' => $showAgePhysicianRestrictionError,
            'visitTypes' => $visitTypes,
            'dob' => $dob,
            'serviceDoctorName' => $serviceDoctorName,
            'serviceIds' => $serviceIds,
            'shouldAskIfNewPatient' => $shouldAskIfNewPatient,
            'shouldAskIfPregnant' => $shouldAskIfPregnant,
            'shouldAskWhenLastMenstrualCycle' => $shouldAskWhenLastMenstrualCycle,
            'showCongratsModal' => $showCongratsModal,
            'showHospitalFollowUpVisit' => $showHospitalFollowUpVisit,
            'futureDateSubmitted' => $futureDateSubmitted
        ]);
    }

    /**
     * @return mixed
     */
    public function actionPreliminary()
    {
        $user = Craft::$app->patient_user->identity;

        if (count($user->getAppointmentServiceIds()) == 0) {
            return $this->redirect('schedule');
        }

        $serviceId = $user->getAppointmentServiceIds()[0];

        $serviceEntry = Entry::find()->section('services')->id($serviceId)->one();
        $appointmentSchedulingRuleOutQuestions = $serviceEntry->appointmentSchedulingRuleOutQuestions->all();

        if (count($appointmentSchedulingRuleOutQuestions) == 0) {
            return $this->redirect('schedule/visit-reason');
        }

        $currentQuestionIndex = Craft::$app->request->getQueryParam('currentQuestionIndex', 0);
        $currentQuestion = $appointmentSchedulingRuleOutQuestions[$currentQuestionIndex];

        $error = null;
        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            if ($user->save()) {
                if ($currentQuestionIndex + 1 == count($appointmentSchedulingRuleOutQuestions)) {
                    return $this->redirect('schedule/visit-reason');
                } else {
                    return $this->redirect('schedule/preliminary?currentQuestionIndex=' . ($currentQuestionIndex + 1));
                }
            } else {
                foreach ($user->errors as $singleError) {
                    $error .= \implode(" ", $singleError) . " ";
                }
            }
        }

        return $this->renderTemplate('_scheduling/_rule-out-questions.twig', [
            'serviceDoctorType' => SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($user->getAppointmentServiceIds()),
            'currentQuestion' => $currentQuestion,
            'currentQuestionIndex' => $currentQuestionIndex,
            'error' => $error
        ]);
    }

    /**
     * @return mixed
     */
    public function actionVisitReason()
    {
        $user = Craft::$app->patient_user->identity;
        $serviceId = $user->getAppointmentServiceIds();

        if (count($serviceId) == 0) {
            return $this->redirect('schedule');
        }
        $serviceId = $serviceId[0];

        $showVisitError = false;

        $serviceEntry = Entry::find()->section('services')->id($serviceId)->one();
        $reasonsForVisit = $serviceEntry->appointmentSchedulingReasonsForVisit->orderBy(['title' => SORT_ASC])->all();
        $serviceQuestions = $serviceEntry->appointmentSchedulingRuleOutQuestions->all();
        $serviceQuestionsCount = 0;

        if (count($serviceQuestions) > 0) {
            $serviceQuestionsCount = count($serviceQuestions);
        }

        if (count($reasonsForVisit) == 0) {
            return $this->redirect('schedule/select-physician');
        }

        $error = null;
        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            $reasonId = $user->appointment_reason_for_visit_id;
            if (isset($user->physician_selected_outside_of_scheduling)) {
                if ($user->physician_selected_outside_of_scheduling == '1') {
                    $physicianId = $user->appointment_physician_id;
                    $showVisitError = SchedulingModule::getInstance()
                        ->schedulingModuleService
                        ->showReasonErrorModalForPhysician(
                            $physicianId,
                            $reasonId
                        );

                    if ($showVisitError) {
                        // allows to redirect to select-physician view
                        $user->physician_selected_outside_of_scheduling = '0';
                        $user->apointment_time_selected_outside_of_scheduling = '0';
                        $user->appointment_time = null;
                        $user->appointment_physician_id = null;
                        $user->appointment_new_patient_visit = "1";
                        $user->setAppointmentServiceIds($user->getAppointmentServiceIds());
                    }
                }
            }

            // if scheduling flow was started from a specific location,
            // but patient selected a reason for visit that isn't trated at this location
            // we need to redirect the user to the "select location" page showing valid locations
            if ($user->isSchedulingFlowWithoutPhysicians() && $user->location_id) {
                $showVisitError = SchedulingModule::getInstance()
                    ->schedulingModuleService
                    ->showReasonErrorModalForLocation(
                        $user->location_id,
                        $reasonId
                    );

                if ($showVisitError) {
                    $user->location_id = null;
                    $user->location_selected_outside_of_scheduling = null;
                }
            }

            if ($user->save()) {
                if ($showVisitError == false) {
                    if ($user->isSchedulingFlowWithoutPhysicians()) {
                        return $this->redirect('schedule/select-location');
                    } else {
                        return $this->redirect('schedule/select-physician');
                    }
                }
            } else {
                foreach ($user->errors as $singleError) {
                    $error .= \implode(" ", $singleError) . " ";
                }
            }
        }

        return $this->renderTemplate('_scheduling/_reasons-for-visit.twig', [
            'serviceDoctorType' => SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($user->getAppointmentServiceIds()),
            'reasonsForVisit' => $reasonsForVisit,
            'serviceQuestionsCount' => $serviceQuestionsCount,
            'serviceEntry' => $serviceEntry,
            'showVisitError' => $showVisitError,
            'error' => $error
        ]);
    }

    /**
     * @return mixed
     */
    public function actionGetServices()
    {
        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        if ($isAjax) {
            $form = new SchedulingServiceSearchForm();
            $form->load([
                'SchedulingServiceSearchForm' => Craft::$app->request->get()
            ]);
            $results = [];

            if ($form->validate()) {
                $results = SchedulingModule::getInstance()->schedulingModuleService->getMatchingEntries(
                    $form->query
                );
            }

            return $this->renderTemplate('_scheduling/services/_results', [
                'results' => $results
            ]);
        } else {
            $error = null;
            if (Craft::$app->request->isPost) {
                $user = Craft::$app->patient_user->identity;

                // if not logged in, log in anonymous user
                if (!$user) {
                    $user = PatientUser::anonymousUser();
                    Craft::$app->patient_user->login($user);
                }

                $user->resetUserData();

                $user->load([
                    'PatientUser' => Craft::$app->request->post()
                ]);

                if ($user->save()) {
                    // a. anonymous users will be given a choice to log in or continue as a guest
                    // b. logged-in mychart users will be shown a modal where they need to indicate which patient they are scheduling for
                    //      This is needed because this specific route does not show eligible patients (only /schedule does)
                    Craft::$app->patient_user->setReturnUrl(getenv('DEFAULT_SITE_URL') . '/schedule/insurance');
                    return $this->redirect('login-portal');
                }
            }

            $results = SchedulingModule::getInstance()->schedulingModuleService->getServices();
            return $this->renderTemplate('/_scheduling/services/_main', [
                'results' => $results
            ]);
        }
    }

    /**
     * @return mixed
     */
    public function actionAutoSuggestions()
    {
        $form = new SchedulingServiceSearchForm();
        $form->load([
            'SchedulingServiceSearchForm' => Craft::$app->request->get()
        ]);

        $entries = [];
        $entries = DupageCoreModule::getInstance()->dupageCoreModuleService->getAutoSuggestions(
            $form->query,
            ['services', 'procedures', 'conditions']
        );

        $this->asJson($entries);
    }

    /**
     * @return mixed
     */
    public function actionSelectPhysician()
    {
        $user = Craft::$app->patient_user->identity;

        if ($user->isSchedulingFlowWithoutPhysicians()) {
            if ($user->location_id) {
                $user->appointment_physician_id = $user->findExternalProviderResourceIdForChosenServiceEntry();

                if ($user->save()) {
                    return $this->redirect('schedule/select-appointment');
                }
            } else {
                // select a location first
                return $this->redirect('schedule/select-location');
            }
        }

        // if doctor chosen from authenticated page, or the physician detail page, skip this step
        if (isset($user->physician_selected_outside_of_scheduling)) {
            if ($user->physician_selected_outside_of_scheduling == '1' && $user->appointment_physician_id) {
                return $this->redirect('schedule/select-appointment');
            }
        }

        // if a category like "Primary Care Services" is selected, there are multiple service IDs assigned
        // these IDs need to be preserved because these IDs eventually get lost when progressing through the scheduling flow
        // i.e if a user selects a physician, then hits the back button, the service selected is now just for that physcian and not the original category service IDs
        if ($user->category_service_ids == null) {
            $serviceId = $user->getAppointmentServiceIds();
        } else {
            $serviceId = $user->category_service_ids;
        }

        if (count($serviceId) > 1) {
            $user->category_service_ids = $serviceId;
        }

        $reasonForVisitId = $user->appointment_reason_for_visit_id;
        $serviceEntry = Entry::find()->section('services')->id($serviceId)->one();

        if (!$serviceEntry) {
            return $this->redirect('schedule');
        }

        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            if (!isset(Craft::$app->request->post()['appointment_selected_recommended_physician_id'])) {
                $user->appointment_selected_recommended_physician_id = null;
            }

            if ($user->save()) {
                return $this->redirect('schedule/select-appointment');
            }
        }

        $reasonsForVisit = $serviceEntry->appointmentSchedulingRuleOutQuestions->all();

        // needed to determine back button flow on view
        $serviceQuestionsCount = 0;
        $serviceQuestions = count($reasonsForVisit);

        if ($serviceQuestions > 0) {
            $serviceQuestionsCount = $serviceQuestions;
        }

        $dob = $user->getDob();
        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        if ($dob && $dob instanceof DateTime) {
            $age = SchedulingModule::getInstance()->schedulingModuleService->determineAgeInYears($dob);
        } else {
            Craft::error(new PsrMessage('Invalid date of birth supplied', [
                'appointment_visit_type_id' => $user->appointment_visit_type_id
            ]), 'dupagecoremodules/models/PatientUser::getDob');

            throw new HttpException(400, Craft::t('scheduling-module', 'There is an issue with the users date of birth.'));
        }

        $results = [];
        $eligiblePhysicianIds = [];
        $form = new PhysicianSearchForm();

        $form->load([
            'PhysicianSearchForm' => Craft::$app->request->get()
        ]);

        if ($age) {
            // first query filters out physicians that do not meet age requirements and services that do not meet age requirements
            $eligiblePhysicianIds = SchedulingModule::getInstance()->schedulingModuleService->queryEligiblePhysicians($form, $serviceId, $age, $reasonForVisitId, $user->appointment_new_patient_visit, $user->location_id, $user->appointment_insurance_plan_medicare_advantage === '1');
        }

        $physicians = SchedulingModule::getInstance()->schedulingModuleService->queryPhysicians($form, $eligiblePhysicianIds);
        $totalCount = $physicians->count();

        // get physican IDs without applying FE filters
        $user->eligible_physicians =  SchedulingModule::getInstance()->schedulingModuleService->queryEligiblePhysicians(new PhysicianSearchForm(), $serviceId, $age, $reasonForVisitId, $user->appointment_new_patient_visit, $user->location_id, $user->appointment_insurance_plan_medicare_advantage === '1');
        $user->save();

        $dataProvider = new ActiveDataProvider([
            'query' => $physicians,
            'pagination' => [
                'pageSize' => 10,
                'totalCount' => $totalCount,
                'urlManager' => new UrlManager([
                    'enablePrettyUrl' => true,
                    'enableStrictParsing' => true,
                    'showScriptName' => false,
                    'rules' => [
                        '/schedule/select-physician' => 'scheduling-module/default/select-physician',
                    ]
                ])
            ]
        ]);

        $results = $dataProvider->getModels();

        if ($isAjax) {
            return $this->renderTemplate('/_scheduling/physicians/_tiles', [
                'physicians' => $results,
                'serviceId' => $serviceId,
                'pagination' => $dataProvider->pagination,
                'totalCount' => $totalCount,
                'reasonForVisitId' => $reasonForVisitId,
                'serviceQuestionsCount' => $serviceQuestionsCount
            ]);
        } else {
            return $this->renderTemplate('/_scheduling/physicians/_main', [
                'physicians' => $results,
                'serviceId' => $serviceId,
                'serviceDoctorName' => SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($user->getAppointmentServiceIds(), true),
                'pagination' => $dataProvider->pagination,
                'totalCount' => $totalCount,
                'reasonForVisitId' => $reasonForVisitId,
                'serviceQuestionsCount' => $serviceQuestionsCount
            ]);
        }
    }

    /**
     * @return mixed
     */
    public function actionSelectLocation()
    {
        $user = Craft::$app->patient_user->identity;

        $reasonForVisitId = $user->appointment_reason_for_visit_id;
        $serviceId = $user->getAppointmentServiceIds();
        $serviceEntry = Entry::find()->section('services')->id($serviceId)->one();

        if (!$serviceEntry) {
            return $this->redirect('schedule');
        }

        if ($user->location_selected_outside_of_scheduling && $user->location_id) {
            $user->appointment_physician_id = $user->findExternalProviderResourceIdForChosenServiceEntry();
            $user->save();
            return $this->redirect('schedule/select-appointment');
        }

        // needed to determine back button flow on view
        $serviceQuestionsCount = count($serviceEntry->appointmentSchedulingRuleOutQuestions->all());

        $form = new LocationSearchForm();
        $form->load([
            'LocationSearchForm' => Craft::$app->request->get()
        ]);

        $form->reasonForVisitId = $reasonForVisitId;

        // pre-select chosen service
        $form->search_service_id = $serviceEntry->id;

        // data provider
        $dataProvider = $form->getDataProvider("scheduling-module/default/select-location");

        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);
        $locations = $dataProvider->getModels();

        $longitude = (float)$form->longitude;
        $latitude = (float)$form->latitude;

        $visitTypeCodes = SchedulingModule::getInstance()->schedulingModuleService->getSchedulingVisitTypeCodesForService($serviceId[0]);
        $chosenVisitTypeCode = null;
        if ($user->appointment_new_patient_visit == "1") {
            $chosenVisitTypeCode = $user->appointment_follow_up_visit == "1" ? $visitTypeCodes['followUpVisitNewPatient'] : $visitTypeCodes['newPatient'];
        } else {
            $chosenVisitTypeCode = $user->appointment_follow_up_visit == "1" ? $visitTypeCodes['followUpVisit'] : $visitTypeCodes['establishedPatient'];
        }

        if ($user->appointment_vein_clinic_visit == "1") {
            $chosenVisitTypeCode = $visitTypeCodes['veinClinic'];
        }

        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            if ($user->location_id) {
                $user->appointment_physician_id = $user->findExternalProviderResourceIdForChosenServiceEntry();
                // save external provider resource id in session in case user chooses a location with mulitple external ids, chooses a time for a single id, then goes back in the flow to where appointment times must be displayed again
                $session = Craft::$app->session;
                $session->set('external_provider_resource_id', $user->appointment_physician_id);
                if ($user->appointment_physician_id && $user->save()) {
                    return $this->redirect('schedule/select-appointment');
                } else {
                    Craft::warning(new PsrMessage('User unable to proceed with selecting a location for given service in main scheduling flow. Is external provider resource ID missing?', [
                        'locationId' => $user->location_id,
                        'serviceId' => $serviceId
                    ]));
                }
            }
        }

        $path = '/_scheduling/locations/_main';
        if ($isAjax) {
            $path = '/_locations/_table';
        }
        return $this->renderTemplate($path, [
            'serviceId' => $serviceId,
            'serviceQuestionsCount' => $serviceQuestionsCount,
            'reasonForVisitId' => $reasonForVisitId,
            'totalCount' => count($locations),
            'locations' => $locations,
            'pagination' => $dataProvider->pagination,
            'hideSpecialtiesFilter' => true,
            'hideSearchByService' => true,
            'hideLocationServices' => true,
            'schedulingPostOnLocationClick' => true,
            'chosenVisitTypeCode' => $chosenVisitTypeCode
        ]);
    }

    /**
     * @return mixed
     */
    public function actionSelectAppointment()
    {
        $user = Craft::$app->patient_user->identity;
        $session = Craft::$app->session;

        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);
            $user->save();
        }

        if (!$user->getPhysicianIdReadyToBook() && !$user->location_id) {
            return $this->redirect('schedule');
        }

        // if appointment time chosen from the physician detail page, skip this step
        if (isset($user->apointment_time_selected_outside_of_scheduling)) {
            if ($user->apointment_time_selected_outside_of_scheduling == '1') {
                return $this->redirect('schedule/book');
            }
        }

        $physicianId = $session->get('external_provider_resource_id') ?? $user->getPhysicianIdReadyToBook();
        $selectedPhysician = Entry::find()->section('physicians')->id($physicianId)->one();
        $physicianLocations = !$selectedPhysician ? [] : Entry::find()
            ->section('locations')
            ->relatedTo($selectedPhysician)
            ->all();

        $selectedLocation = !$user->location_id ? null : Entry::find()
            ->section('locations')
            ->id($user->location_id)
            ->one();

        $error = null;
        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);

            if ($user->save()) {
                return $this->redirect('schedule/book');
            } else {
                foreach ($user->errors as $singleError) {
                    $error .= \implode(" ", $singleError) . " ";
                }
            }
        }

        return $this->renderTemplate('_scheduling/select-appointment/_select-appointment.twig', [
            'error' => $error,
            'physician' => $selectedPhysician,
            'physicianLocations' => $physicianLocations,
            'selectedLocation' => $selectedLocation

        ]);
    }

    /**
     * @return mixed
     */
    public function actionGetAppointmentTimes()
    {
        Craft::$app->response->format = \yii\web\Response::FORMAT_JSON;

        $form = new GetAppointmentTimesForm();
        $form->load([
            'GetAppointmentTimesForm' => \array_merge(
                Craft::$app->request->post(),
                Craft::$app->request->get()
            )
        ]);

        if (!$form->validate()) {
            Craft::$app->response->statusCode = 400;
            return $form->getErrors();
        }

        $user = Craft::$app->patient_user->identity;
        $session = Craft::$app->session;

        // read patient info
        $user->appointment_new_patient_visit = $form->isNewPatient();
        $user->appointment_follow_up_visit = $form->isFollowUpVisit();
        $user->appointment_vein_clinic_visit = $form->isVeinClinicVisit();
        $user->appointment_full_body_skin_exam_visit = $form->isFullBodySkinExamVisit();
        $user->save();

        if ($form->getSelectedPhysician() == null) {
            // This needs further cleanup. Why are we reutilizing the appointment_physician_id attribute
            // for collecting external resource IDs?
            $providerID = $session->get('external_provider_resource_id') ?? $user->appointment_physician_id ?? $form->physicianId;

            $providerIDs = strpos($providerID, ',') > 0 ? array_map('trim', explode(',', $providerID)) : [$providerID];
            $providers = [];
            foreach ($providerIDs as $providerId) {
                // While "human" providers can have a NPISER id, locations/labs/generic provider pools cannot.
                // For locations/labs/generic provider pools, we always use the "external" ID type.
                $providers[$providerId] = "External";
            }
        } else {
            $selectedPhysician = $form->getSelectedPhysician();

            // get the provider ID and ID Type
            // if NPISER is available, NPISER will be returned
            // if not, and "external" id is available, external id will be returned
            // else, empty values will be returned
            $providerIDAndIDType = SchedulingModule::getInstance()
                ->schedulingModuleEpicService
                ->getProviderIdAndIdType($form->getSelectedPhysician());

            $providerID = $providerIDAndIDType['providerID'];
            $providerIDType = $providerIDAndIDType['providerIDType'];

            $providers = [
                $providerID => $providerIDType
            ];
        }

        $appointmentTimesFromEPIC = $form->getAppointmentTimes(
            $providers,
            $selectedPhysician ?? null
        );

        if ($form->getSelectedPhysician() == null) {
            return $form->processAppointmentTimes($appointmentTimesFromEPIC, $user, $form->getSelectedService()->id, null, $providerID);
        } else {
            return $form->processAppointmentTimes($appointmentTimesFromEPIC, $user, $form->getSelectedService()->id, $selectedPhysician);
        }
    }

    /**
     * @return mixed
     */
    public function actionPhysicianAutoSuggestions()
    {
        $user = Craft::$app->patient_user->identity;
        $serviceIds = $user->getAppointmentServiceIds();
        $form = new PhysicianSearchForm();

        $form->load([
            'PhysicianSearchForm' => Craft::$app->request->get()
        ]);

        $entries = [];
        $entries = DupageCoreModule::getInstance()->dupageCoreModuleService->getAutoSuggestions(
            $form->search_physician_attribute,
            ['physicians']
        );

        $this->asJson($entries);
    }

    /**
     * @return mixed
     */
    public function actionBook()
    {
        $user = Craft::$app->patient_user->identity;
        $error = null;
        $existingPatientObject = null;

        if (Craft::$app->request->isPost) {
            $form = new PatientInformationForm();
            $form->load([
                'PatientInformationForm' => Craft::$app->request->post()
            ]);

            if ($form->validate()) {
                // see if patient exists
                $patientId = SchedulingModule::getInstance()->schedulingModuleEpicService->parseForm($form, $user, 'find');

                if ($patientId == null) {
                    // create patient
                    $patientId = SchedulingModule::getInstance()->schedulingModuleEpicService->parseForm($form, $user, 'create');

                    if ($patientId) {
                        // analytics
                        Craft::$app->session->setFlash('new-patient-record', true);
                    }
                }

                if ($patientId != null) {
                    // book appointment
                    $appointmentObject = SchedulingModule::getInstance()->schedulingModuleEpicService->parseForm($form, $user, 'book', $patientId);

                    if ($appointmentObject != null) {
                        //appointment data is cached for 30min in redis.
                        $key = "appointment_time_for_" . $user->id;
                        Craft::$app->cache->set($key, $appointmentObject, 1800);

                        $this->setupGoogleAnalyticsFlashes();

                        $user->resetCachedNextUpcomingAppointment();

                        return $this->redirect('schedule/book/share');
                    } else {
                        Craft::error(new PsrMessage('Invalid appointment object', [
                            'appointment_visit_type_id' => $user->appointment_visit_type_id
                        ]), 'modules/schedulingmodule/services/SchedulingModuleEpicService::parseForm');

                        $error = Craft::t('scheduling-module', 'Your appointment could not be scheduled at the date, time or location selected. Please click Back to update your appointment selection.');
                    }
                } else {
                    Craft::error(new PsrMessage('PatientId is invalid', [
                        'appointment_visit_type_id' => $user->appointment_visit_type_id
                    ]), 'modules/schedulingmodule/services/SchedulingModuleEpicService::parseForm');

                    $error = Craft::t('scheduling-module', 'Your appointment could not be scheduled at the date, time or location selected. Please click Back to update your appointment selection.');
                }
            }
        }

        $selectedPhysician = Entry::find()->section('physicians')->id($user->getPhysicianIdReadyToBook())->one();
        $service = Entry::find()->section('services')->id($user->getAppointmentServiceIds())->one();

        if (!$service) {
            return $this->redirect('schedule');
        }

        $suite = SchedulingModule::getInstance()->schedulingModuleService->findSuiteEntryForGivenExternalDepartmentID($user->appointment_department_id);

        if (isset($user->getPatients()[$user->appointment_current_epi]['details'])) {
            $object = $user->getPatients()[$user->appointment_current_epi]['details'];
            $existingPatientObject = SchedulingModule::getInstance()->schedulingModuleEpicService->parsePatientObject($object);
        }

        return $this->renderTemplate('/_scheduling/book/_main', [
            'physician' => $selectedPhysician,
            'service' => $service,
            'suite' => $suite,
            'error' => $error,
            'patientObject' => $existingPatientObject
        ]);
    }

    /**
     * @return mixed
     */
    public function actionShare()
    {
        $user = Craft::$app->patient_user->identity;
        $key = "appointment_time_for_" . $user->id;
        $appointmentData = Craft::$app->cache->get($key);

        if ($appointmentData != false) {
            $selectedPhysician = Entry::find()->section('physicians')->id($user->getPhysicianIdReadyToBook())->one();
            $services = Entry::find()->section('services')->id($user->getAppointmentServiceIds())->one();

            $suite = SchedulingModule::getInstance()->schedulingModuleService->findSuiteEntryForGivenExternalDepartmentID($user->appointment_department_id);

            $appointmentDate = $appointmentData["Date"];
            $appointmentTime = $appointmentData["Time"];

            return $this->renderTemplate('/_scheduling/share/_main', [
                'physician' => $selectedPhysician,
                'service' => $services,
                'appointmentDate' => $appointmentDate,
                'appointmentTime' => $appointmentTime,
                'suite' => $suite
            ]);
        }

        // redirect user back to schedule landing page if it's been more than 30 minutes from scheduling appointment
        return $this->redirect('schedule');
    }

    /**
     * @return mixed
     */
    public function actionShareEmail()
    {
        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        if ($isAjax) {
            $form = new ShareEmailForm();
            $form->load([
                'ShareEmailForm' => Craft::$app->request->get()
            ]);

            if ($form->validate()) {
                // Send appointment email
                Craft::$app->queue->push(new EmailJob([
                    'template' => 'appointment-notification.twig',
                    'subject' => Craft::t('scheduling-module', 'Appointment for ' . $form->sender_name),
                    'to' => $form->send_to,
                    'templateData' => [
                        'shareDetails' => $form->share_details,
                        'senderName' => $form->sender_name,
                        'physician' => $form->physician,
                        'time' => $form->time,
                        'location' => $form->location,
                        'visitType' => $form->visit_type
                    ],
                ]));

                return $this->asJson(['message' => 'success']);
            }
        }

        throw new HttpException(400, Craft::t('scheduling-module', 'Something went wrong. Please try again later.'));
    }

    /**
     * @return mixed
     */
    public function actionCalendar()
    {
        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        if ($isAjax) {
            $form = new CalendarForm();
            $form->load([
                'CalendarForm' => Craft::$app->request->get()
            ]);

            if ($form->validate()) {
                // create ICS file
                $response = Craft::$app->response;
                $response->format = Response::FORMAT_RAW;
                $headers = $response->headers;
                $headers->add('Content-Type', 'text/calendar; charset=utf-8');
                $headers->add('Content-Disposition', 'attachment; filename=invite.ics');
                $formData = [];

                // user has ability to only share certain details
                // remove location from details if not provided
                if ($form->location) {
                    $formData = [
                        'location' => $form->location,
                        'summary' => $form->description,
                        'dtstart' => $form->startTime
                    ];
                } else {
                    $formData = [
                        'summary' => $form->description,
                        'dtstart' => $form->startTime
                    ];
                }

                $ics = new ICS($formData);
                $file = $ics->to_string();
                $response->content = $file;

                return $response;
            }
        }

        throw new HttpException(400, Craft::t('scheduling-module', 'Something went wrong. Please try again later.'));
    }

    /**
     * @return mixed
     */
    public function actionCancelAppointment()
    {
        $user = Craft::$app->patient_user->identity;
        $key = "appointment_time_for_" . $user->id;
        $appointmentData = Craft::$app->cache->get($key);

        $appointmentCancelled = false;

        if ($appointmentData !== false) {
            $eid = null;
            foreach ($appointmentData['Patient']['IDs'] as $id) {
                if ($id['Type'] == "EPI") {
                    $eid = $id['ID'];
                    break;
                }
            }
            $csnContactId = null;
            foreach ($appointmentData['ContactIDs'] as $contact) {
                if ($contact['Type'] == "CSN") {
                    $csnContactId = $contact['ID'];
                    break;
                }
            }

            $appointmentCancelled = SchedulingModule::getInstance()->schedulingModuleEpicService->cancelAppointment($eid, $csnContactId);
        }

        if ($appointmentCancelled == true) {
            // cancellation successful
            Craft::$app->session->setFlash('edit-appointment-success', true);
        } else {
            // inform the user of the error and come back to the booking confirmation page
            Craft::$app->session->setFlash('cancel-appointment-failed', true);
        }


        return $this->redirect($user->is_video_visit_flow ? '/schedule/video-visit/confirm-video-visit' : 'schedule/book/share');
    }

    /**
     * Sets flash messages used for Google Analytics tracking on successful appointment booking
     */
    private function setupGoogleAnalyticsFlashes()
    {
        $user = Craft::$app->patient_user->identity;

        $physician = Entry::find()->id($user->getPhysicianIdReadyToBook())->one();

        $rating = !$user->isSchedulingFlowWithoutPhysicians() ? DupageCoreModule::getInstance()->dupageCoreModuleService->getProviderRatingAndComments($physician) : null;
        $overallRating = null;
        if (isset($rating['overallRating']) && isset($rating['overallRating']['value'])) {
            $overallRating = $rating['overallRating']['value'];
        }

        $service = SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($user->getAppointmentServiceIds(), true);

        Craft::$app->session->setFlash('national-physician-id', $physician->nationalProviderIdentifier ?? 'NA');
        Craft::$app->session->setFlash('appointment-service-chosen', $service);
        Craft::$app->session->setFlash('epic-physician-id', $physician->epicProviderId ?? 'NA');
        Craft::$app->session->setFlash('physician-rating', $overallRating);

        if ($user->deeplinked_session !== null) {
            Craft::$app->session->setFlash('make-appointment-success-clearstep', true);
        } else {
            Craft::$app->session->setFlash('make-appointment-success', true);
        }
    }

    /**
     * This API endpoint consumes a scheduling token, and redirects the user to /schedule/select-appointment
     * while also populating their session with data from the token.
     *
     * @return mixed
     */
    public function actionScheduleDeeplink()
    {
        $user = Craft::$app->patient_user->identity;

        if (!$user) {
            $user = PatientUser::anonymousUser();
            Craft::$app->patient_user->login($user, (60 * 20));
            $user->save();
        }

        // If a user deeplinks into the flow multiple times during the same session,
        // attempting to schedule for multiple patients, the previous patient's data
        // will be still stored in the session.
        // We should reset it.
        $user->resetUserData();

        $token = Craft::$app->request->get()['token'] ?? null;

        if ($token === null) {
            throw new HttpException(400);
        }

        $deeplinkData = Craft::$app->cache->get(SchedulingTokenForm::TOKEN_PREFIX . $token);
        if ($deeplinkData === false) {
            throw new HttpException(400, Craft::t('scheduling-module', 'Invalid token.'));
        }

        $user->setAppointmentServiceIds([$deeplinkData['service_id']]);
        $user->appointment_new_patient_visit = $deeplinkData['is_new_patient'];
        $user->conversation_id = $deeplinkData['conversation_id'];

        if (isset($deeplinkData['physician_id'])) {
            $user->appointment_physician_id = $deeplinkData['physician_id'];
        } elseif (isset($deeplinkData['location_id'])) {
            $user->location_id = $deeplinkData['location_id'];
            $user->appointment_physician_id = $user->findExternalProviderResourceIdForChosenServiceEntry();
        } elseif (isset($deeplinkData['telemedicine_id'])) {
            $user->location_id = $deeplinkData['telemedicine_id'];
            $user->appointment_physician_id = $user->findExternalProviderResourceIdForChosenServiceEntry();
        }

        // hides template "back" navigation
        $user->deeplinked_session = true;

        $user->save();

        return $this->redirect('schedule/select-appointment');
    }

    public function actionGetLocations()
    {
        MicrositesAuthenticationService::validateJwtToken();
        $request =  Craft::$app->request->get();
        $serviceEntry = Entry::find()->section('services')->slug($request['serviceSlug'])->one();
        $form = new LocationSearchForm();
        $form->load([
            'LocationSearchForm' => $request
        ]);

        $form->search_service_id = $serviceEntry->id;
        $dataProvider = $form->getDataProvider("scheduling-module/default/select-location",null, $request['pageSize']);
        $locations = $dataProvider->getModels();

        return json_encode($locations);
    }
}
