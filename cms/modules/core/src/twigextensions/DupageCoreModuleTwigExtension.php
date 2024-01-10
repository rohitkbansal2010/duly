<?php

/**
 * punchkick/dupage-core-module module for Craft CMS 3.x
 *
 * Core CMS of functionality for DuPage Medical Group
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace modules\DupageCoreModule\twigextensions;

use BrightEdge;
use Craft;
use craft\base\Element;
use craft\elements\Asset;
use craft\elements\Category;
use craft\elements\db\EntryQuery;
use craft\elements\Entry;
use DateTime;
use DOMDocument;
use IndefiniteArticle\IndefiniteArticle;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\models\Locations;
use Twig\Extension\GlobalsInterface;
use Twig_Extension;
use Twig_SimpleFunction;
use yii\base\Exception;
use yii\base\Model;
use yii\data\ArrayDataProvider;
use yii\db\Expression;
use yii\helpers\Html;
use yii\helpers\Inflector;

/**
 * Twig can be extended in many ways; you can add extra tags, filters, tests, operators,
 * global variables, and functions. You can even extend the parser itself with
 * node visitors.
 *
 * http://twig.sensiolabs.org/doc/advanced.html
 *
 * @author    Punchkick Interactive
 * @package   DupageCoreModule
 * @since     0.0.1
 */
class DupageCoreModuleTwigExtension extends Twig_Extension implements GlobalsInterface
{
    // Public Methods
    // =========================================================================

    /**
     * Returns the name of the extension.
     *
     * @return string The extension name
     */
    public function getName()
    {
        return 'DupageCoreModule';
    }

    /**
     * Returns an array of Twig filters, used in Twig templates via:
     *
     *      {{ 'something' | someFilter }}
     *
     * @return array
     */
    public function getFilters()
    {
        return [];
    }

    public function getGlobals()
    {
        return [
            'inflector' => new Inflector,
            'html' => new Html,
        ];
    }

    /**
     * Returns an array of Twig functions, used in Twig templates via:
     *
     *      {% set this = someFunction('something') %}
     *
     * @return array
     */
    public function getFunctions()
    {
        return [
            new Twig_SimpleFunction('getOptimizedImage', [$this, 'getOptimizedImage']),
            new Twig_SimpleFunction('titleize', [$this, 'titleize']),
            new Twig_SimpleFunction('getProviderRatingAndComments', [$this, 'getProviderRatingAndComments']),
            new Twig_SimpleFunction('entryWithOptimizedImages', [$this, 'entryWithOptimizedImages']),
            new Twig_SimpleFunction('getUserDistance', [$this, 'getUserDistance']),
            new Twig_SimpleFunction('getYearsInPractice', [$this, 'getYearsInPractice']),
            new Twig_SimpleFunction('getWalkInsForLocation', [$this, 'getWalkInsForLocation']),
            new Twig_SimpleFunction('getServicesForLocation', [$this, 'getServicesForLocation']),
            new Twig_SimpleFunction('getPhysicianLocations', [$this, 'getPhysicianLocations']),
            new Twig_SimpleFunction('getPhysiciansLocations', [$this, 'getPhysiciansLocations']),
            new Twig_SimpleFunction('getLocationsForService', [$this, 'getLocationsForService']),
            new Twig_SimpleFunction('sortLocationsByCity', [$this, 'sortLocationsByCity']),
            new Twig_SimpleFunction('getServiceSuiteDetails', [$this, 'getServiceSuiteDetails']),
            new Twig_SimpleFunction('getBrightEdge', [$this, 'getBrightEdge']),
            new Twig_SimpleFunction('indefiniteArticle', [$this, 'indefiniteArticle']),
            new Twig_SimpleFunction('getParticipatingLocationsForSpecial', [$this, 'getParticipatingLocationsForSpecial']),
            new Twig_SimpleFunction('getRelatedProducts', [$this, 'getRelatedProducts']),
            new Twig_SimpleFunction('getAListOfCosmeticProceduresPerLocation', [$this, 'getAListOfCosmeticProceduresPerLocation']),
            new Twig_SimpleFunction('getAListOfCosmeticLocations', [$this, 'getAListOfCosmeticLocations']),
        ];
    }

