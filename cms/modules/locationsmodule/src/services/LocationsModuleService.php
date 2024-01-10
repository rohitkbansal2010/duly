<?php
/**
 * Locations module for Craft CMS 3.x
 *
 * Allows for extended management of the locations section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\locationsmodule\services;

use Craft;
use craft\base\Component;
use craft\elements\Category;
use craft\elements\db\EntryQuery;
use craft\elements\Entry;
use DateTime;
use ether\simplemap\services\GeoService;
use modules\DupageCoreModule\models\Locations;
use modules\locationsmodule\forms\ClosestLocationForm;
use modules\locationsmodule\forms\LocationSearchForm;
use modules\locationsmodule\LocationsModule;
use modules\DupageCoreModule\DupageCoreModule;
use yii\db\ArrayExpression;
use yii\db\Expression;
use yii\db\Query;

/**
 * @author    Wipfli Digital
 * @package   LocationsModule
 * @since     1.0.0
 */
class LocationsModuleService extends Component
{
    /**
     * Returns an instance of EntryQuery that queries locations with provided form parameters.
     *
     * @param LocationSearchForm $form Form that validates and contains supported query parameters
     * @return EntryQuery
     */
    public function queryLocations(LocationSearchForm $form): EntryQuery
    {
        $physicalLocations = Entry::find()
            ->section('locations')
            ->type('location');

        $suites = Entry::find()
            ->section('locations')
            ->type('suite');

        $parentIds = [];
        $relatedSuites = null;
        $relatedTo = ['and'];

        if ($form->service || $form->laboratory_services) {
            $this->getRelatedServices($form, $relatedTo);
        }

        if ($form->search_service_id) {
            $this->getRelatedServicesById($form, $relatedTo);
        }
        
        //filter out locations if reason for visit is selected during scheduling process
        if ($form->reasonForVisitId) {
            $this->getRelatedReasonForVisitById($form, $relatedTo);
        }

        if ($form->search_service_attribute) {
            $this->getRelatedSearchTerms($form, $relatedTo);
        }

        if ($form->distance) {
            $this->getDistance($physicalLocations, $form);
        }

        // grab all applicable suites
        if (count($relatedTo) > 1) {
            $relatedSuites = $suites->relatedTo($relatedTo)->all();

            if ($relatedSuites) {
                // only show physical locations on front end
                foreach ($relatedSuites as $suite) {
                    $parent = $suite->getParent();

                    if ($parent) {
                        array_push($parentIds, $parent->id);
                    }
                }
            }
            $parentIds = \array_unique($parentIds);
            $physicalLocations->id($parentIds);
        }

        if ($form->open_now) {
            $relatedSuites ? $this->checkIfOpenNow($relatedSuites, $parentIds, $form) : $this->checkIfOpenNow($suites->all(), $parentIds, $form);
            $physicalLocations->id($parentIds);
        }

        if ($form->city) {
            $this->getRelatedCities($physicalLocations, $form);
        }

        if ($form->care) {
            $this->getCareTypes($physicalLocations, $form);
        }

        if ($form->search_location) {
            $coordinates = $this->getLatLngForAddress($form->search_location);
            
            // if user provides a valid location
            if ($coordinates) {
                $longitude = $coordinates['lng'];
                $latitude = $coordinates['lat'];
                
                $range =  new Expression(
                    $this->sqlDistanceBetweenTwoGivenPoints(
                        $longitude,
                        $latitude,
                        "JSON_EXTRACT(field_address, '$.lng')",
                        "JSON_EXTRACT(field_address, '$.lat')"
                    )
                );
    
                $physicalLocations->orderBy($range);
            } else {
                // if user provides a location that is not valid but also allowed for geolocation services
                if ($form->latitude && $form->longitude) {
                    $this->sortByLatLng($physicalLocations, $form);
                } else {
                    // provides a location that is not valid and does not allow for geolocation services
                    $physicalLocations->orderBy('title asc');
                }
            }
        }

        // if geolocation services enabled on the front end, sort by given distance
        if ($form->latitude && $form->longitude && !$form->search_location) {
            $this->sortByLatLng($physicalLocations, $form);
        }

        // if lat/lng are not provided and the user doesn't have a search location value, sort by title
        if (!$form->latitude && !$form->longitude && !$form->search_location) {
            $physicalLocations->orderBy('title asc');
        }

        return $physicalLocations;
    }

