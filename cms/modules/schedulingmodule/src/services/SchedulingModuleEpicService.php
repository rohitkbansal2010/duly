<?php

/**
 * Scheduling module for Craft CMS 3.x
 *
 * Allows for extended management of EPIC endpoints.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\schedulingmodule\services;

use Craft;
use DateInterval;
use DatePeriod;
use DateTime;
use DateTimezone;
use GuzzleHttp\Client as HTTPClient;
use GuzzleHttp\Exception\RequestException;
use GuzzleHttp\Promise as HTTPPromise;
use SimpleXMLElement;
use \GuzzleHttp\HandlerStack;
use \GuzzleHttp\Handler\CurlMultiHandler;
use craft\base\Component;
use craft\elements\Entry;
use craft\web\View;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\models\Locations;
use modules\DupageCoreModule\models\PatientUser;
use modules\schedulingmodule\SchedulingModule;
use modules\schedulingmodule\forms\PatientInformationForm;
use modules\schedulingmodule\services\Exceptions\InvalidEndDateException;
use modules\schedulingmodule\services\Exceptions\InvalidStartDateException;
use modules\schedulingmodule\services\Exceptions\SoapClientException;
use samdark\log\PsrMessage;
use yii\httpclient\Client as YiiHttpClient;
use yii\httpclient\Exception as HttpClientException;
use yii\httpclient\Response;

class Request
{
    public $method;
    public $uri;
    public $body;
}

/**
 * @author    Wipfli Digital
 * @package   SchedulingModule
 * @since     1.0.0
 */
class SchedulingModuleEpicService extends Component
{
    // 2 hours
    public static $PHYSICIAN_APPOINTMENT_TIMES_CACHE_DURATION = 60 * 60 * 2;

    // 1 hour
    public static $ALL_PHYSICIAN_APPOINTMENT_VISIT_TYPE_CODES_CACHE_DURATION = 60 * 60;

    // 1 hour
    public static $PHYSICIAN_EXTERNAL_DEPARTMENT_IDS_CACHE_DURATION = 60 * 60;

    // Public Methods
    // =========================================================================

    /**
     * Returns a collection of appointment slots for given physician NPIs.
     *
     * Example appointment slot:
     * [
     *      "123514456" => [
     *          [
     *              "Time" => DateTime,
     *              "Provider": [
     *                  "ID" => "1405",
     *                  "Type" => "External"
     *              ],
     *              "Department": [
     *                  "ID" => "25069",
     *                  "Type" => "External"
     *              ],
     *              "VisitType": [
     *                  "ID" => "227",
     *                  "Type" => "External"
     *              ]
     *          ]
     *      ]
     * ]
     *
     * @param DateTime $startDate
     * @param DateTime $endDate
     * @param array $providers - a collection of providers that need appointment times requested
     *      format:
     *      [
     *          $providerID => $providerIDType
     *      ]
     *      example:
     *      [
     *          "12345" => "external"
     *      ]
     * @param array $epicVisitTypeIDs - a collection of EPIC appointment visit type IDs; this limits the EPIC response to appointment times ONLY for those visit type IDs; [] will include all
     *      format:
     *      [
     *          [
     *              "ID" => $code,
     *              "Type" => "External"
     *          ]
     *      ]
     *      example:
     *      [
     *          [
     *              "ID" => "8001",
     *              "Type" => "External"
     *          ]
     *      ]
     * @param Entry $cmsPhysicianEntry
     * 
     * @return array array of appointment slots sorted by date and time ascending
     */
    public function getPhysicianAppointmentTimes(
        DateTime $startDate,
        DateTime $endDate,
        array $providers,
        array $epicVisitTypeIDs,
        Entry $cmsPhysicianEntry = null
    ): array {
        // validate / initialize boundary dates
        $startDate = $this->validateStartDate($startDate);
        $endDate = $this->validateEndDate($startDate, $endDate);

        $requests = [];

        foreach ($providers as $providerID => $providerIDType) {
            $request = new Request();
            $request->method = "POST";
            $request->uri = $this->getRootUrl() . '/api/epic/2017/PatientAccess/External/GetScheduleDaysForProvider2/Scheduling/Open/Provider/GetScheduleDays2';
            $request->body = [
                'json' => [
                    'StartDate' => $startDate->format('Y-m-d'),
                    'EndDate' => $endDate->format('Y-m-d'),
                    'ProviderIDType' => $providerIDType,
                    'ProviderID' => $providerID,
                    'VisitTypeIDs' => $epicVisitTypeIDs,
                    'DepartmentIDs' => $cmsPhysicianEntry ? Craft::$app->cache->getOrSet(
                        "physician_{$providerID}_external_department_ids",
                        function () use ($cmsPhysicianEntry) {
                            return $this->getPhysicianDepartmentIDs($cmsPhysicianEntry);
                        },
                        self::$PHYSICIAN_EXTERNAL_DEPARTMENT_IDS_CACHE_DURATION
                    ) : []
                ]
            ];

            \array_push(
                $requests,
                [
                    'providerID' => $providerID,
                    'request' => $request
                ]
            );
        }

        // make API calls
        $apiRequests = \array_map(
            function ($request) {
                return $request['request'];
            },
            $requests
        );

        // get API responses
        $apiResponses = $this->apiRequests(...\array_values($apiRequests));

        // Caches appointments data, and parses them to match the expected output
        $responses = $this->parsePhysicianAppointmentTimesResponses($apiResponses);

        // flatten the dates responses
        // cache API responses for each physician for each day
        \array_walk(
            $responses,
            function (&$response, $i) {
                $times = [];
                foreach ($response as $slots) {
                    $times = \array_merge($times, $slots);
                }
                $response = $times;
            }
        );

        if ($cmsPhysicianEntry) {
            $providerID =  $this->getProviderIdAndIdType($cmsPhysicianEntry)['providerID'];
            $providerIDType =  $this->getProviderIdAndIdType($cmsPhysicianEntry)['providerIDType'];

            if (isset($responses[$providerID]) && count($responses[$providerID]) != 0) {
                // stored in the CMS with Central timezone
                $savedPhysicianNextAvailableAppointmentTime = DateTime::createFromFormat("Y-m-d H:i:s", $cmsPhysicianEntry->physicianNextAvailableAppointmentTime, new DateTimezone("US/Central"));

                // EPIC returns US/Central
                $earliestAvailableEpicAppointmentTime = $responses[$providerID][0]['Time'];
                $earliestAvailableEpicAppointmentTime->setTimezone(new DateTimezone("US/Central"));

                $now = new DateTime();

                if ($savedPhysicianNextAvailableAppointmentTime < $now || $earliestAvailableEpicAppointmentTime < $savedPhysicianNextAvailableAppointmentTime) {
                    $query = Craft::$app->db
                        ->createCommand()
                        ->update('content', [
                            'field_physicianNextAvailableAppointmentTime' => $earliestAvailableEpicAppointmentTime->format("Y-m-d H:i:s") ?? null
                        ], [
                            'elementId' => $cmsPhysicianEntry->id
                        ]);

                    if ($query->execute() == 1) {
                        $providerInfo = $this->getProviderIdAndIDType($cmsPhysicianEntry);

                        Craft::info(new PsrMessage("Next Available Appointment Time Updated", \array_merge($providerInfo, [
                            'nextAvailableAppointmentTime' => $earliestAvailableEpicAppointmentTime->format("Y-m-d H:i:s") ?? null,
                            'physicianId' => $cmsPhysicianEntry->id,
                            'physicianTitle' => $cmsPhysicianEntry->title,
                        ])), get_class($this) . '::' . __METHOD__);
                    } else {
                        Craft::error(new PsrMessage('Unable to update next available appointment time', [
                            'physicianId' => $cmsPhysicianEntry->id,
                            'physicianTitle' => $cmsPhysicianEntry->title,
                        ]), get_class($this) . '::' . __METHOD__);
                    }
                }
            }
        }

        return $responses;
    }

