<?php
/**
 * Physicians module for Craft CMS 3.x
 *
 * Allows for extended management of the physicians section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\physiciansmodule\services;

use Craft;
use craft\base\Component;
use craft\elements\Category;
use craft\elements\db\EntryQuery;
use craft\elements\Entry;
use DateTime;
use DateTimezone;
use DateInterval;
use DatePeriod;
use ether\simplemap\services\GeoService;
use modules\DupageCoreModule\DupageCoreModule;
use modules\locationsmodule\LocationsModule;
use modules\physiciansmodule\forms\PhysicianSearchForm;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\SchedulingModule;
use yii\db\ArrayExpression;
use yii\db\Expression;
use yii\db\Query;

/**
 * @author    Wipfli Digital
 * @package   PhysiciansModule
 * @since     1.0.0
 */
class PhysiciansModuleService extends Component
{
    // Ages seen labels
    const INFANTS = "Infants";
    const CHILDREN = "Children";
    const ADOLESCENTS = "Adolescents";
    const ADULTS = "Adults";
    const SENIORS = "Seniors";

    /**
     * Returns an instance of EntryQuery that queries physicians with provided form parameters.
     *
     * @param PhysicianSearchForm $form Form that validates and contains supported query parameters
     * @return EntryQuery
     */
    public function queryPhysicians(PhysicianSearchForm $form): EntryQuery
    {
        $physicians = Entry::find()
            ->section('physicians')
            ->with(['physicianHeadshot', 'physicianSpeciality']);

        $relatedTo = ['and'];

        $this->relatedToAgeSeenFilter($form, $physicians);
        $this->relatedToAgeSeenRange($form, $physicians);
        $this->relatedToCities($form, $relatedTo);
        $this->relatedToServices($form, $relatedTo);
        $this->relatedToLanguages($form, $relatedTo);
        $this->relatedToGenders($form, $physicians);
        $this->relatedToLgbtqiaResource($form, $physicians);
        $this->relatedToHospitals($form, $relatedTo);

        if ($form->rating) {
            $this->orderPhysiciansByRating($physicians, $form->rating);
        }

        if ($form->availability) {
            if ($form->availability == 'Today') {
                $startDate = new DateTime();
                $tomorrow = $startDate->modify('+1 days')->format('Y-m-d');
                $physicians->andWhere([
                    '<=',
                    'field_physicianNextAvailableAppointmentTime',
                    $tomorrow
                ])
                ->andWhere([
                    '>=',
                    'field_physicianNextAvailableAppointmentTime',
                    (new DateTime(null, new DateTimezone("US/Central")))->format("Y-m-d H:i:s")
                ]);
            };

            if ($form->availability == 'Next 3 Days') {
                $startDate = new DateTime();
                $threeDays = $startDate->modify('+3 days');
                $physicians->andWhere([
                    '<=',
                    'field_physicianNextAvailableAppointmentTime',
                    $threeDays->format('Y-m-d')
                ])
                ->andWhere([
                    '>=',
                    'field_physicianNextAvailableAppointmentTime',
                    (new DateTime(null, new DateTimezone("US/Central")))->format("Y-m-d H:i:s")
                ]);
            }
        }

        if ($form->search_physician_attribute) {
            // related physicians
            $ids = Entry::find()
                ->section('physicians')
                ->search("title:*" . $form->search_physician_attribute . "*")
                ->ids();

            // physicians can have alternative names
            // i.e Jacob Smith can be "Jake"
            $alternativeSearchPhysicianNameIds = Entry::find()
                ->section('physicians')
                ->where(['like', 'field_alternativeSearchName', $form->search_physician_attribute])
                ->ids();

            if ($alternativeSearchPhysicianNameIds) {
                $ids = \array_merge($alternativeSearchPhysicianNameIds, $ids);
            }

            // services can have alternative names
            // i.e Obstetrics & Gynecology can be "OBGYN"
            $alternativeSearchServiceNameIds = Entry::find()
                ->section('services')
                ->where(['like', 'field_alternativeSearchServiceName', $form->search_physician_attribute])
                ->one();

            // related services
            $services = Entry::find()
                ->section('services')
                ->title(DupageCoreModule::getInstance()->dupageCoreModuleService->htmlSpecialCharsDecode([$form->search_physician_attribute]))
                ->one();

            // related conditions
            $conditions = Entry::find()
                ->section('conditions')
                ->title(DupageCoreModule::getInstance()->dupageCoreModuleService->htmlSpecialCharsDecode([$form->search_physician_attribute]))
                ->one();

            // related procedures
            $procedures = Entry::find()
                ->section('procedures')
                ->title(DupageCoreModule::getInstance()->dupageCoreModuleService->htmlSpecialCharsDecode([$form->search_physician_attribute]))
                ->one();

            if ($alternativeSearchServiceNameIds) {
                $ids = $this->getRelatedServicesSearchTerm($physicians, $alternativeSearchServiceNameIds, $ids);
            }

            if ($services) {
                $ids = $this->getRelatedServicesSearchTerm($physicians, $services, $ids);
            }

            if ($conditions) {
                $ids = $this->getRelatedProceduresAndConditions($physicians, $conditions, $ids);
            }

            if ($procedures) {
                $ids = $this->getRelatedProceduresAndConditions($physicians, $procedures, $ids);
            }
        
            $physicians->id($ids);
        }

        if (count($relatedTo) > 1) {
            $physicians->relatedTo($relatedTo);
        }

        if ($form->address && !$form->order_by) {
            $form->order_by = 1;
        }

        $this->orderPhysicians($form, $physicians);
        $physicians->distinct();

        return $physicians;
    }

