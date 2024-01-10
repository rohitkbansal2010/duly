<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\eventregistration\services;

// Craft classes
use Craft;
use craft\base\Component;
use craft\elements\Entry;
use craft\elements\MatrixBlock;
use craft\records\MatrixBlockType;
use yii\db\Query;

use DateTime;
use Solspace\Calendar\Elements\Event;
use punchkick\eventregistration\records\EventRegistrationRecord;

/**
 * EventRegistrationService Service
 *
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 */
class EventRegistrationService extends Component
{
    public function getRegistrantsForEventByLocationAndTime(
        Event $event,
        MatrixBlock $entry,
        DateTime $date = null,
        DateTime $minDate = null,
        DateTime $maxDate = null,
        bool $selectCount = false
    ): Query {
        $selectFields = [
            'id', 'dateCreated', 'occurrenceDate', 'firstName', 'lastName', 'phoneNumber', 'emailAddress', 'marketingOptin', 'eventId', 'locationId'
        ];

        if ($selectCount) {
            \array_push($selectFields, 'COUNT(*) AS count');
        }
        
        $query = (new Query)
            ->select($selectFields)
            ->from(EventRegistrationRecord::tableName())
            ->where([
                'locationId' => $entry->id,
                'eventId' => $event->id,
            ]);

        if (isset($date)) {
            $query = $query->andWhere([
                'occurrenceDate' => $date->format('Y-m-d')
            ]);
        } else {
            if (isset($minDate)) {
                $query = $query->andWhere([
                    '>=',
                    'occurrenceDate', $minDate->format('Y-m-d')
                ]);
            }
            if (isset($maxDate)) {
                $query = $query->andWhere([
                    '<=',
                    'occurrenceDate', $maxDate->format('Y-m-d')
                ]);
            }
        }
        
        return $query;
    }

    public function getRegistrantsCountForEventByLocationAndTime(
        Event $event,
        MatrixBlock $entry,
        DateTime $date = null,
        DateTime $minDate = null,
        DateTime $maxDate = null
    ): int {
        return $this->getRegistrantsForEventByLocationAndTime($event, $entry, $date, $minDate, $maxDate)->count();
    }

    /**
     * Returns all active event services with params applied
     * @param array $eventParams
     */
    public function getServicesForEvents(array $eventParams)
    {
        $events = Event::buildQuery($eventParams)->all();
        $services = [];

        foreach ($events as $event) {
            if ($event->eventSpecialty->one() != null && $event->eventSpecialty->one()->specialty != null) {
                foreach ($event->eventSpecialty->one()->specialty->all() as $service) {
                    array_push($services, $service->title);
                }
            }
        }

        sort($services);
        $services = array_unique($services);

        return $services;
    }
}