    public function getResourcePhysicianAppointmentTimes(DateTime $startDate = null, DateTime $endDate = null, string $externalPhysicianId = null): array
    {
        // validate / initialize boundary dates
        $startDate = $this->validateStartDate($startDate);
        $endDate = $this->validateEndDate($startDate, $endDate);

        $requests = [];

        $providerID = strpos($externalPhysicianId, ',') > 0 ? array_map('trim', explode(',', $externalPhysicianId)) : [$externalPhysicianId];
        $providerIDType = 'External';

        foreach ($providerID as $id) {
            $request = new Request();
            $request->method = "POST";
            $request->uri = $this->getRootUrl() . '/api/epic/2017/PatientAccess/External/GetScheduleDaysForProvider2/Scheduling/Open/Provider/GetScheduleDays2';
            $request->body = [
                'json' => [
                    'StartDate' => $startDate->format('Y-m-d'),
                    'EndDate' => $endDate->format('Y-m-d'),
                    'ProviderIDType' => $providerIDType,
                    'ProviderID' => $id
                ]
            ];

            \array_push(
                $requests,
                [
                    'providerID' => $id,
                    'request' => $request
                ]
            );
        }

        // make API calls
        $apiRequests = \array_map(
            function ($request) {
                return $request['request'];
            },
            $requests
        );

        // get API responses
        $apiResponses = $this->apiRequests(...\array_values($apiRequests));

        // Caches appointments data, and parses them to match the expected output
        $responses = $this->parsePhysicianAppointmentTimesResponses($apiResponses);

        // flatted the dates responses
        // cache API responses for each physician for each day
        \array_walk(
            $responses,
            function (&$response, $i) {
                $times = [];
                foreach ($response as $slots) {
                    $times = \array_merge($times, $slots);
                }
                $response = $times;
            }
        );

        return $responses;
    }

    public function getAllVisitTypeIDs(Entry $physician = null)
    {
        $codes = [];

        if ($physician === null) {
            return $codes;
        }

        // get global settings
        $appointmentSchedulingVisitTypeCodes = Craft::$app->globals->getSetByHandle('appointmentSchedulingVisitTypeCodes');

        if ($physician->acceptsNewPatients) {
            $codes[] = [
                "ID" => $appointmentSchedulingVisitTypeCodes->newPatientDefaultVisitTypeCode,
                "Type" => "External"
            ];
        }

        $codes[] = [
            "ID" => $appointmentSchedulingVisitTypeCodes->establishedPatientDefaultVisitTypeCode,
            "Type" => "External"
        ];

        // clone the physicianSpeciality query
        // otherwise, we will modify the "physicianSpeciality" ElementQuery attribute value of the physician object
        $physicianServicesQuery = clone $physician->physicianSpeciality;
        $physicianServicesQuery = $physicianServicesQuery->select(['elements.id', 'field_allowFollowUpVisits', 'field_allowOnlineScheduling'])->all();
        foreach ($physicianServicesQuery as $service) {
            if ($service->allowFollowUpVisits && $service->allowOnlineScheduling) {
                $codes[] = [
                    "ID" => $appointmentSchedulingVisitTypeCodes->annualPhysicalVisitTypeCode,
                    "Type" => "External"
                ];
                $codes[] = [
                    "ID" => $appointmentSchedulingVisitTypeCodes->annualPhysicalNewPatientsVisitTypeCode,
                    "Type" => "External"
                ];
                $codes[] = [
                    "ID" => $appointmentSchedulingVisitTypeCodes->annualPhysicalVisitTypeCodeMedicarePlans,
                    "Type" => "External"
                ];
            } elseif ($service->allowOnlineScheduling) {
                $codes[] = [
                    "ID" => $appointmentSchedulingVisitTypeCodes->veinClinicCode,
                    "Type" => "External"
                ];
                $codes[] = [
                    "ID" => $appointmentSchedulingVisitTypeCodes->fullBodySkinExamCode,
                    "Type" => "External"
                ];
            }
        }

        $newPatientServiceBlocks = $appointmentSchedulingVisitTypeCodes->newPatientServiceSpecificVisitTypeCodes->all();
        foreach ($newPatientServiceBlocks as $newPatientServiceBlock) {
            $services = $newPatientServiceBlock->services->all();
            foreach ($services as $service) {
                if (\array_search($service->id, array_column($physicianServicesQuery, 'id')) !== false) {
                    $codes[] = [
                        "ID" => $newPatientServiceBlock->code,
                        "Type" => "External"
                    ];
                }
            }
        }

        $establishedPatientServiceBlocks = $appointmentSchedulingVisitTypeCodes->establishedPatientServiceSpecificVisitTypeCodes->all();
        foreach ($establishedPatientServiceBlocks as $establishedPatientServiceBlock) {
            $services = $establishedPatientServiceBlock->services->all();
            foreach ($services as $service) {
                if (\array_search($service->id, array_column($physicianServicesQuery, 'id')) !== false) {
                    $codes[] = [
                        "ID" => $establishedPatientServiceBlock->code,
                        "Type" => "External"
                    ];
                }
            }
        }

        // video visits
        $codes[] = [
            "ID" => 2990,
            "Type" => "External"
        ];

        return $codes;
    }

