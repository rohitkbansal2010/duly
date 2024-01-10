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
use craft\elements\Entry;
use craft\web\Controller;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\models\PatientUser;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\forms\PatientInformationForm;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\filters\AccessControl;
use yii\filters\Cors;
use yii\filters\RateLimiter;
use yii\filters\VerbFilter;

/**
 * @author    Wipfli Digital
 * @package   SchedulingModule
 * @since     1.0.0
 */
class VideoVisitController extends Controller
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
                    'actions' => ['default', 'select-appointment', 'patient-info', 'confirm-video-visit'],
                    'roles' => ['?', '@'],
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
                'Origin' => [getenv('DEFAULT_SITE_URL')],
                'Access-Control-Request-Method' => ['GET', 'POST'],
                'Access-Control-Request-Headers' => ['*'],
            ]
        ];

        $behaviors['verbs'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'default'  => ['GET', 'POST'],
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

    public function actionDefault()
    {
        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        $user = Craft::$app->patient_user->identity;
        if (!$user) {
            $user = PatientUser::anonymousUser();
            $result = Craft::$app->patient_user->login($user, (60 * 20));
        }

        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);
            if ($user->save() && isset($user->privacy_agree, $user->is_adult, $user->is_in_illinois)) {
                return $this->redirect('/schedule/video-visit/select-appointment');
            }
        }

        $user->resetUserData();
        // flag indicating that the user is in a unique video visit flow
        $user->is_video_visit_flow = true;
        $user->save();

        return $this->renderTemplate('_scheduling/videovisit/_main.twig');
    }

    public function actionSelectAppointment()
    {
        $user = Craft::$app->patient_user->identity;
        if (!$user || !isset($user->privacy_agree, $user->is_adult, $user->is_in_illinois)) {
            return $this->redirect('/schedule/video-visit');
        }

        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);
            if ($user->save()) {
                if ($user->anonymous) {
                    Craft::$app->patient_user->setReturnUrl('/schedule/video-visit/patient-info');
                    return $this->redirect('/login-portal');
                } else {
                    return $this->redirect('/schedule/video-visit/patient-info');
                }
            }
        }

        $age = $user->is_adult ? 18 : 0;

        $serviceAndPhysicianData = Craft::$app->cache->getOrSet("get_cached_video_visit_appointment_page_data_${age}", function () use ($age) {
            return $this->getPhysiciansWithVideoVisitsPerServicePerDate($age);
        }, 60 * 5);

        return $this->renderTemplate('_scheduling/videovisit/_select-appointment.twig', [
            'serviceTiles' => $serviceAndPhysicianData['serviceTiles'],
            'physiciansWithVideoVisitsPerServicePerDate' => \json_encode($serviceAndPhysicianData['physiciansWithVideoVisitsPerServicePerDate'])
        ]);
    }

    private function getPhysiciansWithVideoVisitsPerServicePerDate($age)
    {
        // filter service tiles based on adult/child selection from previous step
        $videoVisitsSingle = Entry::find()->section('videoVisits')->one();
        $allServiceTiles = $videoVisitsSingle->videoVisitServiceTiles->all();
        $serviceTiles = [];
        $physiciansWithVideoVisitsPerServicePerDate = [];

        foreach ($allServiceTiles as $serviceTile) {
            $serviceIds = $serviceTile->service->ids();
            $validServices = SchedulingModule::getInstance()->schedulingModuleService->verifyServicesByAge($serviceIds, $age);
            if ($validServices) {
                sort($validServices);
                array_push($serviceTiles, $serviceTile);

                // get physicians offering this service
                $physicians = Entry::find()->section('physicians')->relatedTo($validServices)->all();

                $physiciansAdded = [];
                $physiciansWithVideoVisitsPerServicePerDate[$validServices[0]] = [];

                foreach ($physicians as $physician) {
                    $slots = [];

                    // get cached appointment times for this service at this location for this external resource ID
                    $appointmentSlotsPerDay = PhysiciansModule::getInstance()
                        ->physiciansModuleService
                        ->getCachedAppointmentTimesForPhysicianFromDate($physician, new \DateTime(), 14);

                    $visitTypeCode = "2990";

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

                            // skip appointment slots older than now; potential side-effect of using cached data
                            if ($appointmentSlot['Time'] < new \DateTime()) {
                                continue;
                            }

                            $date = $appointmentSlot['Time']->format('Y-m-d');

                            $physiciansAdded[$date] = $physiciansAdded[$date] ?? [];

                            if (!in_array($physician->id, $physiciansAdded[$date])) {
                                $physiciansAdded[$date][] = $physician->id;

                                $physiciansWithVideoVisitsPerServicePerDate[$validServices[0]][$date] = $physiciansWithVideoVisitsPerServicePerDate[$validServices[0]][$date] ?? [];

                                $physiciansWithVideoVisitsPerServicePerDate[$validServices[0]][$date][] = [
                                    'physician' => $physician,
                                    'date' => $appointmentSlot['Time']
                                ];
                            }
                        }
                    }
                }

                foreach ($physiciansWithVideoVisitsPerServicePerDate[$validServices[0]] as $date => $physicians) {
                    usort($physiciansWithVideoVisitsPerServicePerDate[$validServices[0]][$date], function ($a, $b) {
                        $diff = $a['date'] <=> $b['date'];
                        // sort by date
                        if ($diff != 0) {
                            return $diff;
                        }

                        // if equal, sort by start date
                        $aDate = $a['physician']->dupageMedicalGroupStartDate ? \DateTime::createFromFormat("m/d/Y", $a['physician']->dupageMedicalGroupStartDate) : new \DateTime();
                        $bDate = $b['physician']->dupageMedicalGroupStartDate ? \DateTime::createFromFormat("m/d/Y", $b['physician']->dupageMedicalGroupStartDate) : new \DateTime();

                        return $aDate < $bDate;
                    });
                    $physiciansWithVideoVisitsPerServicePerDate[$validServices[0]][$date] = \array_map(
                        fn ($p) => $p['physician']->id,
                        $physiciansWithVideoVisitsPerServicePerDate[$validServices[0]][$date]
                    );
                }
            }
        }

        return [
            'serviceTiles' => $serviceTiles,
            'physiciansWithVideoVisitsPerServicePerDate' => $physiciansWithVideoVisitsPerServicePerDate
        ];
    }

    /**
     * This action handles the "Provide Patient Info" page during the Video Visit scheduling flow
     */
    public function actionPatientInfo()
    {
        $user = Craft::$app->patient_user->identity;

        if (!$user) {
            return $this->redirect('/schedule/video-visit');
        }

        if (isset($user->getPatients()[$user->appointment_current_epi]['details'])) {
            $object = $user->getPatients()[$user->appointment_current_epi]['details'];
            $existingPatientObject = SchedulingModule::getInstance()->schedulingModuleEpicService->parsePatientObject($object);
        }

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

                        return $this->redirect('schedule/video-visit/confirm-video-visit');
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

        return $this->renderTemplate('_scheduling/videovisit/_patient-info.twig', [
            'patientObject' => $existingPatientObject ?? null,
            'patientUser' => $user,
            'physician' => Entry::find()->id($user->appointment_physician_id)->one(),
            'isSchedulingFlowWithoutPhysicians' => true,
            'service' => Entry::find()->section('services')->id($user->getAppointmentServiceIds())->one(),
            'error' => $error ?? ''
        ]);
    }

    /**
     * This action handles the "Confirmation page" page during the Video Visit scheduling flow
     */
    public function actionConfirmVideoVisit()
    {
        $user = Craft::$app->patient_user->identity;
        if (!$user) {
            $user = PatientUser::anonymousUser();
            Craft::$app->patient_user->login($user, (60 * 20));
        }

        if (isset($user->getPatients()[$user->appointment_current_epi]['details'])) {
            $object = $user->getPatients()[$user->appointment_current_epi]['details'];
            $existingPatientObject = SchedulingModule::getInstance()->schedulingModuleEpicService->parsePatientObject($object);
        }

        if (Craft::$app->request->isPost) {
            $user->load([
                'PatientUser' => Craft::$app->request->post()
            ]);
        }

        $key = "appointment_time_for_" . $user->id;
        $appointmentData = Craft::$app->cache->get($key);

        if ($appointmentData == null) {
            return $this->redirect('schedule/video-visit');
        }

        $appointmentDate = $appointmentData["Date"];
        $appointmentTime = $appointmentData["Time"];

        return $this->renderTemplate('/_scheduling/videovisit/_confirmation.twig', [
            'appointmentDate' => $appointmentDate,
            'appointmentTime' => $appointmentTime,
            'physician' => Entry::find()->id($user->appointment_physician_id)->one(),
        ]);
    }

    /**
     * Sets flash messages used for Google Analytics tracking on successful appointment booking
     */
    private function setupGoogleAnalyticsFlashes()
    {
        $user = Craft::$app->patient_user->identity;

        $physician = Entry::find()->id($user->getPhysicianIdReadyToBook())->one();

        $rating = !$user->isSchedulingFlowWithoutPhysicians() ? DupageCoreModule::getInstance()
            ->dupageCoreModuleService->getProviderRatingAndComments($physician) : null;
        $overallRating = null;
        if (isset($rating['overallRating']) && isset($rating['overallRating']['value'])) {
            $overallRating = $rating['overallRating']['value'];
        }

        $service = SchedulingModule::getInstance()->schedulingModuleService->getServiceDoctorName($user->getAppointmentServiceIds(), true);

        Craft::$app->session->setFlash('national-physician-id', $physician->nationalProviderIdentifier ?? 'NA');
        Craft::$app->session->setFlash('appointment-service-chosen', $service);
        Craft::$app->session->setFlash('epic-physician-id', $physician->epicProviderId ?? 'NA');
        Craft::$app->session->setFlash('physician-rating', $overallRating);

        if ($physician == null) {
            Craft::$app->session->setFlash('duly-now-by-time-or-by-provider', "By Time");
        } else {
            Craft::$app->session->setFlash('duly-now-by-time-or-by-provider', "By Provider");
        }

        Craft::$app->session->setFlash('make-appointment-success-duly-now-video-visit', true);
    }
}