    /**
     * Checks to see if a suite has a service with open hours
     *
     * @param array $suites
     * @param array $parentIds
     */
    private function checkIfOpenNow(array $suites, array &$parentIds, LocationSearchForm $form)
    {
        $immediateCareServiceIds = Category::find()->group("immediateCareServices")->one()->assignedService->ids();
        $expressCareServiceIds = Category::find()->group("expressCareServices")->one()->assignedService->ids();
        $labServiceIds = Category::find()->group("laboratoryServices")->one()->assignedService->ids();

        $servicesToCheck = [];
        if (!empty($form->care)) {
            if (in_array('Immediate Care', $form->care)) {
                $servicesToCheck = \array_merge($servicesToCheck, $immediateCareServiceIds);
            }
            if (in_array('Express Care', $form->care)) {
                $servicesToCheck = \array_merge($servicesToCheck, $expressCareServiceIds);
            }
        }
        if ($form->laboratory_services) {
            $servicesToCheck = \array_merge($servicesToCheck, $labServiceIds);
        }

        foreach ($suites as $suite) {
            $services = $suite->suiteServices;
            $parent = $suite->getParent();

            if (!$parent) {
                continue;
            }
     
            if ($services != null) {
                $servicesOffered = $services->all();

                foreach ($servicesOffered as $service) {
                    $serviceType = $service->serviceType->one();
                        
                    if ($serviceType != null) {

                        if (count($servicesToCheck)) {
                            if (!in_array($serviceType->id, $servicesToCheck)) {
                                continue;
                            }
                        }

                        // use logic already created for determining suite hours
                        $details = Locations::determineSuiteDetails($service);

                        // if location has hours assigned, is not closed today, or does not have a specific listed closed date, the suite is considered open
                        // all we care about is one service having open hours
                        // break once found
                        if ($details['closedRightNow'] == false && $details['closedToday'] == false && $details['todayStartTime'] != null && !$details['temporarilyClosed']) {
                            \array_push($parentIds, $parent->id);
                            break;
                        } else {
                            $index = array_search($parent->id, $parentIds);
                            if ($index !== false) {
                                unset($parentIds[$index]);
                            }
                        }
                    }
                }
            }
        }
    }

    /**
     * Applies a services to a query relation
     *
     * @param LocationSearchForm $form
     * @param array $relatedTo
     */
    private function getRelatedSearchTerms(LocationSearchForm $form, array &$relatedTo)
    {
        $serviceQuery = Entry::find()
            ->section('services')
            ->where(['title' => $form->search_service_attribute])
            ->orWhere(['like', 'field_alternativeSearchServiceName', $form->search_service_attribute]);

        if ($serviceQuery) {
            \array_push(
                $relatedTo,
                [
                    'field' => 'suiteServices',
                    'targetElement' => $serviceQuery
                ]
            );
        }
    }
    

    /**
     * Applies orderBy to a query relation
     *
     * @param EntryQuery $physicalLocations
     * @param LocationSearchForm $form
     */
    private function sortByLatLng(EntryQuery $physicalLocations, LocationSearchForm $form)
    {
        $latitude = $form->latitude;
        $longitude = $form->longitude;
        $range =  new Expression(
            $this->sqlDistanceBetweenTwoGivenPoints(
                $longitude,
                $latitude,
                "JSON_EXTRACT(field_address, '$.lng')",
                "JSON_EXTRACT(field_address, '$.lat')"
            )
        );

        $physicalLocations->orderBy($range);
    }

