<?php

namespace modules\DupageCoreModule\models;

use Craft;
use DateTime;
use craft\elements\Entry;
use craft\elements\MatrixBlock;
use yii\base\Model;
use yii\data\ArrayDataProvider;
use yii\data\Sort;
use yii\db\Expression;
use yii\db\Query;
use yii\helpers\Json;

final class Locations extends Model
{
    public $id;

    public $ownerId;

    public $slug;

    public $number;

    public $streetAddress;

    public $address;

    public $city;

    public $postcode;

    public $county;

    public $state;

    public $country;

    public $lat;

    public $lng;

    public $title;
    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['id', 'ownerId', 'slug', 'number', 'streetAddress', 'address', 'city', 'postcode', 'county', 'state', 'country', 'lat', 'lng'], 'required'],
            [['id', 'ownerId', 'type', 'number', 'streetAddress', 'address', 'city', 'postcode', 'county', 'state', 'country', 'lat', 'lng'], 'safe']
        ];
    }

    /**
     * Creates a dataprovider for searching, sorting, and filtering
     *
     * @param array $locations[self]
     * @return ArrayDataProvider
     */
    public static function getDataProvider(array $locations = []) : ArrayDataProvider
    {
        $provider = new ArrayDataProvider([
            'allModels' => $locations,
            'pagination' => [
                'pageSize' => 50
            ],
            'sort' => new Sort([
                'defaultOrder' => [
                    'city' => SORT_ASC
                ],
                'attributes' => [
                    'city' => [
                        'asc' => ['city' => SORT_ASC, 'state' => SORT_ASC],
                        'desc' => ['city' => SORT_DESC, 'state' => SORT_DESC],
                        'default' => SORT_ASC,
                        'label' => 'city'
                    ]
                ]
            ])
        ]);

        return $provider;
    }

    /**
     * Converts data taken from the query into a model instance
     *
     * @param array $locationData
     * @return self
     */
    public static function getModelFromJson($locationData) : self
    {
        $model = new self;
        $locationData['field_address'] = Json::decode($locationData['field_address']);

        $model->setAttributes([
            'id' => $locationData['lid'],
            'ownerId' => $locationData['ownerId'],
            'slug' => $locationData['uri'],
            'number' => $locationData['field_address']['parts']['number'] ?? null,
            'address' => $locationData['field_address']['parts']['address'] ?? null,
            'streetAddress' => $locationData['field_address']['parts']['streetAddress'] ?? null,
            'city' => $locationData['field_address']['parts']['city'] ?? null,
            'state' => $locationData['field_address']['parts']['state'] ?? null,
            'county' => $locationData['field_address']['parts']['county'] ?? null,
            'country' => $locationData['field_address']['parts']['country'] ?? null,
            'postcode' => $locationData['field_address']['parts']['postcode'] ?? null,
            'lat' => $locationData['field_address']['lat'] ?? null,
            'lng' => $locationData['field_address']['lng'] ?? null
        ]);

        return $model;
    }


    /**
     * If a location has a suiteService that matches with the service passed in, the suiteService MatrixBlock is returned
     *
     * @param int $locationId
     * @param int $serviceId
     * @param out[Entry]
     * @return mixed
     */
    public static function getSuiteDetailsForService(int $locationId, int $serviceId, self &$entry = null)
    {
        $data = Craft::$app->cache->getOrSet('suiteDetailsForService____' . $locationId . '__' . $serviceId, function () use ($locationId, $serviceId) {
            $location = Entry::find()
                ->section('locations')
                ->id($locationId)
                ->with(['parent', 'suiteServices'])
                ->one();

            if ($location != null) {
                // grab all services
                $services = $location->suiteServices;

                if ($services != null) {
                    foreach ($services as $service) {
                        $s = $service->serviceType->one();
                        if ($s === null) {
                            return null;
                        } elseif ($s->id == $serviceId) {
                            return [
                                'location' => $location,
                                'service' => $service
                            ];
                        }
                    }
                }
            }

            return false;
        }, 3600);

        if ($data == false) {
            return null;
        }

        $location = $data['location'];
        $service = $data['service'];
        if ($location->parent !== null) {
            $entry = self::getModelFromLocationEntry($location->parent);
            $entry->title = $location->hiddenSuite ? null : $location->title;
        }
        return self::determineSuiteDetails($service);
    }

    /**
     * Creates a $res array that provides all necessary details needed to be displayed on the front end
     *
     * @param MatrixBlock $details
     * @return array
     */
    public static function determineSuiteDetails(Matrixblock $details)
    {
        // set defaults
        $res = [
            'temporarilyClosed' => $details['temporarilyClosed'] ?? null,
            'phoneNumber' => $details['phoneNumber'] ?? null,
            'allowWalkInAppointments' => $details['allowWalkInAppointments'] ?? null,
            'additionalInformation' => $details['additionalInformation'] ?? null,
            'closedRightNow' => false,
            'closedToday' => false,
            'closedTomorrow' => false,
            'todayStartTime' => null,
            'todayEndTime' => null,
            'tomorrowStartTime' => null,
            'tomorrowEndTime' => null,
            'officeHours' => null,
            'nearestLocations' => null
        ];

        if ($details['closedDates']) {
            $res = self::checkForClosedDates($details, $res);
        }

        if ($details['officeHours']) {
            $res = self::checkHoursSchedule($details, $res);
        }

        if ($details['temporarilyClosed']) {
            $nearestLocations = $details->nearestLocations->all();
            if (count($nearestLocations)) {
                $suites = [];
                $serviceType = $details->serviceType->one();
                foreach ($nearestLocations as $nearestLocation) {
                    $suitesWithService = $nearestLocation
                        ->children
                        ->type('suite')
                        ->relatedTo([
                            'field' => 'suiteServices',
                            'targetElement' => $serviceType
                        ])
                        ->all();
                    $suites = array_merge($suites, $suitesWithService);
                }
                $res['nearestLocations'] = $suites;
            }
        }

        return $res;
    }

    /**
     * Checks to see if suiteService has any specific assigned closed dates
     *
     * @param MatrixBlock $details
     * @param array $res
     * @return array
     */
    private static function checkForClosedDates(Matrixblock $details, array $res)
    {
        foreach ($details['closedDates'] as $date) {
            $dateClosed = $date['datesClosed'];
            if ($dateClosed) {
                $formattedDate = $dateClosed->format('Y-m-d');

                if ($formattedDate == (new DateTime)->format('Y-m-d')) {
                    $res['closedToday'] = true;
                }

                if ($formattedDate == (new DateTime('+1 day'))->format('Y-m-d')) {
                    $res['closedTomorrow'] = true;
                }
            }
        }

        return $res;
    }

    /**
     * Checks to see if today and tomorrow dates are part of any listed suiteService officeHours
     *
     * @param MatrixBlock $details
     * @param array $res
     * @return array
     */
    private static function checkHoursSchedule(Matrixblock $details, array $res)
    {
        foreach ($details['officeHours'] as $key => $value) {
            $day = $value['daysOfWeek'];
            
            $fullDayToday = strtolower((new DateTime)->format('l'));
            $fullDayTomorrow = strtolower((new DateTime("+1 day"))->format('l'));

            if ($day == $fullDayToday) {
                if ($value['closed'] == true) {
                    $res['closedToday'] = true;
                } else {
                    if ($value['openingHours']) {
                        $res['todayStartTime'] = $value['openingHours'];
                    }

                    if ($value['closingHours']) {
                        if ((new DateTime)->format('H:i:s') > $value['closingHours']->format('H:i:s')) {
                            $res['closedRightNow'] = true;
                        }

                        $res['todayEndTime'] = $value['closingHours'];
                    }
                }
            }

            if ($day == $fullDayTomorrow) {
                if ($value['closed'] == true) {
                    $res['closedTomorrow'] = true;
                } else {
                    if ($value['openingHours']) {
                        $res['tomorrowStartTime'] = $value['openingHours'];
                    }

                    if ($value['closingHours']) {
                        $res['tomorrowEndTime'] = $value['closingHours'];
                    }
                }
            }
        }

        return $res;
    }

    /**
     * Converts a Location Entry into a model instance
     *
     * @param Entry $location
     * @return self
     */
    public static function getModelFromLocationEntry(Entry $location) : self
    {
        $model = new self;
        $address = $location->address;

        $model->setAttributes([
            'id' => $location->id,
            'ownerId' => $location->id,
            'slug' => $location->uri[0] == '/' ? $location->uri : "/{$location->uri}",
            'number' => $address->parts->number ?? null,
            'address' => $address->parts->address ?? null,
            'streetAddress' => $address->parts->streetAddress ?? null,
            'city' => $address->parts->city ?? null,
            'state' => $address->parts->state ?? null,
            'county' => $address->parts->county ?? null,
            'country' => $address->parts->country ?? null,
            'postcode' => $address->parts->postcode ?? null,
            'lat' => $address->lat ?? null,
            'lng' => $address->lng ?? null
        ]);

        return $model;
    }

    /**
     * Wrapper function for getLocationsForPhysicians - returns an array of locations for a single physician
     *
     * @param Entry $physician
     * @return ArrayDataProvider
     */
    public static function getLocationsForPhysician(Entry $physician): ArrayDataProvider
    {
        return self::getLocationsForPhysicians([$physician]);
    }

    /**
     * Returns all locations associate to a list of physicians
     *
     * @param array[Entry[Physician]] $physicians
     * @param array[Entry[Location]] $additionalLocations additional locations to return that are not associated to any physicians
     * @return ArrayDataProvider
     */
    public static function getLocationsForPhysicians(array $physicians = [], array $additionalLocations = []): ArrayDataProvider
    {
        $ids = \array_map(function ($physician) {
            return $physician->id;
        }, $physicians);

        $query = self::getLocationsForPhysiciansQuery($ids);

        // This is all the unique locations and suites for a given physician, with duplicates addresses removed due to sgroupBy('ownerId')
        $results = $query->all();
        $locations = [];

        foreach ($results as $result) {
            $locations[] = self::getModelFromJson($result);
        }

        foreach ($additionalLocations as $additionalLocation) {
            if (!$additionalLocation) {
                continue;
            }

            // Location suites do not have addresses associated to them, but parent Locations do
            if ($additionalLocation->parent) {
                $additionalLocation = $additionalLocation->parent;
            }

            // ignore duplicates
            if (array_search($additionalLocation->id, array_column($locations, 'ownerId')) === false) {
                $locations[] = self::getModelFromLocationEntry($additionalLocation);
            }
        }

        return self::getDataProvider($locations);
    }

    public static function getLocationsForPhysiciansQuery(array $ids = []): Query
    {
        return (new Query)
            ->select([
                'pl.locationElementId AS lid',
                'c.field_address as field_address',
                new Expression("JSON_UNQUOTE(JSON_EXTRACT(c.field_address, '$.ownerId')) AS ownerId"),
                "CONCAT('/', uri) AS uri" # Add a leading slash to convert relative URI into an absolute URI
            ])
            ->from('physicians_locations AS pl')
            ->innerJoin('content AS c', 'c.elementId = pl.locationElementId')
            ->innerJoin('elements_sites AS es', 'pl.locationElementId = es.elementId')
            ->groupBy('ownerId')
            ->where(['pl.physicianElementId' => $ids]);
    }
}
