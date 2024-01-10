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
use League\Csv\Writer;
use modules\DupageCoreModule\queue\EmailJob;
use punchkick\eventregistration\EventRegistration;
use punchkick\eventregistration\EventRegistrationPlugin;
use punchkick\eventregistration\records\EventRegistrationRecord;
use Solspace\Calendar\Calendar;
use Solspace\Calendar\Elements\Event;
use yii\base\Model;
use yii\db\Query;

/**
 * EventRegistrationPlugin Form
 *
 * This form wraps the logic for requesting a CSV list of event registrations.
 *
 * @author    Punchkick Interactive
 * @package   EventRegistrationPlugin
 * @since     0.0.1
 */
final class EventRegistrationsListForm extends Model
{
    /**
     * @var int $eventId
     */
    public $eventId;

    /**
     * @var int $locationId
     */
    public $locationId;

    /**
     * @var string $date
     */
    public $date;
    
    /**
     * @var string $minDate
     */
    public $minDate;
    
    /**
     * @var string $maxDate
     */
    public $maxDate;
    
    /**
     * @var Event $event
     */
    private $event;
    
    /**
     * @var MatrixBlock $location
     */
    private $location;
    
    /**
     * @var Query $query
     */
    private $query;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['eventId', 'locationId'], 'required'],
            [['eventId', 'locationId'], 'integer'],
            [['date', 'minDate', 'maxDate'], 'string'],
            [['date'], 'date', 'format' => 'php:Y-m-d'],
            [['minDate', 'maxDate'], 'date', 'format' => 'php:m/d/Y'],
            [['date'], 'default', 'value' => null],
            [['minDate'], 'default', 'value' => null],
            [['maxDate'], 'default', 'value' => null],
        ];
    }

    /**
     * Returns the list of current registrations for the event at the specified location and date(s)
     *
     * @return ?string
     */
    public function getRegistrationsList(): ?array
    {
        return $this->getQuery()->all();
    }

    /**
     * Returns the CSV of current registrations for the event at the specified location and date(s)
     *
     * @return ?string
     */
    public function getRegistrationsListAsCSV(): ?string
    {
        $records = $this->getRegistrationsList();

        $records = \array_map(function ($record) {
            $record['event'] = $this->event->title;
            $record['address'] = $this->location->addressLocation->address;
            unset($record['uid'], $record['eventId'], $record['locationId'], $record['siteId'], $record['dateUpdated'], $record['count']);
            $record = \array_values($record);
            return $record;
        }, $records);

        $header = [
                'ID',
                'Registered At',
                'Event Date',
                'First Name',
                'Last Name',
                'Phone Number',
                'Email Address',
                'Marketing Optin?',
                'Event Name',
                'Address'
            ];

        $csv = Writer::createFromString('');
        $csv->insertOne($header);
        $csv->insertAll($records);

        return $csv->getContent();
    }

    /**
     * Returns the CSV filename.
     *
     * @return string
     */
    public function getCSVFilename(): string
    {
        $fileName =  $this->event->title . ' ' . $this->location->addressLocation->address;

        if (isset($this->date)) {
            $fileName .= ' ' . $this->date;
        } else {
            if (isset($this->minDate)) {
                $fileName .= ' from ' . \DateTime::createFromFormat('m/d/Y', "{$this->minDate}")->format('Y-m-d');
            }
            if (isset($this->maxDate)) {
                $fileName .= ' to ' . \DateTime::createFromFormat('m/d/Y', "{$this->maxDate}")->format('Y-m-d');
            }
        }

        return $fileName . '.csv';
    }

    /**
     * Get $query
     *
     * @return ?Query
     */
    public function getQuery(bool $selectCount = false)
    {
        if (!$this->validate()) {
            return null;
        }

        $this->event = Event::find()
            ->id($this->eventId)
            ->one();
        
        if ($this->event === null) {
            return null;
        }

        $this->location = MatrixBlock::find()
                ->owner($this->event)
                ->id($this->locationId)
                ->one();

        if ($this->location === null) {
            return null;
        }

        $this->query = EventRegistration::$plugin->eventRegistrationService->getRegistrantsForEventByLocationAndTime(
            $this->event,
            $this->location,
            $this->date ? DateTime::createFromFormat('Y-m-d', $this->date) : null,
            $this->minDate ? DateTime::createFromFormat('m/d/Y', $this->minDate) : null,
            $this->maxDate ? DateTime::createFromFormat('m/d/Y', $this->maxDate) : null,
            $selectCount
        );

        return $this->query;
    }

    /**
     * Get $event
     *
     * @return  Event
     */
    public function getEvent()
    {
        return $this->event;
    }

    /**
     * Get $location
     *
     * @return  MatrixBlock
     */
    public function getLocation()
    {
        return $this->location;
    }
}