    private function getRelatedServicesSearchTerm(EntryQuery $physicians, Entry $services, array $ids)
    {
        // related services of search term
        $parentServiceQuery = Entry::find()
            ->section('services')
            ->descendantOf($services)
            ->all();

        // grab all ids if search term is either a direct match or of a sub-specialty
        $serviceRelatedPhysicianIDs = $physicians
            ->relatedTo(['or',
                [
                    'targetElement' => $services,
                ],
                [
                    'targetElement' => $parentServiceQuery
                ]
            ])
            ->ids();

        $ids = \array_merge($ids, $serviceRelatedPhysicianIDs);

        return $ids;
    }

    private function getRelatedProceduresAndConditions(EntryQuery $physicians, Entry $type, array $ids)
    {
        $servicesQuery = Entry::find()
            ->section('services')
            ->relatedTo($type);

        $typeIds = $physicians
            ->relatedTo(['or',
                [
                    'targetElement' => $type,
                ],
                [
                    'targetElement' => $servicesQuery
                ]
            ])
            ->ids();
        
        $ids = \array_merge($ids, $typeIds);
        return $ids;
    }

    /**
     * Applies an ORDER BY clause to the query based on the picked order. Defaults to physician name asc by default.
     *
     * @param PhysicianSearchForm $form Form that validates and contains supported query parameters
     * @param EntryQuery $query Query to be modified
     * @return EntryQuery
     */
    private function orderPhysicians(PhysicianSearchForm $form, EntryQuery $query)
    {
        if ($form->order_by == PhysicianSearchForm::SORT_PHYSICIANS_BY_DISTANCE) {
            $this->orderPhysiciansByDistance($form, $query);
        } elseif ($form->order_by == PhysicianSearchForm::SORT_PHYSICIANS_BY_RATING) {
            $this->orderPhysiciansByRating($query);
        } elseif ($form->order_by == PhysicianSearchForm::SORT_PHYSICIANS_BY_LAST_NAME_ASC) {
            $query->orderBY(new Expression(
                'field_lastName IS NULL ASC, field_lastName ASC'
            ));
        } elseif ($form->order_by == PhysicianSearchForm::SORT_PHYSICIANS_BY_LAST_NAME_DESC) {
            $query->orderBY(new Expression(
                'field_lastName DESC, field_lastName IS NULL DESC'
            ));
        } else {
            // default to next available time
            $query->orderBY(new Expression(
                'field_physicianNextAvailableAppointmentTime IS NULL ASC, field_physicianNextAvailableAppointmentTime ASC'
            ));
        }
    }

