<?php

namespace punchkick\eventregistration\Elements\Db;

use Solspace\Calendar\Elements\Db\EventQuery as BaseEventQuery;
use punchkick\eventregistration\Elements\Event;
use Carbon\Carbon;
use yii\db\Query as YiiQuery;

class EventQuery extends BaseEventQuery
{
    /** @var Event[] */
    protected $events;

    /** @var int */
    protected $totalCount;

    /** @var string[] */
    public $cityIds;

    /**
     * @param string $q
     * @param null   $db
     *
     * @return int
     */
    public function count($q = '*', $db = null): int
    {
        if ($this->totalCount === null) {
            $this->all($db);
            $this->totalCount = \count($this->events ?? []);
        }

        return $this->totalCount;
    }

    /**
     * @param null $db
     *
     * @return Event[]
     */
    public function all($db = null): array
    {
        $this->limit = PHP_INT_MAX;
        $this->offset = 0;
        $this->events = parent::all();

        $tempEvents = [];
        $classes = [];

        $matrixBlockLocationTypeID = (new YiiQuery())
            ->select([
                'id'
            ])
            ->from('matrixblocktypes')
            ->where('name = "Location"')
            ->one();

        foreach ($this->events as $event) {
            $locations = (new YiiQuery())
                ->select([
                    'id'
                ])
                ->from('matrixblocks')
                ->where('matrixblocks.ownerId = ' . $event->id)
                ->andWhere('matrixblocks.typeId = ' . $matrixBlockLocationTypeID['id']);

            foreach ($locations->all() as $location) {
                // "class" events are treated differently
                // we show them only once (per location) instead of showing multiples for individual dates
                // e.g. a class on March 2 and March 3, in Chicago and Des Plaines, should generate 2 entries total
                if ($event->repeatsOnSelectDates()) {
                    // add current event to the list of classes
                    $classes[$event->id] = $classes[$event->id] ?? [];

                    // if this class location was already processed, skip it
                    if (\in_array($location, $classes[$event->id])) {
                        continue;
                    }
                    
                    // add current class location to the list of classes so it can be skipped when encountered again
                    \array_push(
                        $classes[$event->id],
                        $location
                    );
                }

                // do not include filtered out cities
                if (isset($this->cityIds) && !\in_array($location['id'], $this->cityIds)) {
                    continue;
                }

                $tempEvent = clone $event;
                $tempEvent->internalLocationId = $location;
                $tempEvents[] = $tempEvent;
            }
        }

        $this->events = $tempEvents;

        return $this->events;
    }
}