    private function getPhysicianDepartmentIDs(Entry $physician)
    {
        $suites = Entry::find()
            ->section('locations')
            ->type('suite')
            ->relatedTo([
                'field' => 'physicianLocations',
                'sourceElement' => $physician
            ])
            ->with('suiteServices')
            ->all();

        $physicianServiceIDs = $physician->physicianSpeciality->ids();

        $departmentIDs = [];

        foreach ($suites as $suite) {
            foreach ($suite->suiteServices as $suiteService) {
                $suiteServiceId = $suiteService->serviceType->ids()[0] ?? null;
                if ($suiteServiceId && \in_array($suiteServiceId, $physicianServiceIDs) && !empty($suiteService->externalDepartmentId)) {
                    $departmentIDs[] = [
                        "ID" => $suiteService->externalDepartmentId,
                        "Type" => "External"
                    ];
                }
            }
        }

        return $departmentIDs;
    }

    /**
     * Returns patient ID if a unique patient is found, null otherwise.
     * Patient type is always EPI.
     *
     * Example appointment slot:
     * [
     *      "ID" => "3258559",
     *      "Type => "EPI"
     * ]
     *
     * @param DateTime $dob
     * @param string $gender
     * @param string $firstName
     * @param string $lastName
     * @param string $street
     * @param string $streetLine2
     * @param string $state
     * @param string $postalCode
     * @param string $city
     * @param string $email
     * @return string|null returns patient's ID if unique match is found, null otherwise
     */
    public function findPatient(
        DateTime $dob,
        string $gender,
        string $firstName,
        string $lastName,
        string $email,
        string $phone,
        string $street = null,
        string $streetLine2 = null,
        string $state = null,
        string $postalCode = null,
        string $city = null
    ): ?string {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2017/EMPI/External/PatientLookup3/Patient/Lookup3';
        $request->body =  [
            'json' => [
                'DOB' => $dob->format("Y-m-d"),
                'Gender' => $gender,
                'Name' => "{$firstName} {$lastName}",
                'Demographics' => [
                    'Phones' => [
                        ['Number' => $phone]
                    ],
                    'Email' => $email
                ]
            ]
        ];

        // make API calls
        $patient = $this->apiRequests($request);

        if (count($patient) == 0 || !isset($patient[0]['IDs'])) {
            Craft::info(new PsrMessage('Patient Lookup: Patient Not Found'), get_class($this) . '::' . __METHOD__);
            return null;
        }

        $patient = $patient[0];

        foreach ($patient['IDs'] as $id) {
            if ($id['Type'] == "EPI") {
                Craft::info(new PsrMessage('Patient Lookup: Patient Found', [
                    'patientId' => $id['ID'],
                    'patientIdType' => $id['Type']
                ]), get_class($this) . '::' . __METHOD__);
                return $id['ID'];
            }
        }

        Craft::error(
            Craft::t(
                'scheduling-module',
                'Something went wrong when looking for a patient.'
            ),
            __METHOD__
        );

        return null;
    }

    /**
     * Returns the patient object, minus SSN.
     *
     * @param string $id
     * @param string $idType
     * @return array|null returns patient's ID if unique match is found, null otherwise
     */
    public function findPatientByID(string $id, string $idType): ?array
    {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2017/EMPI/External/PatientLookup3/Patient/Lookup3';
        $request->body =  [
            'json' => [
                "PatientIDType" => $idType,
                "PatientID" => $id
            ]
        ];

        // make API calls
        $patient = $this->apiRequests($request);

        if (count($patient) == 0 || !isset($patient[0]['IDs'])) {
            Craft::info(new PsrMessage('Patient Lookup: Patient not Found', [
                'patientId' => $id,
                'patientIdType' => $idType
            ]), get_class($this) . '::' . __METHOD__);
            return null;
        }

        Craft::info(new PsrMessage('Patient Lookup: Patient Found', [
            'patientId' => $id,
            'patientIdType' => $idType
        ]), get_class($this) . '::' . __METHOD__);

        $patientObject = $patient[0];

        // drop SSN from memory
        $patientObject['SSN'] = null;

        // remove unnecessary data
        $patient = [];
        $patient['Name'] = $patientObject['Name'];
        $patient['DOB'] = $patientObject['DOB'];
        $patient['Gender'] = $patientObject['Gender'];
        $patient['IDs'] = $patientObject['IDs'];
        $patient['Addresses'] = $patientObject['Addresses'];
        $patient['NameComponents'] = $patientObject['NameComponents'];

        return $patient;
    }

