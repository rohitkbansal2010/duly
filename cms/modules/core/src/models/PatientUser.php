<?php

namespace modules\DupageCoreModule\models;

use Craft;
use craft\elements\Entry;
use DateTime;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\base\Model;
use yii\db\ActiveRecord;
use yii\filters\RateLimitInterface;
use yii\web\IdentityInterface;

class PatientUser extends Model implements IdentityInterface, RateLimitInterface
{
    const SCENARIO_ANONYMOUS = 'anonymous';

    const SCENARIO_DEFAULT = 'default';

    public $anonymous;
    public $apointment_time_selected_outside_of_scheduling;
    public $appointment_current_epi;
    public $appointment_department_id;
    public $appointment_established_patient_visit;
    public $appointment_follow_up_visit;
    public $appointment_hospital_follow_up_visit;
    public $appointment_vein_clinic_visit;
    public $appointment_full_body_skin_exam_visit;
    public $appointment_insurance_plan_id;
    public $appointment_insurance_plan_medicare_advantage;
    public $appointment_insurance_provider_id;
    public $appointment_new_patient_visit;
    public $appointment_physician_id;
    public $appointment_reason_for_visit_id;
    public $appointment_selected_recommended_physician_id;
    public $appointment_rule_out_question_answer;
    public $appointment_service_ids;
    public $appointment_time;
    public $appointment_visit_type_id;
    public $category_service_ids;
    public $date_of_birth_day;
    public $date_of_birth_month;
    public $date_of_birth_year;
    public $eligible_physicians;
    public $id;
    public $location_id;
    public $location_selected_outside_of_scheduling;
    public $main_patient_epi;
    public $name;
    public $patient_epis_last_updated;
    public $patient_epis;
    public $physician_selected_outside_of_scheduling;
    public $proxy_epis;
    public $deeplinked_session;
    public $is_video_visit_flow;
    public $chosen_video_visit_group;
    public $conversation_id;
    public $is_adult;
    public $is_in_illinois;
    public $privacy_agree;

    /**
     * The rate limit
     * @var integer
     */
    private $rateLimit = 100;

    /**
     * The rate limit window
     * @var integer
     */
    private $rateLimitWindow = 1800;

    /**
     * @return PatientUser|null
     */
    public static function anonymousUser(): ?self
    {
        $model = new self(['scenario' => self::SCENARIO_ANONYMOUS]);
        $model->id = \bin2hex(\random_bytes(32));
        $model->anonymous = true;

        if ($model->save()) {
            return $model;
        }

        return null;
    }

    /**
     * @inheritdoc
     */
    public function scenarios()
    {
        return [
            self::SCENARIO_ANONYMOUS => $this->attributes(),
            self::SCENARIO_DEFAULT => $this->attributes()
        ];
    }