    public function getBrightEdge()
    {
        $config = [
            BrightEdge\BEIXFClient::$CAPSULE_MODE_CONFIG => BrightEdge\BEIXFClient::$REMOTE_PROD_CAPSULE_MODE,
            BrightEdge\BEIXFClient::$ACCOUNT_ID_CONFIG => "f00000000262861",
            BrightEdge\BEIXFClient::$WHITELIST_PARAMETER_LIST_CONFIG => "ixf"
        ];

        $sdk = new BrightEdge\BEIXFClient($config);

        return $sdk;
    }

    public function sortLocationsByCity(Entry $entry)
    {
        return Entry::find()
            ->section('locations')
            ->relatedTo($entry)
            ->orderBy(new Expression('JSON_EXTRACT(field_address, "$.parts.city")'))
            ->all();
    }

    /**
     * Returns a sorted, unique dataprovider for a single location suite
     *
     * @param string $locationId
     * @param string $serviceId
     * @return mixed
     */
    public function getServiceSuiteDetails(string $locationId, string $serviceId)
    {
        return Locations::getSuiteDetailsForService($locationId, $serviceId);
    }

    /**
     * Returns all locations associated to given Service
     *
     * @param array[Entry[Service]] $service
     * @return ArrayDataProvider
     */
    public function getLocationsForService(Entry $service): array
    {
        // get all suites related to this service
        $locations = Entry::find()
            ->section('locations')
            ->type('suite')
            ->relatedTo([
                'field' => 'suiteServices',
                'targetElement' => $service
            ])
            ->orderBy(new Expression('JSON_EXTRACT(field_address, "$.parts.city")'))
            ->all();

        return $locations;
    }

    /**
     * Returns a sorted, unique dataprovider for a single physician
     *
     * @param Entry $physician
     * @return ArrayDataProvider
     */
    public function getPhysicianLocations(Entry $physician): ArrayDataProvider
    {
        return Locations::getLocationsForPhysician($physician);
    }

    /**
     * Returns a sorted, unique dataprovider for a list of physicians
     *
     * @param array[Entry[Physician]] $physicians
     * @param array[Entry[Location]] $additionalLocations additional locations to return that are not associated to any physicians
     * @return ArrayDataProvider
     */
    public function getPhysiciansLocations(array $physicians = [], array $additionalLocations = []): ArrayDataProvider
    {
        return Locations::getLocationsForPhysicians($physicians, $additionalLocations);
    }

    /**
     * Yii2 slug inflector
     *
     * @param string $slug
     */
    public function titleize($slug)
    {
        return Inflector::slug($slug);
    }

    /**
     * Calculates the years in practice of a physician
     *
     * @param Entry $entry
     * @return number
     */
    public function getYearsInPractice($month = null, string $year = null)
    {
        $now = new DateTime();
        $start = null;

        // months are not required in the CMS, only years
        if ($month && $year) {
            $start = DateTime::createFromFormat('Y-F', "{$year}-{$month}");
        } elseif ($year) {
            $start = DateTime::createFromFormat('Y', $year);
        }

        if (!$start) {
            return null;
        }

        $dateDifference = $now->diff($start);
        $years = $dateDifference->y;

        return $years;
    }

    public function getParticipatingLocationsForSpecial(array $participatingLocations)
    {
        $locations = [];
        foreach ($participatingLocations as $participatingLocation) {
            if (isset($participatingLocation->parent->address->parts->city)) {
                array_push($locations, $participatingLocation->parent->address->parts->city);
                continue;
            } elseif (isset($participatingLocation->address->parts->city)) {
                array_push($locations, $participatingLocation->address->parts->city);
            }
        }
        return array_unique($locations);
    }

    /**
     * Retrieves the Press Ganey ratings and comments for a given provider
     *
     * @param Entry $entry
     * @return float
     */
    public function getProviderRatingAndComments(Entry $entry)
    {
        return DupageCoreModule::getInstance()->dupageCoreModuleService->getProviderRatingAndComments($entry);
    }

    /**
     * Generates clean HTML markup with embeded asset images and videos using getOptimizedImage
     *
     * @param Element $entry
     * @param string $field
     * @return string
     */
    public function entryWithOptimizedImages(Element $entry, $field = 'blogContent')
    {
        return DupageCoreModule::getInstance()->dupageCoreModuleService->entryWithOptimizedImages($entry, $field);
    }

