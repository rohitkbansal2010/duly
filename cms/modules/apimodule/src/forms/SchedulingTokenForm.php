<?php

namespace modules\apimodule\forms;

use Craft;
use craft\elements\Category;
use craft\elements\Entry;
use DateTime;
use DateInterval;
use yii\base\Model;
use yii\data\ActiveDataProvider;
use yii\web\UrlManager;

use modules\apimodule\ApiModule;

/**
 * Hopspital Form
 *
 * This form wraps the logic for generating a scheduling token used to deep-link users directly
 * into the /schedule/select-appointment page.
 *
 * This form is tailored for use in /api endpoints.
 */
final class SchedulingTokenForm extends Model
{
    const PHYSICIAN_SCHEDULING_SCENARIO = 'PHYSICIAN_SCHEDULING_SCENARIO';
    const LOCATION_SCHEDULING_SCENARIO = 'LOCATION_SCHEDULING_SCENARIO';
    const TELEMEDICINE_SCHEDULING_SCENARIO = 'TELEMEDICINE_SCHEDULING_SCENARIO';
    const TOKEN_EXPIRATION_TIME = 60 * 30; // 30 minutes
    const TOKEN_PREFIX = "scheduling_token_";

    /**
     * @var string $location_id
     */
    public $location_id;

    /**
     * @var string $physician_id
     */
    public $physician_id;

    /**
     * @var string $telemedicine_id
     */
    public $telemedicine_id;

    /**
     * @var string $service_id
     */
    public $service_id;

    /**
     * @var string $is_new_patient
     */
    public $is_new_patient;

    /**
     * @var string $conversation_id
     */
    public $conversation_id;

    /**
     * @var Entry $serviceEntry
     */
    private $serviceEntry = null;

    /**
     * @var Entry $physicianEntry
     */
    private $physicianEntry = null;

    /**
     * @var Entry $locationEntry
     */
    private $locationEntry = null;

    /**
     * @inheritdoc
     */
    public function load($data, $formName = null)
    {
        if (!parent::load($data, $formName)) {
            return false;
        }

        if ($this->service_id !== null) {
            $service = Entry::find()->id($this->service_id)->one();
            if ($service) {
                $this->setServiceEntry($service);
            }
        }

        if ($this->physician_id !== null) {
            $this->setScenario(self::PHYSICIAN_SCHEDULING_SCENARIO);
            $physician = Entry::find()->id($this->physician_id)->one();
            if ($physician) {
                $this->setPhysicianEntry($physician);
                return true;
            }
        } elseif ($this->location_id !== null) {
            $this->setScenario(self::LOCATION_SCHEDULING_SCENARIO);
            $location = Entry::find()->id($this->location_id)->one();
            if ($location) {
                $this->setLocationEntry($location);
                return true;
            }
        } elseif ($this->telemedicine_id !== null) {
            $this->setScenario(self::TELEMEDICINE_SCHEDULING_SCENARIO);
            $location = Entry::find()->id($this->telemedicine_id)->one();
            if ($location) {
                $this->setLocationEntry($location);
                return true;
            }
        }

        return false;
    }

    /**
     * @inheritdoc
     */
    public function scenarios()
    {
        $scenarios = parent::scenarios();
        $scenarios[self::LOCATION_SCHEDULING_SCENARIO] = ['location_id', 'service_id', 'is_new_patient', 'conversation_id'];
        $scenarios[self::PHYSICIAN_SCHEDULING_SCENARIO] = ['physician_id', 'service_id', 'is_new_patient', 'conversation_id'];
        $scenarios[self::PHYSICIAN_SCHEDULING_SCENARIO] = ['telemedicine_id', 'service_id', 'is_new_patient', 'conversation_id'];
        return $scenarios;
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['physician_id'], 'required', 'on' => self::PHYSICIAN_SCHEDULING_SCENARIO],
            [['location_id'], 'required', 'on' => self::LOCATION_SCHEDULING_SCENARIO],
            [['telemedicine_id'], 'required', 'on' => self::TELEMEDICINE_SCHEDULING_SCENARIO],
            [['service_id'], 'validateServiceId'],
            [['service_id', 'is_new_patient', 'conversation_id'], 'required'],
            [['location_id', 'physician_id', 'telemedicine_id', 'service_id', 'is_new_patient', 'conversation_id'], 'safe']
        ];
    }

    /**
     * Validates the provided service id.
     *
     * @return boolean
     */
    public function validateServiceId()
    {
        if (!$this->getServiceEntry()) {
            $this->addError('service_id', 'Invalid service id.');
            return false;
        }

        return true;
    }
    
    /**
     * This method returns a token used to deep-link into /schedule/select-appointment?token=<token>
     *
     * @return array
     */
    public function generateToken()
    {
        if (!$this->validate()) {
            return [];
        }

        // this token will be consumable by /schedule/select-appointment 
        // and effectively act as a way to deep-link a user into scheduling
        $tokenName = bin2hex(random_bytes(16));

        // this data will be available to the /schedule/select-appointment page during a deep-link process
        $storedData = [
            'service_id' => $this->service_id,
            'is_new_patient' => $this->is_new_patient,
            'conversation_id' => $this->conversation_id,
        ];

        if ($this->scenario === self::LOCATION_SCHEDULING_SCENARIO) {
            $storedData['location_id'] = $this->location_id;
        } elseif ($this->scenario === self::PHYSICIAN_SCHEDULING_SCENARIO) {
            $storedData['physician_id'] = $this->physician_id;
        } elseif ($this->scenario === self::TELEMEDICINE_SCHEDULING_SCENARIO) {
            $storedData['telemedicine_id'] = $this->telemedicine_id;
        }

        $tokenExpirationDate = (new DateTime())->add(new DateInterval("PT" . self::TOKEN_EXPIRATION_TIME . "S"));

        Craft::$app->cache->set(
            self::TOKEN_PREFIX . $tokenName,
            $storedData,
            self::TOKEN_EXPIRATION_TIME
        );

        return [
            'token' => $tokenName,
            'expiresAt' => $tokenExpirationDate->format('c')
        ];
    }

    /**
     * Get $serviceEntry
     *
     * @return  Entry
     */
    public function getServiceEntry()
    {
        return $this->serviceEntry;
    }

    /**
     * Set $serviceEntry
     *
     * @param  Entry  $serviceEntry  $serviceEntry
     *
     * @return  self
     */
    public function setServiceEntry(Entry $serviceEntry)
    {
        $this->serviceEntry = $serviceEntry;

        return $this;
    }

    /**
     * Get $physicianEntry
     *
     * @return  Entry
     */ 
    public function getPhysicianEntry()
    {
        return $this->physicianEntry;
    }

    /**
     * Set $physicianEntry
     *
     * @param  Entry  $physicianEntry  $physicianEntry
     *
     * @return  self
     */ 
    public function setPhysicianEntry(Entry $physicianEntry)
    {
        $this->physicianEntry = $physicianEntry;

        return $this;
    }

    /**
     * Get $locationEntry
     *
     * @return  Entry
     */ 
    public function getLocationEntry()
    {
        return $this->locationEntry;
    }

    /**
     * Set $locationEntry
     *
     * @param  Entry  $locationEntry  $locationEntry
     *
     * @return  self
     */ 
    public function setLocationEntry(Entry $locationEntry)
    {
        $this->locationEntry = $locationEntry;

        return $this;
    }
}