    /**
     * @inheritdoc
     */
    public function load($data, $formName = null)
    {
        $scope = $formName === null ? $this->formName() : $formName;

        parent::load($data, $formName);

        if (isset($data[$scope]['appointmentServiceIds'])) {
            $this->setAppointmentServiceIds($data[$scope]['appointmentServiceIds']);
        }

        if (isset($data[$scope]['appointment_physician_id'])) {
            // if we are setting a physician directly,
            // we can extract their schedulable services and store that as well
            $physician = Entry::find()
                ->id($data[$scope]['appointment_physician_id'])
                ->one();
            if ($physician && count($this->getAppointmentServiceIds()) == 0) {
                $services = SchedulingModule::getInstance()
                    ->schedulingModuleService
                    ->getSchedulableServicesForPhysician($physician);

                $serviceIds = \array_values(\array_map(
                    function ($service) {
                        return $service->id;
                    },
                    $services
                ));

                $this->setAppointmentServiceIds($serviceIds);
            }
        }

        return true;
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            ['id', 'required'],
            [['anonymous', 'appointment_insurance_plan_medicare_advantage', 'is_video_visit_flow', 'is_adult', 'is_in_illinois', 'privacy_agree'], 'boolean'],
            [['id', 'name', 'appointment_insurance_provider_id', 'appointment_insurance_plan_id', 'appointment_rule_out_question_answer', 'appointment_department_id', 'appointment_current_epi', 'appointment_visit_type_id', 'physician_selected_outside_of_scheduling', 'apointment_time_selected_outside_of_scheduling', 'location_id', 'location_selected_outside_of_scheduling', 'appointment_physician_id', 'chosen_video_visit_group'], 'string'],
            [['date_of_birth_month', 'date_of_birth_day', 'date_of_birth_year', 'appointment_reason_for_visit_id', 'appointment_selected_recommended_physician_id'], 'number'],
            ['appointment_time', 'date', 'format' => 'php:Y-m-d\TH:i:sO'],
            ['appointment_physician_id', 'validatePhysicianId'],
            ['appointment_insurance_provider_id', 'appointmentInsuranceProviderId'],
            ['appointment_insurance_plan_id', 'appointmentInsurancePlanId'],
            ['appointment_reason_for_visit_id', 'appointmentReasonForVisitId'],
            ['appointment_rule_out_question_answer', 'appointmentRuleOutQuestionAnswer'],
            [['id', 'name', 'patient_epis', 'date_of_birth_month', 'appointment_service_ids', 'category_service_ids', 'date_of_birth_day', 'date_of_birth_year', 'appointment_insurance_provider_id', 'appointment_insurance_plan_id', 'appointment_rule_out_question_answer', 'appointment_reason_for_visit_id', 'appointment_physician_id', 'appointment_time', 'appointment_department_id', 'appointment_current_epi', 'anonymous', 'appointment_current_epi', 'appointment_visit_type_id', 'appointment_visit_type_id', 'appointment_follow_up_visit', 'appointment_hospital_follow_up_visit', 'appointment_vein_clinic_visit', 'appointment_full_body_skin_exam_visit', 'physician_selected_outside_of_scheduling', 'apointment_time_selected_outside_of_scheduling', 'location_id', 'location_selected_outside_of_scheduling', 'appointment_selected_recommended_physician_id', 'deeplinked_session', 'conversation_id', 'is_video_visit_flow', 'chosen_video_visit_group'], 'safe']
        ];
    }

    /**
     * @inheritdoc
     *
     * Update current session with new values.
     */
    public function save()
    {
        if (!parent::validate()) {
            return false;
        }

        Craft::$app->session->set('user', \igbinary_serialize($this));
        return true;
    }

    /**
     * Resets user data by clearing out the values and forcing a database refresh in Redis
     * @return void
     */
    public function resetUserData()
    {
        static $attributes = [];
        $this->removeSessionData();

        if ($this->anonymous) {
            $attributes = $this->getSafeAnonymousAttributes();
        } else {
            $attributes = $this->getSafeAuthenticatedAttributes();
        }

        foreach ($this->attributes as $attribute => $param) {
            if (!\in_array($attribute, $attributes)) {
                $this->$attribute = null;
            }
        }

        $this->save();
        Craft::info("Cleared session data for " . $this->id, get_class($this) . '::' . __METHOD__);
    }

    /**
     * Safe attributes tbat should remain after a reset for unauthenticated users
     * @return array
     */
    private function getSafeAnonymousAttributes()
    {
        return [
            'id',
            'anonymous',
            'patient_epis_last_updated'
        ];
    }

    /**
     * Removes any sensitive session data that may be stored throughout the scheduling process
     */
    private function removeSessionData()
    {
        $session = Craft::$app->session;
        $session->remove('patient_is_pregnant');
        $session->remove('last_menstrual_cycle_day');
        $session->remove('last_menstrual_cycle_month');
        $session->remove('last_menstrual_cycle_year');
        $session->remove('external_provider_resource_id');
    }

    /**
     * Safe attributes that should remain after a reset for authenticated users
     * @return array
     */
    private function getSafeAuthenticatedAttributes()
    {
        return [
            'id',
            'anonymous',
            'appointment_current_epi',
            'name',
            'external_patient_id_type',
            'main_patient_epi',
            'proxy_epis',
            'patient_epis_last_updated'
        ];
    }

    /**
     * @inheritdoc
     */
    public function getRateLimit($request, $action)
    {
        return [
            $this->rateLimit,
            $this->rateLimitWindow
        ];
    }

    /**
     * @inheritdoc
     */
    public function loadAllowance($request, $action)
    {
        $hash = $this->id . $request->getUrl() . $action->id;
        $allowance = Craft::$app->cache->get($hash);
        if ($allowance === false) {
            return [
                $this->rateLimit,
                time()
            ];
        }
        return $allowance;
    }

    /**
     * @inheritdoc
     */
    public function saveAllowance($request, $action, $allowance, $timestamp)
    {
        $hash = $this->id . $request->getUrl() . $action->id;
        $allowance = [
            $allowance,
            $timestamp
        ];
        Craft::$app->cache->set($hash, $allowance, $this->rateLimitWindow);
    }

    /**
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function appointmentInsuranceProviderId($attribute, $param)
    {
        if (!Entry::find()->section('insuranceProviders')->id($this->$attribute)->one() && $this->appointment_insurance_provider_id !== "self-pay" && $this->appointment_insurance_provider_id !== "no-provider") {
            $this->addError($attribute, 'Please select a valid insurance provider.');
            return false;
        }

        return true;
    }

    /**
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function appointmentInsurancePlanId($attribute, $param)
    {
        // -1 is user doesn't have a plan so this validation should be skipped
        if ($this->$attribute != '-1') {
            if (!Entry::find()->section('insurancePlans')->id($this->$attribute)->one() && $this->appointment_insurance_provider_id !== "self-pay" && $this->appointment_insurance_provider_id !== "no-provider") {
                $this->addError($attribute, 'Please select a valid insurance plan.');
                return false;
            }
        }

        return true;
    }

    /**
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function appointmentReasonForVisitId($attribute, $param)
    {
        if (
            !Entry::find()->section('serviceReasonsForVisit')->id($this->$attribute)->one()
            && $this->$attribute !== "other"
        ) {
            $this->addError($attribute, 'Please select a valid reason for visit.');
            return false;
        }

        return true;
    }

    /**
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function appointmentRuleOutQuestionAnswer($attribute, $param)
    {
        if ($this->$attribute !== "no") {
            $this->addError($attribute, 'Your condition might require an emergency appointment.');
            return false;
        }

        return true;
    }

    /**
     * Validates service
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateServices($attribute, $param)
    {
        $result = Entry::find()
            ->section('services')
            ->id($param)
            ->one();

        if ($result === null) {
            $this->addError('service', Craft::t('scheduling-module', 'The service ID provided is not valid.'));
            return false;
        }

        return true;
    }

    /**
     * Validates physician
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validatePhysicianId($attribute, $param)
    {
        $result = Entry::find()
            ->section('physicians')
            ->id($param)
            ->one();

        if ($result === null) {
            $this->addError('physicianId', Craft::t('physicians-module', 'The Physician ID provided is not valid.'));
            return false;
        }

        return true;
    }

    /**
     * {@inheritdoc}
     */
    public static function findIdentity($id)
    {
        $sessionUser = \igbinary_unserialize(Craft::$app->session->get('user'));

        if (is_a($sessionUser, 'modules\DupageCoreModule\models\PatientUser')) {
            return $sessionUser;
        }

        return null;
    }

    /**
     * {@inheritdoc}
     */
    public static function findIdentityByAccessToken($token, $type = null)
    {
    }

    /**
     * {@inheritdoc}
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * {@inheritdoc}
     */
    public function getAuthKey()
    {
        return $this->id;
    }

    /**
     * {@inheritdoc}
     */
    public function validateAuthKey($authKey)
    {
        return $this->authKey === $authKey;
    }

    /**
     * Retrns the patient date of birth
     *
     * @return DateTime|null
     */
    public function getDob(): ?DateTime
    {
        if (isset($this->appointment_current_epi) && $this->appointment_current_epi != "-1") {
            $patients = $this->getPatients();

            if (isset($patients[$this->appointment_current_epi])) {
                return DateTime::createFromFormat('Y-m-d', $patients[$this->appointment_current_epi]['details']['DOB']);
            }
        }

        $dob = DateTime::createFromFormat('Y-m-d', $this->date_of_birth_year . "-" . $this->date_of_birth_month . "-" . $this->date_of_birth_day);

        if ($dob) {
            return $dob;
        } else {
            Craft::error(new PsrMessage('Invalid Date Of Birth', [
                'date_of_birth_year' => $this->date_of_birth_year,
                'date_of_birth_month' => $this->date_of_birth_month,
                'date_of_birth_day' => $this->date_of_birth_day,
                'appointment_current_epi' => $this->appointment_current_epi
            ]), 'dupagecoremodules/models/PatientUser::getDob');

            return null;
        }
    }

    /**
     * Retrns user's age
     *
     * @return int
     */
    public function getUserAge(): int
    {
        $dob = $this->getDob();

        if ($dob) {
            return date_diff($dob, date_create('now'))->y;
        }

        if (isset($this->is_adult)) {
            return $this->is_adult ? 18 : 0;
        }

        return -1;
    }

    /**
     * Returns patient EPIs manages by this patient.
     * @return array
     */
    private function getPatientEpis()
    {
        $now = new \DateTime;
        $lastUpdated = DateTime::createFromFormat('U', $this->patient_epis_last_updated ?? \time());

        if ($this->main_patient_epi === null) {
            $this->main_patient_epi = SchedulingModule::getInstance()->schedulingModuleEpicService->getPatientIdentifiers($this->id, "EPI");
        }

        $patientEpis = $this->main_patient_epi ? [$this->main_patient_epi] : [];

        if (empty($this->getProxyEpis()) || $this->getProxyEpis() === "null" || $now->diff($lastUpdated)->h > 1) {
            Craft::info('Fetching proxy EPIs for ' . $this->main_patient_epi);
            $this->setProxyEpis(SchedulingModule::getInstance()->schedulingModuleEpicService->getProxyInformation($this->id));
            $this->patient_epis_last_updated = \time();
            $this->save();
        }

        $this->setProxyEpis((function () {
            if (\is_array($this->proxyEpis)) {
                return $this->proxyEpis;
            }

            $d = \json_decode($this->proxyEpis);
            if (\json_last_error() === JSON_ERROR_NONE) {
                return $d;
            }

            return $this->proxyEpis ?? [];
        })());

        return \array_merge($patientEpis, $this->getProxyEpis());
    }

    /**
     * Returns patients managed by this main patient account.
     * Includes patient EPIs, patient names, as well as their past and future appointments
     */
    public function getPatients()
    {
        // anonymous (non-mychart) users are not eligible for this
        if ($this->anonymous || $this->scenario == self::SCENARIO_ANONYMOUS) {
            return [];
        }

        $patients = [];

        $epis = $this->getPatientEpis();

        foreach ($epis as $epi) {
            $patients[$epi] = [];

            // cache for 15min
            $patients[$epi]['details'] = Craft::$app->cache->getOrSet($this->id . '_' . $epi . '_details', function () use ($epi) {
                return SchedulingModule::getInstance()->schedulingModuleEpicService->findPatientByID($epi, "EPI");
            }, 900);

            // cache for 15min
            $patients[$epi]['pastAppointmentsPhysicianIDs'] = Craft::$app->cache->getOrSet($this->id . '_' . $epi . '_previously_seen_physicians', function () use ($epi) {
                return SchedulingModule::getInstance()->schedulingModuleEpicService->getPreviouslySeenProviders($epi);
            }, 900);

            // cache for 15min
            $patients[$epi]['nextUpcomingAppointment'] = Craft::$app->cache->getOrSet($this->id . '_' . $epi . '_next_upcoming_appointment', function () use ($epi) {
                return SchedulingModule::getInstance()->schedulingModuleEpicService->getNextUpcomingAppointment($epi);
            }, 900);
        }

        if (count($epis) > 0) {
            // select the first patient as the "active" patient
            // this can be changed with the main scheduling page patient picker
            foreach ($patients[array_keys($patients)[0]]['details']['IDs'] as $id) {
                if ($id['Type'] == 'EPI') {
                    $this->appointment_current_epi = $this->appointment_current_epi ?? $id['ID'];
                    break;
                }
            }
        } else {
            $this->appointment_current_epi = "-1";
        }

        $this->save();

        return $patients;
    }

    public function resetCachedNextUpcomingAppointment()
    {
        Craft::$app->cache->delete($this->id . '_' . $this->appointment_current_epi . '_next_upcoming_appointment');
    }

    /**
     * Get the value of proxyEpis
     */
    public function getProxyEpis(): array
    {
        return \json_decode($this->proxy_epis, true) ?? [];
    }

    /**
     * Set the value of proxyEpis
     *
     * @return  self
     */
    public function setProxyEpis(array $proxyEpis)
    {
        $this->proxy_epis = \json_encode($proxyEpis);

        return $this;
    }

    /**
     * Get selected service IDs
     *
     * @return  string[]
     */
    public function getAppointmentServiceIds(): array
    {
        if ($this->appointment_service_ids) {
            return \json_decode($this->appointment_service_ids, true);
        }

        return [];
    }

    /**
     * Set the value of appointmentServiceIds
     *
     * @return  self
     */
    public function setAppointmentServiceIds(array $appointmentServiceIds)
    {
        $this->appointment_service_ids = \json_encode($appointmentServiceIds);

        return $this;
    }

    /**
     * Determines if the current scheduling flow will happen without selecting specific physician.
     * This is true for scheduling flows for special services that specify a special external provider resource id (per suite).
     *
     * @return  bool
     */
    public function isSchedulingFlowWithoutPhysicians()
    {
        if ($this->is_video_visit_flow) {
            return true;
        }

        $service = Entry::find()
            ->section('services')
            ->id($this->getAppointmentServiceIds())
            ->one();

        if (!$service) {
            return false;
        }

        return $service->schedulingWithoutPhysicians;
    }

    /**
     * If chosen location ID and chosen service ID are configured for a location suite,
     * this function returns this suite's external provider resouce ID (used for scheduling without physicians).
     *
     * @return  string|null
     */
    public function findExternalProviderResourceIdForChosenServiceEntry()
    {
        $suites = Entry::find()
            ->section('locations')
            ->id($this->location_id)
            ->one()
            ->children
            ->with('suiteServices')
            ->all();

        foreach ($suites as $suite) {
            foreach ($suite->suiteServices as $suiteService) {
                $suiteServiceId = $suiteService->serviceType->ids()[0] ?? null;
                if ($suiteServiceId == $this->getAppointmentServiceIds()[0]) {
                    return $suiteService->externalProviderResourceId;
                }
            }
        }

        return null;
    }

    /**
     * This method returns the ID of a physician that is ready to be booked.
     * This only applies to the last step of the scheduling process.
     * When a user is selecting a time for the appointment, they may be given a list of recommended physicians.
     * If they select a recommended physician, this is the physician whose ID should be used for booking.
     * At the same time, we don't want to override the original chosen physician ID in case the user wants to go back
     * in the flow and choose a different recomended physician, or choose the original chosen physician.
     */
    public function getPhysicianIdReadyToBook()
    {
        if (!empty($this->appointment_selected_recommended_physician_id)) {
            return $this->appointment_selected_recommended_physician_id;
        } else {
            return $this->appointment_physician_id;
        }
    }

    /**
     * This method translates the selected insurance provider ID into a valid string insruance carrier name.
     *
     * @return string
     */
    public function getInsuranceProviderName()
    {
        if ($this->appointment_insurance_provider_id === 'no-provider') {
            return "N/A";
        } elseif ($this->appointment_insurance_provider_id === 'self-pay') {
            return "N/A";
        } else {
            $insuranceProvider = Entry::find()
                ->section('insuranceProviders')
                ->id($this->appointment_insurance_provider_id)
                ->one();

            return $insuranceProvider !== null ? $insuranceProvider->title : 'N/A';
        }
    }
}