    /**
     * Applies an ORDER BY clause to the query to sort physicians by their locations.
     * If address is applied, lat/lng coordinates are calculated from the address.
     * If lat/lng coordinates are supplied, lat/lng coordinates are used for the sort.
     * Else, skipped.
     *
     * @param PhysicianSearchForm $form Form that validates and contains supported query parameters
     * @param EntryQuery $query Query to be modified
     * @return EntryQuery
     */
    public function orderPhysiciansByDistance(PhysicianSearchForm $form, EntryQuery $query)
    {
        $longitude = (float)$form->longitude;
        $latitude = (float)$form->latitude;

        if ($form->address) {
            $coordinates = LocationsModule::getInstance()
                ->locationsModuleService
                ->getLatLngForAddress($form->address);
            if (!$coordinates) {
                return;
            }
            $longitude = $coordinates['lng'];
            $latitude = $coordinates['lat'];
        }

        if (!$longitude || !$latitude) {
            return;
        }

        $orderedPhysicianIDs = \implode(", ", $this->getPhysicianIDsOrderedByDistanceFromLatLng($longitude, $latitude));
        $query->orderBy([new Expression("FIELD (elements.id, {$orderedPhysicianIDs})")]);
    }

    /**
     * Returns a list of physicians ordered by distance ASC from the given lat/lng
     * according to physicians' locations, and the distance for each physician.
     *
     * @param float $longitude
     * @param float $latitude
     * @return EntryQuery
     */
    private function getPhysiciansOrderedByDistanceFromLatLngHelper($longitude, $latitude): array
    {
        $distanceExpression = new Expression(
            $this->sqlDistanceBetweenTwoGivenPoints(
                $longitude,
                $latitude,
                "JSON_EXTRACT(field_address, '$.lng')",
                "JSON_EXTRACT(field_address, '$.lat')"
            )
        );

        return (new Query())
            ->select([
                'physicianElementId',
                $distanceExpression . ' as distance'
            ])
            ->from('physicians_locations as pl')
            ->innerJoin('content as c', 'c.elementId = pl.locationElementId')
            ->orderBy($distanceExpression)
            ->all();
    }

    /**
     * Returns a list of physician IDs ordered by distance ASC from the given lat/lng
     * according to physicians' locations;
     *
     * @param float $longitude
     * @param float $latitude
     * @return EntryQuery
     */
    public function getPhysicianIDsOrderedByDistanceFromLatLng($longitude, $latitude): array
    {
        $orderedPhysiciansByDistance = $this->getPhysiciansOrderedByDistanceFromLatLngHelper($longitude, $latitude);
        return \array_column($orderedPhysiciansByDistance, 'physicianElementId');
    }