    /**
     * Creates a patient record in given department.
     * Returns patient ID if successful, null otherwise.
     * Patient type is always EPI.
     *
     * Example appointment slot:
     * [
     *      "ID" => "3258559",
     *      "Type => "EPI"
     * ]
     *
     * @param DateTime $dob
     * @param string $gender
     * @param string $firstName
     * @param string $lastName
     * @param string $departmentID login department ID in which the patient is created.
     * @param string $departmentIDType login department type in which patient is created.
     * @param string $street
     * @param string $streetLine2
     * @param string $state
     * @param string $postalCode
     * @param string $city
     * @param string $email
     * @return string|null returns patient's ID if unique match is found, null otherwise
     */
    public function createPatient(
        DateTime $dob,
        string $gender,
        string $firstName,
        string $lastName,
        string $email,
        string $phone,
        string $departmentID,
        string $departmentIDType,
        string $street = null,
        string $streetLine2 = null,
        string $state = null,
        string $postalCode = null,
        string $city = null
    ): ?string {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2012/EMPI/External/PatientCreate/Patient/Create';
        $request->body =  [
            'json' => [
                'DateOfBirth' => $dob->format("Y-m-d"),
                'Gender' => $gender,
                'Name' => [
                    'First' => $firstName,
                    'LastName' => $lastName
                ],
                'DepartmentID' => $departmentID,
                'DepartmentIDType' => $departmentIDType,
                'Address' => [
                    'Street' => $street,
                    'StreetLine2' => $streetLine2,
                    'State' => $state,
                    'PostalCode' => $postalCode,
                    'City' => $city,
                    'Email' => $email,
                    'Phones' => [
                        ['Number' => $phone, 'Type' => 'Mobile']
                    ]
                ]
            ]
        ];

        // make API calls
        $patient = $this->apiRequests($request);

        if (count($patient) == 0 || !isset($patient[0]['PatientIDs'])) {
            Craft::info(new PsrMessage('Patient Create: Unable to Create Patient'), get_class($this) . '::' . __METHOD__);
            return null;
        }

        $patient = $patient[0];

        foreach ($patient['PatientIDs'] as $id) {
            if ($id['Type'] == "EPI") {
                Craft::info(new PsrMessage('Patient Create: Patient Created', [
                    'patientId' => $id['ID'],
                    'patientIdType' => $id['Type']
                ]), get_class($this) . '::' . __METHOD__);

                return $id['ID'];
            }
        }

        Craft::error(
            Craft::t(
                'scheduling-module',
                'Something went wrong when creating a patient.'
            ),
            __METHOD__
        );

        return null;
    }

    /**
     * Schedules an appointment.
     *
     * @param string $patientID
     * @param string $patientIDType
     * @param string $departmentID
     * @param string $departmentIDType
     * @param string $visitTypeID
     * @param string $visitTypeIDType
     * @param DateTime $date appointment date and time
     * @param string $providerID
     * @param string $providerIDType
     * @param string[] $comments additional comments about the visit from the patient
     * @return array|null appointment details if successful, or null otherwise
     */
    public function scheduleAppointment(
        string $patientID,
        string $patientIDType,
        string $departmentID,
        string $departmentIDType,
        string $visitTypeID,
        string $visitTypeIDType,
        DateTime $date,
        string $providerID,
        string $providerIDType,
        string $insuranceMemberId = null,
        string $insuranceGroupId = null,
        string $insuranceProviderName = null,
        String ...$comments
    ): ?array {
        $requestData = [
            'PatientID' => $patientID,
            'PatientIDType' => $patientIDType,
            'DepartmentID' => $departmentID,
            'DepartmentIDType' => $departmentIDType,
            'VisitTypeID' => $visitTypeID,
            'VisitTypeIDType' => $visitTypeIDType,
            'Date' => $date->format('Y-m-d'),
            // always 0
            'IsReviewOnly' => 'false',
            'ProviderID' => $providerID,
            'ProviderIDType' => $providerIDType,
            'Time' =>  $date->format('H:i:s')
        ];

        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2018/PatientAccess/External/ScheduleAppointmentWithInsurance/Scheduling2018/Open/ScheduleWithInsurance'
            . '?' . http_build_query($requestData);
        $request->body = [
            'json' => [
                'Comments' => $comments,
            ]
        ];

        if ($insuranceMemberId !== '' && $insuranceGroupId !== '') {
            $request->body['json']['Insurance'] = [
                'MemberNumber' => $insuranceMemberId,
                'GroupNumber' => $insuranceGroupId,
                'InsuranceName' => $insuranceProviderName
            ];
        }

        // make API calls
        $appointment = $this->apiRequests($request);

        if (count($appointment) == 0 || !isset($appointment[0]['Appointment'])) {
            Craft::info(new PsrMessage('Something went wrong when scheduling an appointment.', [
                'response' => $appointment
            ]), get_class($this) . '::' . __METHOD__);
            return null;
        }

        Craft::info(new PsrMessage('Appointment Scheduled', [
            'date' => $appointment[0]['Appointment']['Date'],
            'time' => $appointment[0]['Appointment']['Time'],
            'provider' => $appointment[0]['Appointment']['Provider'],
            'patient' => $appointment[0]['Appointment']['Patient']['IDs'],
            'department' => $appointment[0]['Appointment']['Department'],
            'visitType' => $appointment[0]['Appointment']['VisitType'],
            'contact' => $appointment[0]['Appointment']['ContactIDs']
        ]), get_class($this) . '::' . __METHOD__);

        return $appointment[0]['Appointment'];
    }

    /**
     * Exchanges oauth 2 authorization code for patient ID
     *
     * @param string $code oauth2 authorization code
     * @return string|null Epic /patient ID, or null otherwise
     */
    public function exchangeOauthCode(string $code): ?string
    {
        $headers = [
            'Epic-Client-ID' => Craft::$app->config->general->epic_client_id,
            'Content-Type' => 'application/x-www-form-urlencoded'
        ];

        $client = new HTTPClient($headers);

        try {
            $response = $client->post(getenv('OAUTH_TOKEN_URL'), [
                'form_params' => [
                    'grant_type' => 'authorization_code',
                    'code' => $code,
                    'redirect_uri' => getenv('OAUTH_REDIRECT_URI'),
                    'client_id' => Craft::$app->config->general->epic_client_id
                ]
            ]);
            $response = \json_decode($response->getBody(), true);
        } catch (RequestException $e) {
            Craft::error('Unable to exchange OAuth2 authorization code');
            Craft::error($e);
        }

        // Return the EPI
        if ($response['patient'] !== null) {
            return $this->getPatientIdentifiers($response['patient'], 'FHIR');
        }

        return null;
    }

    /**
     * Retrieves a valid physician id and id type for given physician
     *
     * @param Entry $physsician
     * @return array
     */
    public function getProviderIdAndIdType(Entry $physician)
    {
        $providerIDType = 'NPISER';
        $providerID = $physician->nationalProviderIdentifier;

        if (!$providerID) {
            $providerIDType = 'External';
            $providerID = $physician->epicProviderId;

            if (!$providerID) {
                $providerIDType = '';
                $providerID = '';
            }
        }

        return [
            'providerIDType' => $providerIDType,
            'providerID' => $providerID,
        ];
    }

