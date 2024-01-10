<?php

namespace modules\physiciansmodule\forms;

use Craft;
use DateTime;
use yii\db\Query;
use yii\base\Model;
use yii\db\Expression;
use craft\elements\Entry;
use yii\helpers\Inflector;
use craft\elements\Category;

/**
 * PhysicianSearch Form
 *
 * This form wraps the logic for requesting a list of physicians based on provided parameters.

 */
final class PhysicianSearchForm extends Model
{
    const SORT_PHYSICIANS_BY_DISTANCE = "1";
    const SORT_PHYSICIANS_BY_LAST_NAME_ASC = "2";
    const SORT_PHYSICIANS_BY_LAST_NAME_DESC = "3";
    const SORT_PHYSICIANS_BY_RATING = "4";

    /**
     * @var string $beginDate
     */
    public $beginDate;

    /**
     * @var string $city
     */
    public $city;

    /**
     * @var string $service
     */
    public $service;

    /**
     * @var string $language
     */
    public $language;

    /**
     * @var string $gender
     */
    public $gender;

    /**
     * @var string $age
     */
    public $age;

    /**
     * @var string $min_age
     */
    public $min_age;

    /**
     * @var string $max_age
     */
    public $max_age;

    /**
     * @var string $hospital
     */
    public $hospital;

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
     * @var string $search_physician_attribute
     */
    public $search_physician_attribute;

    /**
     * @var string $address
     */
    public $address;

    /**
     * @var string $limit
     */
    public $limit;

    /**
     * @var string $order_by
     */
    public $order_by;

    /**
     * @var string $availability
     */
    public $availability;

    /**
     * @var number $page
     */
    public $rating;
    
    /**
     * @var number $page
     */
    public $page = 1;

    /**
     * @var boolean $lgbtqia_resource
     */
    public $lgbtqia_resource;

    /**
     * @var number $perPage
     */
    public $perPage = 10;

    /**
     * @var string $dateFormat
     */
    public $dateFormat = "m-d-Y";

    public function __construct()
    {
        $this->beginDate = (new DateTime())->format($this->dateFormat);
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['city', 'service', 'language', 'gender', 'age', 'min_age', 'max_age', 'hospital', 'availability'], 'each', 'rule' => ['string']],
            [['distance', 'latitude', 'longitude', 'rating', 'page', 'perPage'], 'number'],
            [['search_physician_attribute', 'address', 'beginDate', 'order_by'], 'string'],
            [['limit'], 'number'],
            [['lgbtqia_resource'], 'boolean'],
            [['beginDate', 'city', 'service', 'language', 'gender', 'age', 'min_age', 'max_age', 'hospital', 'distance', 'latitude', 'longitude', 'limit', 'search_physician_attribute', 'address', 'order_by', 'rating', 'page', 'perPage', 'availability', 'lgbtqia_resource'], 'safe']
        ];
    }
}