    /**
     * Returns a randomized list of physician IDs ordered by distance ASC from the given lat/lng
     * according to physicians' locations within given miles range.
     *
     * @param Entry $physician
     * @param float $maxDistanceInMiles
     * @return [] list of physician IDs
     */
    public function getSimilarPhysicianIds(Entry $physician, float $maxDistanceInMiles): array
    {
        // get all location suites assigned to the physician
        $physicianSuites = $this->getPhysicianLocations($physician);

        $closestPhysiciansPerLocation = [];
        foreach($physicianSuites as $physicianSuite) {
            $location = $physicianSuite->parent ?? $physicianSuite;
            // get a sorted list of physicians for each location and put them all on one list
            // each element contains physician ID and the distance from source lat/lng.
            $closestPhysiciansPerLocation = \array_merge(
                $this->getPhysiciansOrderedByDistanceFromLatLngHelper(
                    $location['address']['lng'],
                    $location['address']['lat'],
                    'distance'
                ),
                $closestPhysiciansPerLocation
            );
        }

        // order the list by distance
        \usort(
            $closestPhysiciansPerLocation,
            function ($p1, $p2) {
                return $p1['distance'] > $p2['distance'];
            }
        );

        // filter out physician who are too far
        // and exclude current physician
        $closestPhysiciansPerLocation = \array_filter(
            $closestPhysiciansPerLocation,
            function ($p) use ($maxDistanceInMiles, $physician) {
                return $p['distance'] <= $maxDistanceInMiles && $p['physicianElementId'] != $physician->id;
            }
        );

        // get physician IDs
        $closestPhysiciansPerLocation = \array_column($closestPhysiciansPerLocation, 'physicianElementId');
        
        // shufle the IDs
        \shuffle($closestPhysiciansPerLocation);
        
        // find core specialty (subspecialty > specialty)
        $specialties = $physician->physicianSpeciality->all();
        // sort specialties by hierarchy levels
        \usort(
            $specialties, 
            function ($s1, $s2) {
                // level 1 == parent
                // level 2 == child
                return $s1->level < $s2->level;
            }
        );
        $coreSpecialties = \array_filter(
            $specialties,
            function ($specialty) use ($specialties) {
                return $specialty->level === $specialties[0]->level;
            }
        );

        $sameServicePhysicians = Entry::find()
            ->section('physicians')
            ->type($physician->type)
            ->relatedTo($coreSpecialties)
            ->ids();

        // filter out physicians who do not practice the same core specialty
        $similarClosePhysicians = \array_filter(
            $closestPhysiciansPerLocation,
            function ($physicianId) use ($sameServicePhysicians) {
                return \in_array($physicianId, $sameServicePhysicians);
            }
        );

        return $similarClosePhysicians;
    }

    /**
     * Filters physicians by ages seen
     *
     * @param PhysicianSearchForm $form
     * @param EntryQuery $physicians
     */
    public function relatedToAgeSeenFilter(PhysicianSearchForm $form, EntryQuery &$physicians)
    {
        if ($form->age) {
            $physicians
                ->innerJoin('matrixblocks', 'matrixblocks.ownerId = elements.id')
                ->innerJoin('matrixcontent_agerestrictions', 'matrixcontent_agerestrictions.elementId = matrixblocks.id');
        
            // 0-4 = Infants
            // 5-12 = Children
            // 13-17 = Adolescents
            // 18-64 = Adults
            // 65-999 = Seniors
            
            // if all filters are selected, do nothing
            // this is a design flaw PM knows about
            if (count($form->age) == 5) {
                return;
            }

            // most physicians default to 0 - 999
            // it shouldn't matter what filter is selected as 0 - 999 sees all age ranges
            $physicians->orWhere(['and',
                ['=','matrixcontent_agerestrictions.field_restriction_minimumAge', 0],
                ['=','matrixcontent_agerestrictions.field_restriction_maximumAge', 999]
            ]);

            if (in_array(self::INFANTS, $form->age)) { 
                $this->addAgeRestriction($physicians, 0, 4);
            }

            if (in_array(self::CHILDREN, $form->age)) {
                $this->addAgeRestriction($physicians, 5, 12);
            }

            if (in_array(self::ADOLESCENTS, $form->age)) {
                $this->addAgeRestriction($physicians, 13, 17);
            }

            if (in_array(self::ADULTS, $form->age)) {
                $this->addAgeRestriction($physicians, 18, 65);
            }
            
            if (in_array(self::SENIORS, $form->age)) {
                $this->addAgeRestriction($physicians, 65, 999);
            }
        }
    }