    /**
     * Retrieves the EPI for the main patient.
     *
     * @param string $epicPatientID Epic patient ID
     * @return string|null main patient EPI, or null if something went wrong
     */
    public function getPatientIdentifiers(string $epicPatientID, string $patientIdType = 'FHIR'): ?string
    {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2015/Common/Patient/GetPatientIdentifiers/Patient/Identifiers';
        $request->body = [
            "json" => [
                "PatientID" => $epicPatientID,
                "PatientIDType" => $patientIdType, // static value
                "UserID" => "28281", // static value
                "UserIDType" => "External" // static value
            ]
        ];

        // make API calls
        $response = $this->apiRequests($request);

        if (count($response) == 0 || !isset($response[0]['Identifiers'])) {
            Craft::error(
                Craft::t(
                    'scheduling-module',
                    'Unable to get patient identifiers.'
                ),
                __METHOD__
            );
            return null;
        }

        $epi = null;
        foreach ($response[0]['Identifiers'] as $identifier) {
            if ($identifier['IDType'] == 'EPI') {
                $epi = $identifier['ID'];
                break;
            }
        }

        return $epi;
    }

    /**
     * Returns the list of NPIs from past appointments.
     * Sorted by the most recent visit's NPI.
     *
     * @param string $epi patient's EPI
     * @return array|null returns physician's ID if unique match is found, null otherwise
     */
    public function getPreviouslySeenProviders(string $epi): ?array
    {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2015/PatientAccess/External/GetPastAppointments/Scheduling/Past';
        $request->body =  [
            'json' => [
                "Patient" => [
                    "ID" => $epi,
                    "Type" => "EPI"
                ]
            ]
        ];

        // make API calls
        $response = $this->apiRequests($request);

        if (count($response) == 0 || !isset($response[0]['Appointments'])) {
            return null;
        }

        try {
            if (!is_array($response[0]['Appointments'])) {
                Craft::warning(new PsrMessage('Bad data from GetPastAppointments', [
                    'patientIdType' => 'EPI',
                    'patientId' => $epi
                ]));
                return null;
            }

            static $npis = [];
            foreach ($response[0]['Appointments'] as $k => $val) {
                if (isset($val["ProviderDepartments"][0]) && isset($val["ProviderDepartments"][0]['Provider']['IDs'])) {
                    foreach ($val["ProviderDepartments"][0]['Provider']['IDs'] as $providerId) {
                        static $physician = null;
                        if (empty($providerId['ID'])) {
                            continue;
                        }
                        if ($providerId['Type'] == 'NPISER') {
                            $physician = Entry::find()
                                ->select('entries.id')
                                ->section('physicians')
                                ->nationalProviderIdentifier($providerId['ID'])
                                ->one();
                        } elseif ($providerId['Type'] == 'External') {
                            $physician = Entry::find()
                                ->select('entries.id')
                                ->section('physicians')
                                ->epicProviderId($providerId['ID'])
                                ->one();
                        }

                        if ($physician !== null) {
                            $npis[] = $physician->id;
                        }
                    }
                }
            }

            $npis = \array_unique($npis);

            if (count($npis) == 0) {
                return null;
            }

            return $npis;
        } catch (\Exception $e) {
            Craft::warning(new PsrMessage('Bad data from GetPastAppointments', [
                'patientIdType' => 'EPI',
                'patientId' => $epi
            ]), 'modules/schedulingmodule/services/SchedulingModuleEpicService::getPreviouslySeenProviders');
            Craft::error($e, 'modules/schedulingmodule/services/SchedulingModuleEpicService::getPreviouslySeenProviders');
            return null;
        }
    }

    /**
     * Returns the next upcoming appointment.
     *
     * @param string $epi patient's EPI
     * @return array|null returns patient's ID if unique match is found, null otherwise
     */
    public function getNextUpcomingAppointment(string $epi): ?array
    {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2017B/PatientAccess/Patient/GetFutureAppointments/Epic/Patient/Scheduling2017b/GetFutureAppointments';
        $request->body =  [
            'json' => [
                "PatientID" => $epi,
                "PatientIDType" => "EPI"
            ]
        ];

        // make API calls
        $response = $this->apiRequests($request);

        if (count($response) == 0 || !isset($response[0]['Appointments'])) {
            return null;
        }

        $appointmentObject = $response[0]['Appointments'][0] ?? null;
        if (!$appointmentObject) {
            return $appointmentObject;
        }

        // remove unnecessary data
        $appointment = [];
        $appointment['Date'] = $appointmentObject['Date'];
        $appointment['Time'] = $appointmentObject['Time'];
        $appointment['ProviderDepartments'] = $appointmentObject['ProviderDepartments'];

        return $appointment;
    }

    /**
     * Attempts to cancel an appointment. May not be available for all appointments.
     *
     * @param string $epi
     * @param string $csnContactId
     * @return bool true if success, false if not
     */
    public function cancelAppointment(string $epi, string $csnContactId): bool
    {
        $request = new Request();
        $request->method = "POST";
        $request->uri = $this->getRootUrl() . '/api/epic/2012/PatientAccess/External/CancelAppointment/Scheduling/Open/Cancel';
        $request->body =  [
            'json' => [
                "PatientID" => $epi,
                "PatientIDType" => "EPI",
                "ContactID" => $csnContactId,
                "ContactIDType" => "CSN"
            ]
        ];

        // make API calls
        $response = $this->apiRequests($request);

        if (count($response) == 0 || !isset($response[0]['IsSuccess'])) {
            return false;
        }

        return $response[0]['IsSuccess'];
    }