    /**
     * Adds a service relation by service title
     *
     * @param LocationSearchForm $form
     * @param array $relatedTo
     */
    private function getRelatedServices(LocationSearchForm $form, array &$relatedTo)
    {
        $terms = [];

        if ($form->service) {
            $terms = \array_merge($terms, $form->service);
        }

        if ($form->laboratory_services) {
            $laboratoryServiceTitles = $this->getLaboratoryServiceTitles();
            
            if ($laboratoryServiceTitles) {
                $terms = \array_merge($terms, $laboratoryServiceTitles);
            }
        }

        if ($terms) {
            $servicesQuery = Entry::find()
                ->section('services')
                ->title($terms);

            \array_push(
                $relatedTo,
                [
                    'field' => 'suiteServices',
                    'targetElement' => $servicesQuery
                ]
            );
        }
    }

    /**
     * Adds a service relation by service ID
     *
     * @param LocationSearchForm $form
     * @param array $relatedTo
     */
    private function getRelatedServicesById(LocationSearchForm $form, array &$relatedTo)
    {
        $servicesQuery = Entry::find()
            ->section('services')
            ->id($form->search_service_id);

        \array_push(
            $relatedTo,
            [
                'field' => 'suiteServices',
                'targetElement' => $servicesQuery
            ]
        );
    }

    /**
     * Adds a reason for visit relation by service ID
     *
     * @param LocationSearchForm $form
     * @param array $relatedTo
     */
    private function getRelatedReasonForVisitById(LocationSearchForm $form, array &$relatedTo)
    {
        $reasonForVisitQuery = Entry::find()
            ->section('serviceReasonsForVisit')
            ->id($form->reasonForVisitId);

        \array_push(
            $relatedTo,
            [
                'field' => 'serviceReasonForVisitLocations',
                'sourceElement' => $reasonForVisitQuery
            ]
        );
    }

    /**
     * Adds a new care type query relation
     *
     * @param EntryQuery $physicalLocations
     * @param LocationSearchForm $form
     */
    private function getCareTypes(EntryQuery $physicalLocations, LocationSearchForm $form)
    {
        $careTypes = $form->care;

        if (in_array("Immediate Care", $careTypes) && in_array("Express Care", $careTypes)) {
            $physicalLocations->orWhere(['field_immediateCareLocation' => 1]);
            $physicalLocations->orWhere(['field_expressCareLocation' => 1]);
        } elseif (in_array("Immediate Care", $careTypes)) {
            $physicalLocations->immediateCareLocation([1]);
        } elseif (in_array("Express Care", $careTypes)) {
            $physicalLocations->expressCareLocation([1]);
        }
    }

    /**
     * Returns an array of laboratory service titles
     */
    private function getLaboratoryServiceTitles()
    {
        $laboratoryServiceIds = [];
        $serviceTitles = [];

        $laboratoryServiceCategories = Category::find()
            ->group('laboratoryServices')
            ->all();

        // look up all ids for services
        foreach ($laboratoryServiceCategories as $category) {
            if (isset($category->assignedService) && ($category->assignedService->one() != null)) {
                \array_push($laboratoryServiceIds, $category->assignedService->one()->id);
            }
        }

        // create a suite service query with applicable ids
        if ($laboratoryServiceIds) {
            $services = Entry::find()
                ->section('services')
                ->id($laboratoryServiceIds)
                ->all();

            if ($services) {
                foreach ($services as $service) {
                    \array_push($serviceTitles, $service->title);
                }
            }
        }

        return $serviceTitles;
    }