    /**
     * Filters physicians by ages seen
     *
     * @param PhysicianSearchForm $form
     * @param EntryQuery $physicians
     */
    public function relatedToAgeSeenRange(PhysicianSearchForm $form, EntryQuery &$physicians)
    {
        // custom age slider
        if (isset($form->min_age) && isset($form->max_age)) {
            $physicians
                ->innerJoin('matrixblocks', 'matrixblocks.ownerId = elements.id')
                ->innerJoin('matrixcontent_agerestrictions', 'matrixcontent_agerestrictions.elementId = matrixblocks.id');

            $min = intval($form->min_age);
            $max = intval($form->max_age);

            if ($max == 100) {
                $max = 999;
            }

            $this->addStrictAgeRestriction($physicians, $min, $max);
        }
    }

    /**
     * Applies a strict age filter relation where age must fall within range
     *
     * @param EntryQuery $query Query to be modified
     * @param int $minAge minimum age required
     * @param int $maxAge maximum age required
     * @return EntryQuery
     */
    public function addStrictAgeRestriction(EntryQuery &$physicians, int $min, int $max) 
    {   
        // most physicians default to 0 - 999
        // it shouldn't matter what filter is selected as 0 - 999 sees all age ranges
        $physicians->orWhere(['and',
            ['=','matrixcontent_agerestrictions.field_restriction_minimumAge', 0],
            ['=','matrixcontent_agerestrictions.field_restriction_maximumAge', 999]
        ]);

        $physicians->orWhere(['and',
            ['<=','matrixcontent_agerestrictions.field_restriction_minimumAge', $min],
            ['>=','matrixcontent_agerestrictions.field_restriction_maximumAge', $max],
            ['between','matrixcontent_agerestrictions.field_restriction_maximumAge', $max, 999]
        ]);
    }

    /**
     * Applies an age filter relation with a min and max age range
     *
     * @param EntryQuery $query Query to be modified
     * @param int $minAge minimum age required
     * @param int $maxAge maximum age required
     * @return EntryQuery
     */
    public function addAgeRestriction(EntryQuery &$physicians, int $min, int $max) 
    {            
        $physicians->orWhere(['between','matrixcontent_agerestrictions.field_restriction_minimumAge', $min, $max]);
        $physicians->orWhere(['between','matrixcontent_agerestrictions.field_restriction_maximumAge', $min, $max]);

        $physicians->orWhere(['and',
            ['<=','matrixcontent_agerestrictions.field_restriction_minimumAge', $min],
            ['>=','matrixcontent_agerestrictions.field_restriction_maximumAge', $max]
        ]);
    }

    /**
     * Applies an ORDER BY clause to the query to sort physicians by their numerical ratings.
     *
     * @param EntryQuery $query Query to be modified
     * @param number $minRating threshold of rating
     * @return EntryQuery
     */
    public function orderPhysiciansByRating(EntryQuery $query, $minRating = 0)
    {
        // get all physician IDs
        $ids = $query->ids();

        // get numerical ratings or all physicians and group them by physician IDs
        // the getProviderRatingAndComments caches the results for a few days,
        // so no significant performance drag should be noticable here
        $ratings = [];
        foreach ($ids as $id) {
            $entry = Entry::find()->id($id)->one();
            $obj = DupageCoreModule::getInstance()
                ->dupageCoreModuleService
                ->getProviderRatingAndComments($entry);
            
            if ($obj === null) {
                continue;
            }

            if (isset($obj['overallRating'])) {
                $overallRating = $obj['overallRating'];
            }

            if (isset($overallRating['value'])) {
                if ($overallRating['value'] >= $minRating) {
                    \array_push(
                        $ratings,
                        [
                            'id' => $id,
                            'rating' => $overallRating['value'] ?? PHP_INT_MIN
                        ]
                    );
                }
            } else {
                // since NPIs are no longer required, a physician could not have a rating
                // if this is the case, for sorting purposes only, the physician then has a rating of 0
                $noNPI = [
                    'id' => $id,
                    'rating' => 0
                ];

                if ($noNPI['rating'] >= $minRating) {
                    \array_push(
                        $ratings,
                        $noNPI
                    );
                };
            }
        }

        // sort the ratings by the numerical values from highest to lowest
        \usort(
            $ratings,
            function ($a, $b) {
                return (float)$a['rating'] < (float)$b['rating'];
            }
        );

        // get physician IDs and preseve the ratings order
        $ids = implode(", ", \array_map(
            function ($a) {
                return $a['id'];
            },
            $ratings
        ));

        $query->id($ids);
    
        if (count($ratings) > 0) {
            // apply the "order by" clause with a specific order of IDs
            $query->orderBy([new Expression("FIELD (elements.id, {$ids})")]);
        }
    }