    /**
     * Submits and array of requests
     *
     * @param Request[] $request]
     */
    private function apiRequests(Request ...$requests)
    {
        $client = Craft::$app->httpclient->getClient();
        $batchRequests = [];
        foreach ($requests as $id => $request) {
            $batchRequests[] = $this->generateHttpRequest(
                $client,
                $request->uri,
                $request->method,
                YiiHttpClient::FORMAT_JSON,
                [
                    'Epic-Client-ID' => getenv('EPIC_CLIENT_ID'),
                    'Authorization' => 'Basic ' .  \base64_encode(getenv('EPIC_REST_USERNAME') . ':' .  getenv('EPIC_REST_PASSWORD'))
                ],
                isset($request->body['json']) ? $request->body['json'] : null
            );
        }

        $responses = $client->batchSend($batchRequests);

        $bodies = [];
        foreach ($responses as $response) {
            if ($response->getHeaders()->get('http-code') != 200) {
                if ($response->getHeaders()->get('http-code') >= 500) {
                    // If we get a 500 back, log the url and action for analysis later then skip to next response
                    Craft::error(new PsrMessage('500 response from REST Call', [
                        'url' => $request->uri,
                        'status' => $response->getHeaders()->get('http-code'),
                        'headers' => $response->getHeaders()->toArray()
                    ]), get_class($this) . '::' . __METHOD__);
                    $bodies[] = [];
                    continue;
                }
                // If we get a non 200 back, log the url and action for analysis later
                Craft::error(new PsrMessage('REST Call Failed', [
                    'url' => $request->uri,
                    'status' => $response->getHeaders()->get('http-code'),
                    'headers' => $response->getHeaders()->toArray(),
                    'request' => \stripos($request->uri, 'GetScheduleDays2') !== false ? $request->body['json'] : [],
                    'response' => \stripos($request->uri, 'GetScheduleDays2') !== false ? $response->getData() : []
                ]), get_class($this) . '::' . __METHOD__);
            }

            $bodies[] = $response->getData();
        }

        return $bodies;
    }

    /**
     * Caches appointments data, and parses them to match the expected output
     * @param array $responses - EPIC endpoint response
     */
    private function parsePhysicianAppointmentTimesResponses(array $responses)
    {
        $retval = [];

        foreach ($responses as $response) {
            if (!isset($response['Provider']) || !isset($response['ScheduleDays'])) {
                continue;
            }

            $npi = null;
            $eid = null;
            foreach ($response['Provider']['IDs'] as $val) {
                if ($val['Type'] === 'NPISER' && Entry::find()->section('physicians')->nationalProviderIdentifier($val['ID'])->one()) {
                    $npi = $val['ID'];
                } elseif ($val['Type'] === 'External') {
                    $eid = $val['ID'];
                }
            }

            $physicianID = $npi ?? $eid;

            $retval[$physicianID] = $retval[$physicianID] ?? [];

            foreach ($response['ScheduleDays'] as $scheduleDay) {
                Craft::$app->cache->delete(
                    "physician_{$physicianID}_appointment_times_rt_{$scheduleDay['Date']}"
                );

                foreach ($scheduleDay['Slots'] as $slot) {
                    $appointmentSlot = [
                        'Provider' => $scheduleDay['Provider'],
                        'Department' => $scheduleDay['Department'],
                        'VisitType' => $scheduleDay['VisitType']
                    ];

                    $retval[$physicianID][$scheduleDay['Date']] = $retval[$physicianID][$scheduleDay['Date']] ?? [];

                    $appointmentSlot = \array_merge($appointmentSlot, $slot);
                    unset($appointmentSlot['DisplayTime']);
                    unset($appointmentSlot['ArrivalTime']);

                    $appointmentSlot['Time'] = DateTime::createFromFormat("Y-m-d H:i:s", $scheduleDay['Date'] . " " . $appointmentSlot['Time']);

                    \array_push(
                        $retval[$physicianID][$scheduleDay['Date']],
                        $appointmentSlot
                    );
                }
            }

            // sort date keys A-Z
            \ksort($retval[$physicianID]);

            foreach ($retval[$physicianID] as $dayKey => $timesArray) {
                \usort(
                    $timesArray,
                    function ($a, $b) {
                        return $a['Time'] > $b['Time'];
                    }
                );

                $retval[$physicianID][$dayKey] = $timesArray;

                // store in cache sorted appointment times for given day for given physician
                // this can be used to retrieve next available appointment
                Craft::$app->cache->set(
                    "physician_{$physicianID}_appointment_times_rt_{$dayKey}",
                    $timesArray,
                    self::$PHYSICIAN_APPOINTMENT_TIMES_CACHE_DURATION
                );
            }
        }

        return $retval;
    }

    private function getHTTPClient()
    {
        $headers = [
            'Epic-Client-ID' => getenv('EPIC_CLIENT_ID'),
            'Authorization' => 'Basic ' .  \base64_encode(getenv('EPIC_REST_USERNAME') . ':' .  getenv('EPIC_REST_PASSWORD')),
            'Content-Type' => 'application/json'
        ];

        $options = ['headers' => $headers];

        return new HTTPClient($options);
    }

    private function validateStartDate(DateTime $startDate = null)
    {
        if (!$startDate) {
            $startDate = new DateTime();
        }
        $startDate->setTime(0, 0, 0, 0);

        // start date must be today or later
        if ($startDate < (new DateTime())->setTime(0, 0, 0, 0)) {
            throw new InvalidStartDateException("Invalid start date");
        }

        return  $startDate;
    }

    private function validateEndDate(DateTime $startDate = null, DateTime $endDate = null)
    {
        if (!$endDate) {
            $endDate = clone $startDate;
            $endDate->add(new DateInterval("P7D"));
        }
        $endDate->setTime(0, 0, 0, 0);

        // end date must be 30 days or less since start date
        if ($startDate->diff($endDate)->days > 30) {
            throw new InvalidEndDateException("Invalid end date");
        }

        return  $endDate;
    }