    /**
     * Returns all walk ins associated with location
     *
     * @param Element $location
     * @return array
     */
    public function getWalkInsForLocation(Element $location): array
    {
        $walkIns = [];

        $relatedSuites = Entry::find()
            ->section('locations')
            ->type('suite')
            ->descendantOf($location)
            ->all();

        foreach ($relatedSuites as $suite) {
            $services = $suite->suiteServices->all();

            foreach ($services as $service) {
                if ($service->allowWalkInAppointments) {
                    \array_push($walkIns, $service->serviceType->one()->title);
                }
            }
        }

        \sort($walkIns);

        return $walkIns;
    }

    /**
     * Returns an associated array of services at a physicial location
     * with parent->child relationships established and compressed internal IDs
     *
     * @param Element $location
     * @return array
     */
    public function getServicesForLocation(Element $location): array
    {
        // redis content cleared by DupageCoreModule->clearRedisCacheDataUsingThisLocation(),
        // triggered on Location Entry save action from the CMS
        return Craft::$app->cache->getOrSet("services_for_location_{$location->id}", function () use ($location) {
            $services = [];

            $suites = Entry::find()
                ->section('locations')
                ->type('suite')
                ->descendantOf($location)
                ->all();

            foreach ($suites as $suite) {
                foreach ($suite->suiteServices->all() as $service) {
                    $offering = $service->serviceType->one();
                    if ($offering != null) {
                        if ($offering->level == 1 || $offering->level == 2) {
                            $parent = $offering->getParent();
                            if ($parent != null) {
                                if (!isset($services[$parent->title])) {
                                    $services[$parent->title] = [
                                        'children' => [],
                                        '_suiteIds' => [
                                            $suite->id
                                        ],
                                        '_ids' => [
                                            $parent->id
                                        ]
                                    ];
                                }

                                if (!isset($services[$parent->title]['children'][$offering->title])) {
                                    $services[$parent->title]['children'][$offering->title] = [
                                        '_suiteIds' => [
                                            $suite->id
                                        ],
                                        '_ids' => [
                                            $offering->id
                                        ]
                                    ];
                                } else {
                                    $services[$parent->title]['children'][$offering->title]['_ids'][] = $offering->id;
                                    $services[$parent->title]['children'][$offering->title]['_suiteIds'][] = $suite->id;
                                }
                                $services[$parent->title]['children'][$offering->title]['_ids'] = \array_unique($services[$parent->title]['children'][$offering->title]['_ids']);
                                $services[$parent->title]['children'][$offering->title]['_suiteIds'] = \array_unique($services[$parent->title]['children'][$offering->title]['_suiteIds']);
                                ksort($services[$parent->title]['children']);
                            } else {
                                if (!isset($services[$offering->title])) {
                                    $services[$offering->title] = [
                                        'children' => [],
                                        '_suiteIds' => [
                                            $suite->id
                                        ],
                                        '_ids' => [
                                            $offering->id
                                        ]
                                    ];
                                }

                                $services[$offering->title]['_ids'][] = $offering->id;
                                $services[$offering->title]['_ids'] = \array_unique($services[$offering->title]['_ids']);

                                $services[$offering->title]['_suiteIds'][] = $suite->id;
                                $services[$offering->title]['_suiteIds'] = \array_unique($services[$offering->title]['_suiteIds']);
                            }
                        }
                    }
                }
            }

            ksort($services, SORT_REGULAR);
            return $services;
        }, 3600 * 24);
    }

    /**
     * Generates an optmized image URL for imgproxy using provided settings
     *
     * @param Asset $asset          The Asset
     * @param string $extension     An optional image extension
     * @param bool $asArray         Return the data as an array of DOM string node
     * @param string[] $settings    img proxy settings (defaults to 16:9 ratio)
     * @param string $class         CSS classes to by apply to the img tag
     * @return string|null
     */
    public function getOptimizedImage(
        Asset $asset,
        string $extension = 'webp',
        bool $asArray = false,
        array $settings = [
            ['settings' => ['gravity:sm', 'resize:fill:1200:675:1:1'], 'css' => '(min-width: 1200px)'],
            ['settings' => ['gravity:sm', 'resize:fill:992:558:1:1'], 'css' => '(min-width: 992px)'],
            ['settings' => ['gravity:sm', 'resize:fill:768:432:1:1'], 'css' => '(min-width: 768px)'],
            ['settings' => ['gravity:sm', 'resize:fill:576:324:1:1'], 'css' => '(min-width: 576px)']
        ],
        string $class = null
    ) {
        return DupageCoreModule::getInstance()->dupageCoreModuleService->getOptimizedImage($asset, $extension, $asArray, $settings, $class);
    }