    /**
     * Adds a new cities and distance query relation
     *
     * @param PhysicianSearchForm $form
     * @param array $relatedTo
     */
    public function relatedToCities(PhysicianSearchForm $form, array &$relatedTo)
    {
        // cities filter
        $citiesQuery = Entry::find()->section('locations');

        $this->appendCitySearchExpressions($citiesQuery, $form);

        \array_push(
            $relatedTo,
            [
                'targetElement' => $citiesQuery,
            ]
        );
    }

    /**
     * Applies city filters and sorts (if applicable) to the query.
     *
     * @param PhysicianSearchForm $form Form that validates and contains supported query parameters
     * @param EntryQuery $query Query to be modified
     * @return EntryQuery
     */
    public function appendCitySearchExpressions(EntryQuery $query, PhysicianSearchForm $form)
    {
        if ($form->city) {
            foreach ($form->city as $index => $city) {
                $expression = new Expression(
                    "JSON_CONTAINS(
                        field_address,
                        :city$index,
                        '$.parts.city'
                        )",
                    [':city' . $index => json_encode($city)]
                );
                $query->orWhere($expression);
            }
        }
    }

    /**
     * Gathers physicians related to a service
     *
     * @param Entry $service
     * @return array
     */
    public function getPhysiciansForService(Entry $service)
    {
        $physicians = Entry::find()
            ->section('physicians')
            ->relatedTo($service)
            ->with([
                'physicianHeadshot',
                'physicianSpeciality'
            ])
            ->orderBY(new Expression(
                'field_lastName IS NULL ASC, field_lastName ASC'
            ));

        return $physicians->all();
    }

    /**
     * Gets providers for each service/procedure category assigned to a service entry 
     *
     * @param Entry $service
     * @return array
     */
    public function getPhysiciansByProcedureForService(Entry $service)
    {
        $physicians = [];
        foreach ($service->servicesProceduresCategories as $category) {
            $procedures = $category->procedures->all();
            array_push($physicians, ...Entry::find()
                ->section('physicians')
                ->relatedTo(['or', $procedures])
                ->with([
                    'physicianHeadshot',
                    'physicianSpeciality'
                ])
                ->orderBY(new Expression(
                    'field_lastName IS NULL ASC, field_lastName ASC'
                )));
        }
        return array_unique($physicians);
    }

    /**
     * Returns a SQL query for calculating the distance (in miles) between two given coordinates.
     * Supplied variables can be either raw coordinate values, or SQL expressions evaluating to a raw coordinate value
     *
     * @param string $p1Lng - Point 1 Longitude
     * @param string $p1Lat - Point 1 Latitude
     * @param string $p2Lng - Point 2 Longitude
     * @param string $p2Lat - Point 2 Latitude
     */
    public function sqlDistanceBetweenTwoGivenPoints($p1Lng, $p1Lat, $p2Lng, $p2Lat)
    {
        return
            "(
                3959 * acos (
                      cos( radians( {$p1Lat} ) )
                    * cos( radians( {$p2Lat} ) )
                    * cos( radians( {$p2Lng} ) - radians({$p1Lng}) )
                    + sin( radians( {$p1Lat} ) )
                    * sin( radians( {$p2Lat} ) )
                )
            )";
    }

    /**
     * Addss a new services query relation
     *
     * @param PhysicianSearchForm $form
     * @param array $relatedTo
     */
    private function relatedToServices(PhysicianSearchForm $form, array &$relatedTo)
    {
        if ($form->service) {
            $servicesQuery = Entry::find()
                ->section('services')
                ->title(DupageCoreModule::getInstance()->dupageCoreModuleService->htmlSpecialCharsDecode($form->service));

            \array_push(
                $relatedTo,
                [
                    'targetElement' => $servicesQuery,
                ]
            );
        }
    }

    /**
     * Adds a new languages query relation
     *
     * @param PhysicianSearchForm $form
     * @param array $relatedTo
     */
    public function relatedToLanguages(PhysicianSearchForm $form, array &$relatedTo)
    {
        if ($form->language) {
            $languagesQuery = Category::find()
                ->category('group')
                ->title(DupageCoreModule::getInstance()->dupageCoreModuleService->htmlSpecialCharsDecode($form->language));

            \array_push(
                $relatedTo,
                [
                    'targetElement' => $languagesQuery,
                ]
            );
        }
    }

    /**
     * Adds a new genders query relation
     *
     * @param PhysicianSearchForm $form
     * @param EntryQuery $physicians
     */
    public function relatedToGenders(PhysicianSearchForm $form, EntryQuery &$physicians)
    {
        if ($form->gender) {
            $physicians->andWhere([
                'content.field_gender' => $form->gender
            ]);
        }
    }

    /**
     * Adds a new LGBTQIA+ query relation
     *
     * @param PhysicianSearchForm $form
     * @param EntryQuery $physicians
     */
    public function relatedToLgbtqiaResource(PhysicianSearchForm $form, EntryQuery &$physicians)
    {
        if ($form->lgbtqia_resource) {
            $physicians->andWhere([
                'content.field_lgbtqiaResource' => true
            ]);
        }
    }

    /**
     * Adds a new hospitals query relation
     *
     * @param PhysicianSearchForm $form
     * @param array $relatedTo
     */
    public function relatedToHospitals(PhysicianSearchForm $form, array &$relatedTo)
    {
        // hospitals filter
        if ($form->hospital) {
            $hospitalsQuery = Entry::find()
                ->section('hospitals')
                ->title($form->hospital);

            \array_push(
                $relatedTo,
                [
                    'targetElement' => $hospitalsQuery,
                ]
            );
        }
    }
    
    /**
     * Returns a collection of locations where given physician offers services.
     *
     * @param Entry $physician Requested physician
     * @return array
     */
    public function getPhysicianLocations(Entry $physician, PhysicianSearchForm $form = null): array
    {
        $locations = Entry::find()
            ->section('locations')
            ->relatedTo($physician);

        if ($form) {
            $this->appendCitySearchExpressions($locations, $form);
        }

        return $locations->all();
    }
    
    /**
     * Returns all cached appointment times for given physician from given date
     *
     * @param Entry $physician
     * @param DateTime $date
     * @return mixed
     */
    public function getCachedAppointmentTimesForPhysicianFromDate($physician, $date, $maxDays = null)
    {
        // get physician ID to look up their cached apointment times
        $providerInfo = SchedulingModule::getInstance()->schedulingModuleEpicService->getProviderIdAndIdType($physician);
        $providerID = $providerInfo['providerID'];

        // appointment times are cached per Y-m-d date string for each day
        $interval = DateInterval::createFromDateString('1 day');

        // check the next X days
        $maxDays = $maxDays ?? 90;
        $endDate = clone $date;
        $endDate->add(new DateInterval("P${maxDays}D"));
        $period = new DatePeriod($date, $interval, $endDate);

        // generate all date-based cache keys so we can grab them all at once from Redis
        $cacheKeys = [];
        foreach ($period as $dt) {
            $dt = $dt->format("Y-m-d");
            $cacheKeys[] = "physician_{$providerID}_appointment_times_rt_{$dt}";
        }

        // get the cached appointment times for given physician
        return Craft::$app->cache->multiGet($cacheKeys);
    }
    
    /**
     * Returns all cached appointment times for given external resource from given date
     *
     * @param int $externalResourceId
     * @param DateTime $date
     * @return mixed
     */
    public function getCachedAppointmentTimesForExternalResourceFromDate($externalResourceId, $date)
    {
        // appointment times are cached per Y-m-d date string for each day
        $interval = DateInterval::createFromDateString('1 day');

        // check the next 90 days
        $endDate = clone $date;
        $endDate->add(new DateInterval("P90D"));
        $period = new DatePeriod($date, $interval, $endDate);

        // generate all date-based cache keys so we can grab them all at once from Redis
        $cacheKeys = [];
        foreach ($period as $dt) {
            $dt = $dt->format("Y-m-d");
            $cacheKeys[] = "physician_{$externalResourceId}_appointment_times_rt_{$dt}";
        }

        // get the cached appointment times for given physician
        return Craft::$app->cache->multiGet($cacheKeys);
    }

    /**
     * Returns a date object representing next available appointment for physician
     *
     * @param Entry $physician
     * @param DateTime $date
     * @return mixed
     */
    public function getAvailablePhysicianAppointmentFromDate(Entry $physician, DateTime $date)
    {
        $cachedAppointmentTimesPerDate = $this->getCachedAppointmentTimesForPhysicianFromDate($physician, $date);

        // find the first available time that is not in the past
        $foundNewDate = false;
        foreach ($cachedAppointmentTimesPerDate as $key => $timeSlots) {
            // continue only if we have appointment time for current key (date), AND we have not already updated the field in this round
            if ($timeSlots && !$foundNewDate) {
                foreach ($timeSlots as $timeSlot) {
                    if ($timeSlot['Time'] >= $date) {
                        return $timeSlot['Time'];
                    }
                }
            }
        }

        // no date found
        return null;
    }

    /**
     * Returns the next available appointment information for given physician, department id and visit type ID from given date.
     *
     * @param Entry $physician
     * @param DateTime $date
     * @return mixed
     */
    public function getNextAppointmentForPhysicianLocationAndVisitTypeIDFromDate($physician, $externalDepartmentId, $visitTypeId, $date)
    {
        $cachedAppointmentTimesPerDate = $this->getCachedAppointmentTimesForPhysicianFromDate($physician, $date);

        $nextAppointmentDate = null;
        foreach ($cachedAppointmentTimesPerDate as $key => $timeSlots) {
            if (!$timeSlots) {
                continue;
            }
            foreach ($timeSlots as $timeSlot) {
                if (
                    (
                        $timeSlot['Department']['ID'] == $externalDepartmentId
                        || $visitTypeId == "2990"
                    )
                    && $timeSlot['VisitType']['ID'] == $visitTypeId
                ) {
                    return $timeSlot;
                }
            }
        }

        // no date found
        return null;
    }

    /**
     * Returns the next available appointment information for given external provider resource ID and visit type ID from given date.
     *
     * @param string $externalDepartmentId
     * @param string $visitTypeId
     * @param DateTime $date
     * @return mixed
     */
    public function getNextAppointmentForExternalResourceLocationAndVisitTypeIDFromDate($externalDepartmentId, $visitTypeId, $date)
    {
        $cachedAppointmentTimesPerDate = $this->getCachedAppointmentTimesForExternalResourceFromDate($externalDepartmentId, $date);

        $nextAppointmentDate = null;
        foreach ($cachedAppointmentTimesPerDate as $key => $timeSlots) {
            if (!$timeSlots) {
                continue;
            }
            foreach ($timeSlots as $timeSlot) {
                if (
                    (
                        $timeSlot['Provider']['ID'] == $externalDepartmentId
                    )
                    && $timeSlot['VisitType']['ID'] == $visitTypeId
                ) {
                    return $timeSlot;
                }
            }
        }

        // no date found
        return null;
    }
}
