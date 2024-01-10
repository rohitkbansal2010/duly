<?php
/**
 * API Module for Craft 3 CMS
 *
 * Allows for exposing API endpoints
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2021 Wipfli Digital
 */


namespace modules\apimodule\controllers;

use DateTime;
use Craft;
use craft\web\Controller;
use craft\web\Response;
use modules\apimodule\forms\HospitalForm;
use modules\apimodule\forms\LocationForm;
use modules\apimodule\forms\PhysicianForm;
use modules\apimodule\forms\SchedulingTokenForm;
use modules\apimodule\forms\TelemedicineForm;
use modules\apimodule\forms\AppointmentForm;

use modules\apimodule\forms\ScheduleAppointmentForm;
use modules\schedulingmodule\services\SchedulingModuleEpicService;
use modules\apimodule\forms\ServiceForm;
use samdark\log\PsrMessage;
use yii\filters\AccessControl;
use yii\filters\RateLimiter;
use yii\filters\VerbFilter;
use yii\web\HttpException;
use modules\DupageCoreModule\models\PatientUser;

class DefaultController extends Controller
{
    /**
    * Allow anonymous access to all endpoints
    */
    protected $allowAnonymous = self::ALLOW_ANONYMOUS_LIVE;

    /**
     * Disable CSRF
     */
    public $enableCsrfValidation = false;

    public function behaviors()
    {
        $behaviors = parent::behaviors();

        $behaviors['verbs'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'ping' => ['GET'],
                'get-location' => ['GET'],
                'get-hospital' => ['GET'],
                'get-physician' => ['GET'],
                'get-service' => ['GET'],
                'generate-scheduling-token' => ['POST'],
                'get-appointment' => ['GET'],
                'schedule' => ['POST']
            ]
        ];

        $behaviors['access'] = [
            'class' => AccessControl::class,
            'only' => ['ping'],
            'rules' => [
                [
                    'allow' => true,
                    'actions' => ['ping'],
                    'roles' => ['?', '@']
                ]
            ]
        ];

        $behaviors['rateLimiter'] = [
            'class' => RateLimiter::class,
            'user' => Craft::$app->patient_user->identity,
        ];

