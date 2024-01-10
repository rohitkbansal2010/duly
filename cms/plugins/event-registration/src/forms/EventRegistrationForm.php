<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\eventregistration\forms;

use Craft;
use craft\elements\MatrixBlock;
use DateTime;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\queue\EmailJob;
use punchkick\eventregistration\EventRegistration;
use punchkick\eventregistration\EventRegistrationPlugin;
use punchkick\eventregistration\records\EventRegistrationRecord;
use Solspace\Calendar\Calendar;
use Solspace\Calendar\Elements\Event;
use yii\base\Model;

/**
 * EventRegistrationPlugin Form
 *
 * This form wraps the main event registration form and abstracts
 * away database functionality from the UI
 *
 * @author    Punchkick Interactive
 * @package   EventRegistrationPlugin
 * @since     0.0.1
 */
final class EventRegistrationForm extends Model
{
    /**
     * @var integer $eventId
     */
    public $eventId;

    /**
     * @var integer $locationId
     */
    public $locationId;

    /**
     * @var string $occurrenceDate
     */
    public $occurrenceDate;

    /**
     * @var string $firstName
     */
    public $firstName;

    /**
     * @var string $lastName
     */
    public $lastName;

    /**
     * @var string $phoneNumber
     */
    public $phoneNumber;

    /**
     * @var string $emailAddress
     */
    public $emailAddress;

    /**
     * @var boolean $marketingOptin
     */
    public $marketingOptin = false;

    /**
     * @var Event $_event
     */
    private $_event;

    /**
     * @var Matrix $_location
     */
    private $_location;

    /**
     * @var string $recaptchaToken
     */
    public $recaptchaToken;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['eventId', 'locationId', 'occurrenceDate', 'firstName', 'lastName', 'phoneNumber', 'emailAddress', 'marketingOptin'], 'required'],
            [['eventId', 'locationId'], 'integer'],
            [['firstName', 'lastName', 'recaptchaToken'], 'string'],
            [['occurrenceDate'], 'date', 'format' => 'php:Y-m-d'],
            [['emailAddress'], 'email'],
            [['phoneNumber'], 'string'],
            [['marketingOptin'], 'default', 'value' => 0],
            [['marketingOptin'], 'boolean'],
            [['eventId'], 'validateEvent'],
            ['recaptchaToken', 'required', 'message' => 'Something went wrong. Please try again.'],
            ['recaptchaToken', 'validateRecaptchaToken'],
            [['eventId', 'locationId', 'occurrenceDate', 'firstName', 'lastName', 'phoneNumber', 'emailAddress', 'marketingOptin', 'recaptchaToken'], 'safe']
        ];
    }

    /**
     * Validates reCAPTCHA token
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateRecaptchaToken($attribute, $param)
    {
        $verified = DupageCoreModule::getInstance()
                        ->dupageCoreModuleService
                        ->verifyRecaptchaToken($this->$attribute);

        if (!$verified) {
            $this->addError($attribute, 'Something went wrong. Please try again.');
        }
    }

    /**
     * Validates the event data
     *
     * @param string $param
     * @param mixed $attribute
     * @return boolean
     */
    public function validateEvent($param, $attribute)
    {
        // Validate the event is a valid Event instance
        $event = Calendar::getInstance()->events->getEventById($this->eventId);
        if ($event === null || !($event instanceof Event)) {
            $this->addError('eventId', Craft::t('event-registration', 'The event ID provided is not valid.'));
            return false;
        }

        // Occurrence dates get reset to the event start date for classes.
        if ($event->freq === "SELECT_DATES") {
            $this->occurrenceDate = $event->startDate->format('Y-m-d');
        }

        // Validate that there is a valid occurance on the selected data.
        $occurrence = DateTime::createFromFormat('Y-m-d', $this->occurrenceDate);
        $occurrence->setTime(0, 0, 0, 0);
        if (!$event->happensOn($occurrence)) {
            $this->addError('occurrenceDate', Craft::t('event-registration', 'The date provided for this event is not valid.'));
            return false;
        }

        if ($occurrence < (new DateTime)) {
            $this->addError('occurrenceDate', Craft::t('event-registration', 'You cannot register for a event that has already happened.'));
            return false;
        }

        $matrixLocation = null;
        foreach ($event->eventLocation->all() as $location) {
            if ($location->id == $this->locationId) {
                $matrixLocation = $location;
                break;
            }
        }

        if ($matrixLocation === null) {
            $this->addError('locationId', Craft::t('event-registration', 'The location ID provided is not valid for this event.'));
            return false;
        }

        // Enforce capacity limits
        $capacity = $matrixLocation->capacity ?? PHP_INT_MAX;
        if (EventRegistration::$plugin->eventRegistrationService->getRegistrantsCountForEventByLocationAndTime($event, $location, $occurrence) >= $capacity) {
            $this->addError('locationId', Craft::t('event-registration', 'Capacity limits for this event have been reached.'));
            return false;
        }

        $this->_event = $event;
        $this->_location = $location;
        return true;
    }

    /**
     * Handles the database interaction
     *
     * @return boolean
     */
    public function save()
    {
        if ($this->validate()) {
            $model = new EventRegistrationRecord;
            $model->marketingOptin = 0;
            $model->load(['EventRegistrationRecord' => $this->getAttributes()]);
            $model->siteId = Craft::$app->sites->getPrimarySite()->id;
            if ($model->save()) {
                // Send an email to the customer
                Craft::$app->queue->push(new EmailJob([
                    'template' => 'event-registration.twig',
                    'subject' => Craft::t('event-registration', 'You\'re Registered!'),
                    'to' => [$this->emailAddress => ($this->firstName . ' ' . $this->lastName)],
                    'templateData' => [
                        'event' => $this->_event->id,
                        'location' => $this->_location->id,
                        'occurrence' => $this->occurrenceDate
                    ],
                ]));

                return true;
            }
        }

        return false;
    }
}