    /**
     * function to generate distance (in miles) between two geolocation points
     *
     * @param number $p1Lng
     * @param number $p1Lat
     * @param number $p2Lng
     * @param number $p2Lat
     */
    public function getUserDistance($p1Lng, $p1Lat, $p2Lng, $p2Lat)
    {
        return 3959 * acos(
            cos($this->radians($p1Lat))
                * cos($this->radians($p2Lat))
                * cos($this->radians($p2Lng) - $this->radians($p1Lng))
                + sin($this->radians($p1Lat))
                * sin($this->radians($p2Lat))
        );
    }

    /**
     * helper function to convert radians from degrees
     *
     * @param number $degrees
     */
    private function radians($degrees)
    {
        return 0.0174532925 * $degrees;
    }

    /**
     * Returns a proper indefinite article followed by the original string.
     *
     * Uses the "zachflower/php-indefinite-article" package.
     *
     * @param string $str - string to be preceded with the article
     */
    public function indefiniteArticle($str)
    {
        return IndefiniteArticle::A($str ?? ' ');
    }

    /**
     * Returns an array of entries related to categories related to the provided entry
     * 
     * @param Entry $product
     * @param int $count - the maximum number of related products to be returned
     */
    public function getRelatedProducts(Entry $product, int $count = 3)
    {
        $related = [];
        $categories = Category::find()
            ->relatedTo($product)
            ->all();
        foreach ($categories as $category) {
            if ($category->products) {
                array_push($related, ...$category->products->id("not {$product->id}")->all());
            }
        }
        if (!count($related)) {
            return null;
        }
        $related = array_unique($related);
        if (count($related) <= $count) {
            return $related;
        }
        $rand = array_rand($related, $count);
        $relatedProducts = array_map(function ($key) use ($related) {
            return $related[$key];
        }, $rand);
        return $relatedProducts;
    }

    public function getAListOfCosmeticProceduresPerLocation()
    {
        $procedures = [];
        $locations = [];

        // find all cosmeticServicesProcedures (group) procedures
        $cosmeticServicesProcedureGroups = Category::find()
            ->group('cosmeticServicesProcedures')
            ->all();

        foreach ($cosmeticServicesProcedureGroups as $cosmeticServicesProcedureGroup) {
            $procedures = \array_merge($procedures, $cosmeticServicesProcedureGroup->procedures->all());
        }

        // get the locations for these procedures
        foreach ($procedures as $procedure) {
            // convert (if needed) locations from suite to physical location
            $procedureLocations = $procedure->physicianLocationsMany->all();
            foreach ($procedureLocations as $procedureLocation) {
                if ($procedureLocation->parent !== null) {
                    $procedureLocation = $procedureLocation->parent;
                }


                // group procedures per location
                $locations[$procedureLocation->address->address] = $locations[$procedureLocation->address->address] ?? [];
                $locations[$procedureLocation->address->address]['location'] = $procedureLocation;
                if (!\in_array($procedure->title, $locations[$procedureLocation->address->address])) {
                    $locations[$procedureLocation->address->address]['procedures'] = $locations[$procedureLocation->address->address]['procedures'] ?? [];
                    $locations[$procedureLocation->address->address]['procedures'][] = $procedure->title;
                }

                sort($locations[$procedureLocation->address->address]['procedures']);
            }
        }

        return $locations;
    }

    public function getAListOfCosmeticLocations()
    {
        $locations = [];
        $cosmeticProceduresPerLocation = $this->getAListOfCosmeticProceduresPerLocation();

        foreach ($cosmeticProceduresPerLocation as $key => $proceduresAndLocations) {
            $locations[] = $proceduresAndLocations['location'];
        }
        
        return $locations;
    }
}