    public function parseForm(PatientInformationForm $form, PatientUser $user, string $action = 'find', string $patientId = null)
    {
        $dob = Craft::$app->patient_user->identity->getDob();
        $gender = $form->gender;
        $firstName = $form->firstName;
        $lastName = $form->lastName;
        $patientName = $firstName . ' ' . $lastName;
        $email = $form->emailAddress;
        $phone = $form->phoneNumber;
        $street = $form->primaryAddress;
        $streetLine2 = $form->secondaryAddress;
        $state = $form->state;
        $postalCode = $form->zipcode;
        $city = $form->city;
        $patientIDType = "EPI";
        $departmentID = $user->appointment_department_id;
        $departmentIDType = "External";
        $visitTypeID = $user->appointment_visit_type_id;
        $insuranceMemberId = $form->insurance_member_id;
        $insuranceGroupId = $form->insurance_group_id;
        $insuranceProviderName = $user->getInsuranceProviderName();

        // if Medicare follow-up visit, we need to change the visit type code to Medicare-specific
        if ($user->appointment_follow_up_visit == "1") {
            $insurancePlan =  Entry::find()->id($user->appointment_insurance_plan_id)->one();
            if ($insurancePlan && $insurancePlan->isPlanMedicare) {
                $visitTypeID = SchedulingModule::getInstance()
                    ->schedulingModuleService
                    ->getSchedulingVisitTypeCodesForService($user->getAppointmentServiceIds()[0])['followUpVisitMedicare'];
            }
        }
        $visitTypeIDType = "External";
        $appointmentDate = DateTime::createFromFormat("Y-m-d\TH:i:sO", $user->appointment_time);
        $comments = '';

        if ($user->isSchedulingFlowWithoutPhysicians()) {
            $physician =  Entry::find()->id($user->getPhysicianIdReadyToBook())->one();
            if ($physician) {
                $providerIdAndType = $this->getProviderIdAndIdType($physician);
            } else {
                $providerIdAndType = [
                    'providerIDType' => 'External',
                    'providerID' => $user->getPhysicianIdReadyToBook(),
                ];
            }
        } else {
            $providerIdAndType = $this->getProviderIdAndIdType(Entry::find()->section('physicians')->id($user->getPhysicianIdReadyToBook())->one());
        }

        if ($form->patientReasonForVisit) {
            $comments = 'Reason for visit: ' . $form->patientReasonForVisit . ', Additional Comments: ' . $form->additionalReasonComment . "\n";
        }

        if ($user->is_video_visit_flow) {
            $comments .= "Patient Acknowledged - Illinois location during visit and receipt of Notice of Privacy Practices";
        }

        switch ($action) {
            case 'find':
                return $this->findPatient($dob, $gender, $firstName, $lastName, $email, $phone, $street, $streetLine2, $state, $postalCode, $city);
            case 'create':
                return $this->createPatient($dob, $gender, $firstName, $lastName, $email, $phone, $departmentID, $departmentIDType, $street, $streetLine2, $state, $postalCode, $city);
            case 'book':
                return $this->scheduleAppointment($patientId, $patientIDType, $departmentID, $departmentIDType, $visitTypeID, $visitTypeIDType, $appointmentDate, $providerIdAndType['providerID'], $providerIdAndType['providerIDType'], $insuranceMemberId, $insuranceGroupId, $insuranceProviderName, $comments);
        }

        return null;
    }

    public function parsePatientObject(array $response)
    {
        $endResult = [];
        $addressObject = [];
        $firstName = null;
        $lastName = null;
        $gender = null;

        if (isset($response['NameComponents']['FirstName'])) {
            $firstName = $response['NameComponents']['FirstName'];
        }

        if (isset($response['NameComponents']['LastName'])) {
            $lastName = $response['NameComponents']['LastName'];
        }

        if (isset($response['Gender'])) {
            $gender = $response['Gender'];
        }

        $patientObject = [
            'firstName' => $firstName,
            'lastName' => $lastName,
            'gender' => $gender,
        ];

        foreach ($response['Addresses'] as $address) {
            if ($address['Type'] == "Permanent") {
                $streetAddress = null;
                $city = null;
                $state = null;
                $zipcode = null;
                $email = null;
                $phone = null;

                foreach ($address['Phones'] as $phoneObject) {
                    if (!empty($phoneObject['Number'])) {
                        $phone = $phoneObject['Number'];
                        break;
                    }
                }

                if (isset($address['Street'])) {
                    $streetAddress = $address['Street'];
                }

                if (isset($address['City'])) {
                    $city = $address['City'];
                }

                if (isset($address['State'])) {
                    $state = DupageCoreModule::getInstance()->dupageCoreModuleService->convertState($address['State']);
                }

                if (isset($address['Zip'])) {
                    $zipcode = $address['Zip'];
                }

                if (isset($address['Email'])) {
                    $email = $address['Email'];
                }

                if ($phone) {
                    // need specific format to pass front end form validation
                    // https://stackoverflow.com/a/10741461/3007294
                    $phone = preg_replace('~.*(\d{3})[^\d]{0,7}(\d{3})[^\d]{0,7}(\d{4}).*~', '($1) $2-$3', $phone);
                }

                $addressObject =
                    [
                        'address' => $streetAddress,
                        'city' => $city,
                        'state' => $state,
                        'zipcode' => $zipcode,
                        'email' => $email,
                        'phone' => $phone
                    ];
            }
        }

        $endResult = \array_merge($addressObject, $patientObject);

        return $endResult;
    }

    /**
     * Performs an authenticate request to iPatientAccessMobileServices_2013 with the provided username and password
     *
     * @param string $username
     * @param string $password
     *
     * @return array|null
     * @throws SoapClientException
     */
    public function authenticate(string $username, string $password): ?array
    {
        $response = $this->callSoapService(
            'AuthenticatedWebAccountRequest.twig',
            '/wcf/Epic.PatientAccessMobile.Services/PatientAccessMobile.svc/basic_2013',
            "urn:epicsystems-com:PatientAccessMobile.2013.Services.AuthenticateWebAccount",
            [
                'username' => $username,
                'password' => $password
            ]
        );

        if ($response->getHeaders()->get('http-code') != 200) {
            throw new SoapClientException;
        }

        $xml = new SimpleXMLElement($response->getContent());
        $result = $xml->children('s', true)->Body->children()->AuthenticateWebAccountResponse->AuthenticateWebAccountResult;

        if ($result->Status == 1) {
            return [
                'isAdmitted' => (bool)$result->WebUser->isAdmitted,
                'name' => (string)$result->WebUser->Name,
                'isNonPatient' => (string)$result->WebUser->IsNonPatient,
                'epi' => $this->getPatientIdentifiers(
                    (string)$result->WebUser->WebAccountID,
                    'WPRINTERNAL'
                )
            ];
        }

        return null;
    }

