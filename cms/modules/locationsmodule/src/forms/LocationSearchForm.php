<?php

namespace modules\locationsmodule\forms;

use Craft;
use craft\elements\Entry;
use modules\locationsmodule\LocationsModule;
use yii\base\Model;
use yii\data\ActiveDataProvider;
use yii\web\UrlManager;

/**
 * LocationSearch Form
 *
 * This form wraps the logic for requesting a list of locations based on provided parameters.

 */
class LocationSearchForm extends Model
{
    /**
     * @var string $care
     */
    public $care;

    /**
     * @var string $city
     */
    public $city;

    /**
     * @var string $service
     */
    public $service;

    /**
     * @var boolean $laboratory_services
     */
    public $laboratory_services;

    /**
     * @var boolean $open_now
     */
    public $open_now;

    /**
     * @var string $distance
     */
    public $distance;

    /**
     * @var string $latitude
     */
    public $latitude;

    /**
     * @var string $longitude
     */
    public $longitude;

    /**
     * @var string $search_service_attribute
     */
    public $search_service_attribute;

    /**
     * @var string $search_service_id
     */
    public $search_service_id;

    /**
     * @var string $search_location
     */
    public $search_location;

    /**
     * @var string $limit
     */
    public $limit;

    /**
     * @var string $reasonForVisitId
     */
    public $reasonForVisitId;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['city', 'service', 'care'], 'each', 'rule' => ['string']],
            [['distance', 'latitude', 'longitude', 'search_service_id'], 'number'],
            [['search_service_attribute', 'search_location'], 'string'],
            [['limit', 'reasonForVisitId'], 'number'],
            [['laboratory_services'], 'boolean'],
            [['open_now'], 'boolean'],
            [['city', 'service', 'care', 'distance', 'latitude', 'longitude', 'limit', 'search_service_attribute', 'search_location', 'search_service_id'], 'safe']
        ];
    }

    public function getDataProvider($paginationPath, $page = null, $pageSize = null)
    {
        $query = LocationsModule::getInstance()
            ->locationsModuleService
            ->queryLocations($this);

        return new ActiveDataProvider([
            'query' => $query,
            'pagination' => [
                'pageSize' => $pageSize ?? 10,
                'page' => $page,
                'totalCount' => $query->count(),
                'urlManager' => new UrlManager([
                    'enablePrettyUrl'=> true,
                    'showScriptName' => false,
                    'rules'=>[
                        '/<p>' => $paginationPath,
                    ]
                ])
            ]
        ]);
    }
}
