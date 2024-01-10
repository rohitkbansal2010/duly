<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2020 Punchkick Interactive
 */

namespace punchkick\eventregistration\forms;

use Craft;
use craft\elements\Category;
use craft\elements\Entry;
use DateTime;
use Solspace\Calendar\Calendar;
use punchkick\eventregistration\Elements\Event;
use yii\base\Model;
use yii\db\Expression;
use yii\db\Query;
use yii\helpers\Inflector;

/**
 * EventSearch Form
 *
 * This form wraps the logic for requesting a CSV list of event registrations.
 *
 * @author    Punchkick Interactive
 * @package   EventRegistrationPlugin
 * @since     0.0.1
 */
final class EventSearchForm extends Model
{
    /**
     * @var array $service
     */
    public $service;

    /**
     * @var array $category
     */
    public $category;

    /**
     * @var array $city
     */
    public $city;

    /**
     * @var string $rangeStart
     */
    public $rangeStart;

    /**
     * @var string $rangeEnd
     */
    public $rangeEnd;

    /**
     * @var string $startsBeforeOrAt
     */
    public $startsBeforeOrAt;

    /**
     * @var string $search_event_attribute
     */
    public $search_event_attribute;

    /**
     * Base query
     *
     * @var array
     */
    private $_baseQuery = [
        'calendar' => 'dupage_event_calendar',
        'limit' => 10,
        'enabledForSite' => true,
        'orderBy' => 'startDate asc',
        'rangeStart' => 'now',
        'rangeEnd' => '+1 year'
    ];

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['city', 'service', 'category'], 'each', 'rule' => ['string']],
            [['search_event_attribute'], 'string'],
            [['rangeStart', 'startsBeforeOrAt', 'rangeEnd'], 'date', 'format' => 'php:Y-m-d'],
            [['service', 'category', 'city', 'search_event_attribute', 'rangeStart', 'rangeEnd', 'startBeforeOrAt'], 'safe']
        ];
    }

    /**
     * Retrieves Specialties
     *
     * @return array
     */
    public function getServices()
    {
        if ($this->service === null) {
            return [];
        }

        $serviceEntries = [];

        foreach ($this->service as $singleService) {
            $decodedValue = htmlspecialchars_decode($singleService);

            $entry = Entry::find()
                ->section('services')
                ->where(['like', 'title', $decodedValue])
                ->one();
            
            if ($entry) {
                \array_push($serviceEntries, $entry);
            }
        }

        return $serviceEntries;
    }

    /**
     * Retrieves categories
     *
     * @return array
     */
    public function getCategories()
    {
        if ($this->category === null) {
            return [];
        }

        $categories = [];

        foreach ($this->category as $singleCategory) {
            $decodedValue = htmlspecialchars_decode($singleCategory);

            $foundCategory = Category::find()
                ->group('eventCategories')
                ->where(['like', 'title', $decodedValue])
                ->one();
            
            if ($foundCategory) {
                \array_push($categories, $foundCategory);
            }
        }

        return $categories;
    }

    /**
     * Retrieves cities
     *
     * @return array
     */
    public function getCities()
    {
        if ($this->city === null) {
            return [];
        }

        $hasVirtualEvent = false;
        $ownerIds = [];
        $query = (new Query)
            ->select(['matrixblocks.id', 'ownerId'])
            ->from('matrixblocks')
            ->innerJoin(
                'matrixcontent_eventlocation',
                'matrixblocks.id = matrixcontent_eventlocation.elementId'
            );

        // params need to be unique so index needs to be included
        foreach ($this->city as $index => $singleCity) {
            if ($singleCity == 'Virtual') {
                $hasVirtualEvent = true;
            }

            $expression = new Expression(
                "JSON_CONTAINS(
                   field_location_addressLocation,
                   :singleCity$index,
                   '\$.parts.city'
                )",
                [':singleCity' . $index => json_encode($singleCity)]
            );
            $query->orWhere($expression);
        }

        if ($hasVirtualEvent) {
            $query->orWhere(['field_location_isVirtual' => true]);
        }

        return $query
            ->distinct()
            ->all();
    }

    /**
     * Generates a query for use in the controller
     *
     * @return yii\db\Query
     */
    public function getQuery()
    {
        $query = $this->_baseQuery;

        if ($this->validate()) {
            if ($this->rangeStart !== null) {
                $query['rangeStart'] = $this->rangeStart;
            }

            if ($this->rangeEnd !== null) {
                $query['rangeEnd'] = $this->rangeEnd;
            }

            if ($this->startsBeforeOrAt !== null) {
                $query['rangeEnd'] = $this->startsBeforeOrAt;
            }

            if ($this->search_event_attribute !== null) {
                $query['search'] = $this->search_event_attribute;
            }

            $filteredEventTypes = $this->getCategories();
            $filteredSpecialties = $this->getServices();

            if (!empty($filteredEventTypes) || !empty($filteredSpecialties)) {
                $query['relatedTo'] = ['and'];
                if (!empty($filteredEventTypes)) {
                    $query['relatedTo'][] = ['targetElement' => $filteredEventTypes, 'field' => 'eventCategories'];
                }

                if (!empty($filteredSpecialties)) {
                    $query['relatedTo'][] = ['targetElement' => $filteredSpecialties, 'field' => 'eventSpecialty'];
                }
            }

            $cities = $this->getCities();

            if (!empty($cities)) {
                $query['id'] = \array_map(function ($location) {
                    return $location['ownerId'];
                }, $cities);
            }
        }
        
        $results = Event::buildQuery($query)
            ->addSelect([
                'matrixblocks.id AS internalLocationId'
            ])
            ->innerJoin(
                'matrixblocks',
                'matrixblocks.ownerId = calendar_events.id'
            );
        
        // city filter happens in punchkick\eventregistration\Elements\Db\EventQuery::all, after the core Event filter
        // save city IDs to be filetered
        if (!empty($cities)) {
            $results->cityIds = \array_map(
                function ($city) {
                    return $city['id'];
                },
                $cities
            );
        }

        return $results;
    }

    /**
     * Populates data from the qury parameters
     *
     * @param array $data
     * @return void
     */
    public function loadData(array $data = [])
    {
        foreach ($data as $attribute => $param) {
            if ($this->hasProperty($attribute)) {
                $method = 'set' . Inflector::camelize($attribute);
                if (\method_exists($this, $method)) {
                    $this->$method($param);
                } else {
                    $this->$attribute = $param;
                }
            }
        }
    }
}