        return $behaviors;
    }

    /**
     * Test endpoint for connection verification
     *
     * @return JsonResponse
     */
    public function actionPing()
    {
        return $this->asJson([
            "pong" => 1
        ]);
    }

    /**
     * This API endpoint returns a JSON array of location objects.
     *
     * @return JsonResponse
     *
     * Example response:
     * [
     *      {
     *          id: 30985,
     *          address: {
     *              number: "2100",
     *              street: "Glenwood Ave.",
     *              city: "Joliet",
     *              postcode: "60435",
     *              county: "Will County",
     *              state: "Illinois"
     *          },
     *          url: "https://192.168.64.13/location/2100-glenwood-avenue-joliet",
     *          appointment_slots_per_service: {
     *              546962: [
     *                  "2021-03-30T11:20:00-05:00",
     *                  "2021-03-30T11:40:00-05:00",
     *                  "2021-03-30T12:00:00-05:00"
     *              ]
     *          }
     *      }
     * ]
     */
    public function actionGetLocation()
    {
        $form = new LocationForm();

        if ($form->load(['LocationForm' => Craft::$app->request->get()]) && $form->validate()) {
            $dataProvider = $form->getLocations();

            $response = Craft::$app->response;
            $headers = $response->getHeaders();

            $pagination = $dataProvider->getPagination();
            $headers->set('X-Pagination-Total-Count', $dataProvider->getTotalCount());
            $headers->set('X-Pagination-Page-Count', $pagination->getPageCount());
            $headers->set('X-Pagination-Current-Page', $pagination->getPage());
            $headers->set('X-Pagination-Per-Page', $pagination->getPageSize());

            return $this->asJson(\array_values($dataProvider->getModels()));
        }

        Craft::error(new PsrMessage('Invalid API request.', [
            'uri' => '/api/v1/location',
            'errors' => $form->getErrors()
        ]));

        Craft::$app->response->statusCode = 400;
        return $this->asJson($form->getErrors());
    }

    /**
     * This API endpoint returns a JSON array of physician objects.
     *
     * @return JsonResponse
     *
     * Example response:
     * [
     *     {
     *         id: 486295,
     *         provider_id: 1992918510,
     *         provider_id_type": "NPISER",
     *         npi: 1881662534,
     *         title: "Michael P. Murphy, DO",
     *         appointment_slots: [
     *             "2021-04-16T08:45:00-05:00",
     *             "2021-04-16T09:45:00-05:00",
     *             "2021-04-16T10:45:00-05:00"
     *         ],
     *         address: {
     *             number: "2100",
     *             street: "Glenwood Ave.",
     *             city: "Joliet",
     *             postcode: "60435",
     *             county: "Will County",
     *             state: "Illinois",
     *             suite: "Suite A" || null
     *         },
     *         url: "https://192.168.64.13/physicians/michael-p-murphy-do",
     *         rating: 4.9 || null
     *     }
     * ]
     */
    public function actionGetPhysician()
    {
        $form = new PhysicianForm();

        if ($form->load(['PhysicianForm' => Craft::$app->request->get()]) && $form->validate()) {
            $dataProvider = $form->getPhysicians();

            $response = Craft::$app->response;
            $headers = $response->getHeaders();

            $pagination = $dataProvider->getPagination();
            $headers->set('X-Pagination-Total-Count', $dataProvider->getTotalCount());
            $headers->set('X-Pagination-Page-Count', $pagination->getPageCount());
            $headers->set('X-Pagination-Current-Page', $pagination->getPage());
            $headers->set('X-Pagination-Per-Page', $pagination->getPageSize());

            return $this->asJson($dataProvider->getModels());
        }

        Craft::error(new PsrMessage('Invalid API request.', [
            'uri' => '/api/v1/physician',
            'errors' => $form->getErrors()
        ]));

        Craft::$app->response->statusCode = 400;
        return $this->asJson($form->getErrors());
    }

    /**
     * This API endpoint returns a JSON array of service objects.
     *
     * @return JsonResponse
     *
     * Example response:
     * [
     *     {
     *         id: 13648,
     *         name: "Ophthalmology",
     *         resource: "physician" | "location"
     *         reasonsForVisit: [
     *             {
     *                 id: 623687,
     *                 name: "Blurry Vision",
     *                 description: "Lack of sharpness of vision with, as a result, the inability to see fine detail."
     *             }
     *         ]
     *     }
     * ]
     */
    public function actionGetService()
    {
        $form = new ServiceForm();

        if ($form->load(['ServiceForm' => Craft::$app->request->get()]) && $form->validate()) {
            $dataProvider = $form->getServices();

            $response = Craft::$app->response;
            $headers = $response->getHeaders();

            $pagination = $dataProvider->getPagination();
            $headers->set('X-Pagination-Total-Count', $dataProvider->getTotalCount());
            $headers->set('X-Pagination-Page-Count', $pagination->getPageCount());
            $headers->set('X-Pagination-Current-Page', $pagination->getPage());
            $headers->set('X-Pagination-Per-Page', $pagination->getPageSize());

            return $this->asJson($dataProvider->getModels());
        }

        Craft::error(new PsrMessage('Invalid API request.', [
            'uri' => '/api/v1/service',
            'errors' => $form->getErrors()
        ]));

        Craft::$app->response->statusCode = 400;
        return $this->asJson($form->getErrors());
    }

    /**
     * This API endpoint returns a JSON array of hospital objects.
     *
     * @return JsonResponse
     *
     * Example response:
     * [
     *      {
     *          name: "Advocate Lutheran General Hospital",
     *          phone: [
     *              "(847) 723-2210"
     *          ],
     *          address: {
     *              number: "1775",
     *              street: "Dempster St.",
     *              city: "Evanston",
     *              postcode: "60201",
     *              county: "Cook County",
     *              state: "Illinois"
     *          }
     *      }
     * ]
     */
    public function actionGetHospital()
    {
        $form = new HospitalForm();

        if ($form->load(['HospitalForm' => Craft::$app->request->get()]) && $form->validate()) {
            return $this->asJson($form->getHospitals());
        }

        Craft::error(new PsrMessage('Invalid API request.', [
            'uri' => '/api/v1/hospital',
            'errors' => $form->getErrors()
        ]));

        Craft::$app->response->statusCode = 400;
        return $this->asJson($form->getErrors());
    }

    /**
     * This API endpoint returns a JSON array of telemedicine appointment times.
     *
     * @return JsonResponse
     *
     * Example response:
     * {
     *     title: "Primary Care Video Visits",
     *     appointment_slots: [
     *         "2021-05-12T09:00:00-05:00",
     *         "2021-05-12T09:15:00-05:00",
     *         "2021-05-12T09:30:00-05:00",
     *         "2021-05-12T09:45:00-05:00",
     *         "2021-05-12T10:00:00-05:00"
     *     ]
     * }
     */
    public function actionGetTelemedicine()
    {
        $form = new TelemedicineForm();

        $dataProvider = $form->getTelemedicineAppointments();

        $response = Craft::$app->response;
        $headers = $response->getHeaders();

        $pagination = $dataProvider->getPagination();
        $headers->set('X-Pagination-Total-Count', $dataProvider->getTotalCount());
        $headers->set('X-Pagination-Page-Count', $pagination->getPageCount());
        $headers->set('X-Pagination-Current-Page', $pagination->getPage());
        $headers->set('X-Pagination-Per-Page', $pagination->getPageSize());

        return $this->asJson(\array_values($dataProvider->getModels()));
    }

    /**
     * This API endpoint returns a token used to deep-link into /schedule/select-appointment?token=<token>
     *
     * @return JsonResponse
     *
     * Example response:
     * {
     *     "token": "576646ff5dcac045dab7cf508d3880a4",
     *     "expiresAt": "2021-04-21T16:35:18-05:00"
     * }
     */
    public function actionGenerateSchedulingToken()
    {
        $form = new SchedulingTokenForm();

        if ($form->load(['SchedulingTokenForm' => Craft::$app->request->post()]) && $form->validate()) {
            return $this->asJson($form->generateToken());
        }

        Craft::error(new PsrMessage('Invalid API request.', [
            'uri' => '/api/v1/scheduling-token',
            'errors' => $form->getErrors()
        ]));

        Craft::$app->response->statusCode = 400;
        return $this->asJson($form->getErrors());
    }

    /**
     * This API endpoint returns a JSON array of appointment objects.
     *
     * @return JsonResponse
     *
     * Example response:
     *{
     *    "Provider": {
     *       "ID": "5004",
     *       "Type": "External"
     *    },
     *    "Department": {
     *        "ID": "17002",
     *        "Type": "External"
     *    },
     *    "VisitType": {
     *        "ID": "8004",
     *        "Type": "External"
     *    },
     *    "Time": {
     *        "date": "2022-03-21 08:00:00.000000",
     *        "timezone_type": 3,
     *        "timezone": "America/Chicago"
     *    }
     *}
     *
     */
    public function actionGetAppointment()
    {
        $user = Craft::$app->patient_user->identity;

        if (!$user) {
            $user = PatientUser::anonymousUser();
            Craft::$app->patient_user->login($user, (60 * 20));
            $user->save();
        }

        $form = new AppointmentForm();

        try {
            if ($form->load(['AppointmentForm' => Craft::$app->request->get()]) && $form->validate()) {
                return $this->asJson($form->getAppointments());
            }
        } catch (\Exception $e) {
            return $this->asJson($e->getMessage());
        }
    }

    /** This API endpoint is used to validate then forward a request to the Epic web service and returns a JSON array containing the Epic appointment object
     *
     * @return JsonResponse
     *
     * Example response:
     * {
     *   "appointment": {
     *      "Time": "11:30:00",
     *      "DurationInMinutes": 30,
     *      "Date": "2022-03-04",
     *      "PatientInstructions": [
     *          "Hello,",
     *           ...
     *      ],
     *      "Warnings": null,
     *      "Provider": {
     *          "DisplayName": "Guy J Agostino, MD",
     *          "IDs": [
     *              {
     *                  "ID": "2729",
     *                  "Type": "Dphone"
     *              },
     *            ...
     *          ]
     *      },
     *      "Department": {
     *          "Name": "Family Medicine - Burlington Ave, La Grange",
     *          "LocationInstructions": [
     *              "512 W BURLINGTON AVE",
     *              ...
     *          ],
     *          "IDs": [
     *              {
     *                  "ID": "19169",
     *                  "Type": "Internal"
     *              },
     *              ...
     *          ],
     *          "Address": {
     *              "StreetAddress": [
     *                  "512 W BURLINGTON AVE",
     *                  "SUITE 100"
     *              ],
     *              "City": "LA GRANGE",
     *              "PostalCode": "60525",
     *              "HouseNumber": "",
     *              "State": {
     *                  "Number": "14",
     *                  "Title": "Illinois",
     *                  "Abbreviation": "IL"
     *              },
     *              "Country": {
     *                  "Number": "1",
     *                  "Title": "United States",
     *                  "Abbreviation": "USA"
     *              },
     *              "District": {
     *                  "Number": "",
     *                  "Title": "",
     *                  "Abbreviation": ""
     *              },
     *              "County": {
     *                  "Number": "1",
     *                  "Title": "Cook",
     *                  "Abbreviation": "COOK"
     *              }
     *          },
     *          "Specialty": {
     *              "Number": "10062",
     *              "Title": "GE FAMILY PRACTICE",
     *              "Abbreviation": "GEFP",
     *              "ExternalName": ""
     *          },
     *          "OfficialTimeZone": {
     *              "Number": "5",
     *              "Title": "America/Chicago",
     *              "Abbreviation": "America/CHI"
     *          },
     *          "Phones": [
     *              {
     *                  "Type": "General",
     *                  "Number": "555-555-5555"
     *              },
     *              ...
     *          ]
     *      },
     *      "VisitType": {
     *          "Name": "NEW PT ACUTE VISIT",
     *          "DisplayName": "New Patient Office Visit",
     *          "PatientInstructions": [],
     *          "IDs": [
     *              {
     *                  "ID": "8004",
     *                  "Type": "Internal"
     *              },
     *              ...
     *          ]
     *      },
     *      "Patient": {
     *          "Name": "Test Patient",
     *          "IDs": [
     *              {
     *                  "ID": "1234567",
     *                  "Type": "EPI"
     *              },
     *              ...
     *          ]
     *      },
     *      "ContactIDs": [
     *          {
     *              "ID": "55352.99",
     *              "Type": "DAT"
     *          },
     *          ...
     *       ]
     *    }
     * }
     */
    public function actionSchedule()
    {
        $form = new ScheduleAppointmentForm();

        $form->load([
            'ScheduleAppointmentForm' => Craft::$app->request->post()
        ]);

        if ($form->validate()) {
            $schedulingService = new SchedulingModuleEpicService();
            try {
                $appointment = $schedulingService->scheduleAppointment(
                    $form->patient_id,
                    $form->patient_id_type,
                    $form->department_id,
                    $form->department_id_type,
                    $form->visit_type_id,
                    $form->visit_type_id_type,
                    DateTime::createFromFormat("Y-m-d\TH:i:sO", $form->appointment_time),
                    $form->provider_id,
                    $form->provider_id_type,
                    $form->insurance_member_id ?? '',
                    $form->insurance_group_id ?? '',
                    $form->insurance_provider_name ?? '',
                    $form->comments ?? ''
                );
                $data = ['appointment' => $appointment];
                return $this->asJson($data);
            } catch (\Exception $e) {
                Craft::error(new PsrMessage('Error scheduling appointment.', [
                    'uri' => '/api/v1/schedule',
                    'errors' => $e->getMessage()
                ]));
                Craft::$app->response->statusCode = 400;
                return $this->asJson(['message' => 'error scheduling appointment.']);
            }
        }

        Craft::error(new PsrMessage('Invalid API request.', [
            'uri' => '/api/v1/schedule',
            'errors' => $form->getErrors()
        ]));

        Craft::$app->response->statusCode = 400;
        return $this->asJson([
            'message' => 'invalid request',
            'errors' => $form->getErrors()
        ]);
    }
}
