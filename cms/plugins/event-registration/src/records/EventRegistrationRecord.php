<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\eventregistration\records;

use Craft;
use DateTime;
use yii\base\Exception;
use craft\db\ActiveRecord;
use craft\elements\MatrixBlock;
use Solspace\Calendar\Calendar;
use Solspace\Calendar\Elements\Event;
use modules\DupageCoreModule\queue\EmailJob;
use punchkick\eventregistration\EventRegistration;
use punchkick\eventregistration\EventRegistrationPlugin;

/**
 * EventRegistrationPlugin Record
 *
 * ActiveRecord is the base class for classes representing relational data in terms of objects.
 *
 * Active Record implements the [Active Record design pattern](http://en.wikipedia.org/wiki/Active_record).
 * The premise behind Active Record is that an individual [[ActiveRecord]] object is associated with a specific
 * row in a database table. The object's attributes are mapped to the columns of the corresponding table.
 * Referencing an Active Record attribute is equivalent to accessing the corresponding table column for that record.
 *
 * http://www.yiiframework.com/doc-2.0/guide-db-active-record.html
 *
 * @author    Punchkick Interactive
 * @package   EventRegistrationPlugin
 * @since     0.0.1
 */
class EventRegistrationRecord extends ActiveRecord
{
    /**
     * @var Event $_event
     */
    private $_event;

    /**
     * @var MatrixBlock $_location
     */
    private $_location;

    // Public Static Methods
    // =========================================================================

     /**
     * Declares the name of the database table associated with this AR class.
     * By default this method returns the class name as the table name by calling [[Inflector::camel2id()]]
     * with prefix [[Connection::tablePrefix]]. For example if [[Connection::tablePrefix]] is `tbl_`,
     * `Customer` becomes `tbl_customer`, and `OrderItem` becomes `tbl_order_item`. You may override this method
     * if the table is not named after this convention.
     *
     * By convention, tables created by plugins should be prefixed with the plugin
     * name and an underscore.
     *
     * @return string the table name
     */
    public static function tableName()
    {
        return'{{%eventregistration_eventregistrationrecord}}';
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['eventId', 'locationId', 'occurrenceDate', 'firstName', 'lastName', 'phoneNumber', 'emailAddress', 'marketingOptin'], 'required'],
            [['eventId', 'locationId'], 'integer'],
            [['firstName', 'lastName'], 'string'],
            [['occurrenceDate'], 'date', 'format' => 'php:Y-m-d'],
            [['emailAddress'], 'email'],
            [['phoneNumber'], 'string'],
            [['marketingOptin'], 'default', 'value' => 0],
            [['marketingOptin'], 'boolean'],
            [['eventId', 'locationId', 'occurrenceDate', 'firstName', 'lastName', 'phoneNumber', 'emailAddress', 'marketingOptin'], 'safe']
        ];
    }

    /**
     * Returns an event instsance
     *
     * @return Event
     */
    public function getEvent() :? Event
    {
        if ($this->_event === null) {
            $this->_event = Calendar::getInstance()->events
                ->getEventById($this->eventId);
        }

        return $this->_event;
    }

    /**
     * Returns the Matrix Block location data for the location
     *
     * @return MatrixBlock
     */
    public function getLocation() :? MatrixBlock
    {
        if ($this->_location === null) {
            $this->_location = MatrixBlock::find()
                ->owner($this->getEvent())
                ->id($this->locationId)
                ->one();
        }

        return $this->_location;
    }

    /**
     * Returns the current capacity of the event
     *
     * @return integer
     */
    public function getCurrentCapacity() : int
    {
        return (int)EventRegistration::$plugin
            ->eventRegistrationService
            ->getRegistrantsCountForEventByLocationAndTime(
                $this->getEvent(),
                $this->getLocation(),
                DateTime::createFromFormat('Y-m-d', $this->occurrenceDate)
            );
    }

    /**
     * @inheritdoc
     */
    public function afterSave($insert, $changedAttributes)
    {
        parent::afterSave($insert, $changedAttributes);
        if ($insert) {
            // If the notification email is empty, don't do anything
            if (empty($this->getLocation()->notificationEmail)) {
                return true;
            }

            $current = $this->getCurrentCapacity();
            $previous = $current - 1;
            $maxCapacity = $this->getLocation()->capacity;

            $percentageNow = ($current / $maxCapacity) * 100;
            $precentagePrev = ($previous / $maxCapacity) * 100;

            foreach ($this->getLocation()->notificationIntervals->getOptions() as $option) {
                if ($option->selected) {
                    if ($percentageNow >= $option->value && $precentagePrev < $option->value) {
                        Craft::info('Sending notification message to watcher for ' . $option->label);

                        Craft::$app->queue->push(new EmailJob([
                            'template' => 'event-watcher.twig',
                            'subject' => Craft::t('event-registration', 'Your event has hit a capacity milestone!'),
                            'to' => $this->getLocation()->notificationEmail,
                            'templateData' => [
                                'event' => $this->getEvent()->id,
                                'location' => $this->getLocation()->id,
                                'occurrence' => $this->occurrenceDate,
                                'pct' => $percentageNow,
                                'maxCapacity' => $maxCapacity,
                                'currentCapacity' => $current
                            ],
                        ]));
                        break;
                    }
                }
            }

            return true;
        }
    }
}