    /**
     * Returns an array of Proxy EPI's
     *  - An empty array will be returned for patients without proxies
     *  - Proxies without a discernable EPI will not be returned for scheduling
     *  - Only the EPI's are returned as Epic returns the GEC record instead of the EPI.
     *
     * @param string $patientId
     * @param string $patientIdType
     * @return array
     */
    public function getProxyInformation(string $patientId, string $patientIdType = 'EPI'): array
    {
        $response = $this->callSoapService(
            'GetProxyInformation.twig',
            '/wcf/Epic.PatientAccess/PatientAccess.svc/basic',
            "urn:epicsystems-com:PatientAccess.2009.Services.GetProxyInformation",
            [
                'patientId' => $patientId,
                'patientIdType' => $patientIdType
            ]
        );

        if ($response->getHeaders()->get('http-code') != 200) {
            throw new SoapClientException;
        }

        $xml = new SimpleXMLElement($response->getContent());
        $result = $xml->children('s', true)->Body->children()->GetProxyInformationResponse->GetProxyInformationResult;

        static $patientIds = [];
        foreach ($result->AccessList->Subject as $k => $list) {
            if (!empty($list->PatientID->ID)) {
                $patientId = $this->getPatientIdentifiers($list->PatientID->ID, $list->PatientID->IDType);
                if ($patientId != null) {
                    $patientIds[] = $patientId;
                }
            }
        }

        return $patientIds;
    }

    /**
     * Calls the specified SOAP Service manually with cURL
     *
     * @param string $requestBodyFile   Twig file to generate the request
     * @param string $url               The specific endpoint to call
     * @param string $soapAction
     * @param array $options
     *
     * return \yii\httpclient\Response
     */
    private function callSoapService(string $requestBodyFile, string $url, string $soapAction, array $options): \yii\httpclient\Response
    {
        $view = Craft::$app->getView();
        $templateMode = $view->getTemplateMode();
        $view->setTemplateMode($view::TEMPLATE_MODE_SITE);
        $view->setTemplatesPath(Craft::getAlias('@modules/schedulingmodule/templates/ws'));

        $requestBody = $view->renderTemplate($requestBodyFile, \array_merge($options, [
            'epicDate' => date('c'),
            'epicUsername' => \str_replace('$', ':', getenv('EPIC_REST_USERNAME')),
            'epicPassword' => getenv('EPIC_REST_PASSWORD'),
            'epicNonce' => \base64_encode(\random_bytes(16)),
            'epicClientId' => getenv('EPIC_CLIENT_ID')
        ]));

        // Reset the template path
        $view->setTemplatesPath(CRAFT_BASE_PATH . DIRECTORY_SEPARATOR . 'templates');

        $client = Craft::$app->httpclient->getClient();
        $request = $this->generateHttpRequest(
            $client,
            $this->getRootUrl() . $url,
            'POST',
            YiiHttpClient::FORMAT_XML,
            [
                'Content-Type' => 'text/xml; charset=utf-8',
                'Accept' => 'text/xml; charset=utf-8',
                'SOAPAction' => $soapAction
            ],
            $requestBody
        );

        $response = $request->send();

        if ($response->getHeaders()->get('http-code') != 200) {
            // If we get a non 200 back, log the url and action for analysis later
            Craft::error(new PsrMessage('SOAP Called Failed', [
                'url' => $this->getRootUrl() . $url,
                'action' => $soapAction,
                'headers' => $response->getHeaders()->toArray(),
            ]), get_class($this) . '::' . __METHOD__);
        }

        return $response;
    }

    /**
     * Generates a Yii2 HttpRequest
     *
     * @param HttpClient $client
     * @param string $url
     * @param string $method
     * @param string $format
     * @param array $headers
     * @param string|array|n ull $content
     */
    private function generateHttpRequest(
        $client,
        string $url,
        string $method,
        string $format,
        array $headers = [],
        $content = null
    ) {
        $request = $client->createRequest()
            ->setFormat($format)
            ->setMethod($method)
            ->setOptions([
                'sslVerifyPeer' => getenv('ENVIRONMENT') != 'dev',
                CURLOPT_DNS_USE_GLOBAL_CACHE => false,
                CURLOPT_SSL_VERIFYHOST => getenv('ENVIRONMENT') != 'dev' ? 2 : 0,
                CURLOPT_SSL_VERIFYPEER => getenv('ENVIRONMENT') != 'dev',
                CURLOPT_RESOLVE => [
                    'dupagemedicalgroup.com:443:127.0.0.1',
                    'www.dupagemedicalgroup.com:443:127.0.0.1',
                    'dulyhealthandcare.com:443:127.0.0.1',
                    'www.dulyhealthandcare.com:443:127.0.0.1',
                    'npd.dupagemedicalgroup.com:443:127.0.0.1'
                ]
            ])
            ->addHeaders($headers)
            ->setUrl($url);

        if ($content !== null) {
            if (\is_array($content)) {
                $request->setData($content);
            } else {
                $request->setContent($content);
            }
        }

        return $request;
    }

    /**
     * Returns the correct proxy domain for Epic connectivity
     * @return string
     */
    private function getDomain()
    {
        if (getenv('ENVIRONMENT') === '6aac88b258202c4b1774c8362ca3be63.t73.pkiapps.com') {
            return getenv('DEFAULT_SITE_URL');
        }

        $domain = \str_replace([':8443', ':8444'], '', getenv('DEFAULT_SITE_URL'));
        return $domain;
    }

    /**
     * Returns the root domain + url with the interconnect instance route.
     * @return string
     */
    private function getRootUrl()
    {
        // pre-4/9/22, aka using shared EPIC instance, backwards-compatibility
        if (getenv('EPIC_INSTANCE') === false) {
            return $this->getDomain() . '/epic/' . getenv('INTERCONNECT_INSTANCE');
        } else {
            // post-4/9/22, using the new Duly-only instance
            if (getenv('EPIC_INSTANCE') === "prod") {
                return $this->getDomain() . '/epic/prod/wipfli';
            } elseif (getenv('EPIC_INSTANCE') === "npd") {
                return $this->getDomain() . '/epic/npd/wipfli';
            }
        }
    }
}