    /**
     * Adds a new city query relation
     *
     * @param EntryQuery $physicalLocations
     * @param LocationSearchForm $form
     */
    private function getRelatedCities(EntryQuery $physicalLocations, LocationSearchForm $form)
    {
        foreach ($form->city as $index => $city) {
            $expression = new Expression(
                "JSON_CONTAINS(
                    field_address,
                    :city$index,
                    '$.parts.city'
                    )",
                [':city' . $index => json_encode($city)]
            );
            $physicalLocations->orWhere($expression);
        }
    }

    /**
     * Adds a new distance query relation
     *
     * @param EntryQuery $physicalLocations
     */
    private function getDistance(EntryQuery $physicalLocations, LocationSearchForm $form)
    {
        $longitude = (float)$form->longitude;
        $latitude = (float)$form->latitude;

        // if address is supplied, it takes precedence over user's location
        if ($form->search_location) {
            $coordinates = $this->getLatLngForAddress($form->search_location);
            
            if ($coordinates) {
                $longitude = $coordinates['lng'];
                $latitude = $coordinates['lat'];
            }
        }

        if (!$longitude || !$latitude) {
            return;
        }

        $distance = $form->distance;

        $physicalLocations->andWhere(new Expression(
            $this->sqlDistanceBetweenTwoGivenPoints(
                $longitude,
                $latitude,
                "JSON_EXTRACT(field_address, '$.lng')",
                "JSON_EXTRACT(field_address, '$.lat')"
            ) . "< {$distance}"
        ));
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
     * Returns first occurance of immediate care / express care service
     *
     * @param array $locationServices
     * @param string $handle
     * @return int|null
     */
    public function getServiceForClosestLocation(array $locationServices, string $handle) :? int
    {
        $serviceCategoriesIds = [];

        $serviceCategories = Category::find()
            ->group($handle)
            ->all();

        // look up all ids for services
        foreach ($serviceCategories as $category) {
            if (isset($category->assignedService) && ($category->assignedService->one() != null)) {
                \array_push($serviceCategoriesIds, $category->assignedService->one()->id);
            }
        }

        if ($serviceCategoriesIds) {
            // look up all services for the location
            // if location service is in category, return it
            foreach ($locationServices as $service) {
                if (isset($service->serviceType)) {
                    if ($service->serviceType->one() != null) {
                        $serviceId = $service->serviceType->one()->id;
                        if (in_array($serviceId, $serviceCategoriesIds)) {
                            return $serviceId;
                        }
                    }
                }                
            }
        }

        return null;
    }

    /**
     * Returns closest Immediate Care location
     *
     * @param form $form
     * @param bool $isImmediateCare
     * @return Object
     */
    public function getClosestLocation(ClosestLocationForm $form, bool $isImmediateCare = false)
    {
        $serviceCategoriesIds = [];

        if ($isImmediateCare) {
            $serviceCategories = Category::find()
                ->group('immediateCareServices')
                ->all();
        } else {
            $serviceCategories = Category::find()
                ->group('expressCareServices')
                ->all();
        }

        // look up all ids for services
        foreach ($serviceCategories as $category) {
            if (isset($category->assignedService) && ($category->assignedService->one() != null)) {
                \array_push($serviceCategoriesIds, $category->assignedService->one()->id);
            }
        }
        
        $servicesQuery = Entry::find()
            ->section('services')
            ->id($serviceCategoriesIds);

        $query = Entry::find()
            ->section('locations')
            ->type('suite')
            ->relatedTo([
                'field' => 'suiteServices',
                'targetElement' => $servicesQuery
            ]);

        if ($form->latitude && $form->longitude) {
            $latitude = $form->latitude;
            $longitude = $form->longitude;
            $range =  new Expression(
                $this->sqlDistanceBetweenTwoGivenPoints(
                    $longitude,
                    $latitude,
                    "JSON_EXTRACT(field_address, '$.lng')",
                    "JSON_EXTRACT(field_address, '$.lat')"
                )
            );

            $query->orderBy($range);
        } else {
            $query->orderBy('title asc');
        }

        return $query->one();
    }

    /**
     * @return array
     */
    public function getLocationDetailPhysicians($form): array
    {
        // start with locationId in the list of valid locations
        $locationIds = [$form->locationId];

        $servicesQuery = Entry::find()
            ->section('services')
            ->id($form->service);

        $physicalLocationId = Entry::find()
            ->section('locations')
            ->type('location')
            ->id($form->locationId)
            ->one();

        $relatedSuites = Entry::find()
            ->section('locations')
            ->type('suite')
            ->descendantOf($physicalLocationId)
            ->all();

        // grab id of suite
        foreach ($relatedSuites as $suite) {
            \array_push($locationIds, $suite->id);
        }

        $allLocations = Entry::find()
            ->section('locations')
            ->where(['in', 'elements.id', $locationIds]);

        $physicians = Entry::find()
            ->section('physicians')
            ->relatedTo(['and',
                ['targetElement' => $servicesQuery],
                ['targetElement' => $allLocations]
            ])
            ->orderBY(new Expression(
                'field_lastName IS NULL ASC, field_lastName ASC'
            ));

        return $physicians->all();
    }

    /**
     * @return Entry
     */
    public function getService($form):? Entry
    {
        return Entry::find()
            ->section('services')
            ->id($form->service)
            ->one();
    }

    /**
     * @return Entry
     */
    public function getLocation($form):? Entry
    {
        return Entry::find()
            ->section('locations')
            ->id($form->locationId)
            ->one();
    }

    /**
     * @return array
     */
    public function getLocationDetailRelatedOffices($form): array
    {
        $servicesQuery = Entry::find()
            ->section('services')
            ->id($form->service);

        // services are only related to suites
        $relatedSuites = Entry::find()
            ->section('locations')
            ->type('suite')
            ->relatedTo([
                'field' => 'suiteServices',
                'targetElement' => $servicesQuery
            ])
            ->orderBy(new Expression('JSON_EXTRACT(field_address, "$.parts.city")'));

        // prevents other suggested locations from being the same location user is already on
        if ($form->locationId) {
            $physicalLocation = Entry::find()
                ->section('locations')
                ->id($form->locationId)
                ->one();

            // grab all suites associated with location
            $sameLocationSuites = Entry::find()
                ->section('locations')
                ->descendantOf($physicalLocation)
                ->ids();

            // we do not want the same location showing multiple times for different suites
            if (!empty($sameLocationSuites)) {
                $idArray = \array_merge(['and'], $sameLocationSuites);
                $idsToExcludeString = join(', not ', $idArray);
                $relatedSuites->id($idsToExcludeString);
            }
        }
                
        return $relatedSuites->all();
    }

    /**
     * Retrieves lat/lnt corodinates for an address string. The coordinates are pulled from cache if cache entry exists.
     *
     * @param string $address
     * @return EntryQuery
     */
    public function getLatLngForAddress(string $address)
    {
        // cache results if not cached already to save API calls
        // one month in seconds
        $duration = 2629746;
        $coordinates = Craft::$app->cache->getOrSet(
            'lat-lng-from-address-' . preg_replace("/[^A-Za-z0-9 ]/", '', $address),
            function () use ($address) {
                return GeoService::latLngFromAddress($address);
            },
            $duration
        );

        if (!$coordinates) {
            return null;
        }

        return [
            'lng' => $coordinates['lng'],
            'lat' => $coordinates['lat']
        ];
    }

    /**
     * Determines if day of week that is passed in from a suite's location is also listed as a specific closed date
     *
     * @param array $allClosedDates
     * @param string $dayOfWeek
     * @return bool
     */
    public function determineClosedDateWithinNextWeek(array $allClosedDates, string $dayOfWeek)
    {
        $singleDate = new DateTime($dayOfWeek);
        $singleDate = $singleDate->format('Y-m-d');

        foreach ($allClosedDates as $date) {
            $dateClosed = $date['datesClosed'];
            if ($dateClosed) {
                $formattedDate = $dateClosed->format('Y-m-d');
                if ($singleDate == $formattedDate) {
                    return true;
                }
            }
        }

        return false;
    }
}
