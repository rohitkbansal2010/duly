<?php

namespace modules\locationsmodule\forms;

use Craft;
use yii\base\Model;
use craft\elements\Entry;

/**
 * ClosestLocationForm Form
 *
 * This form wraps the logic for requesting a list of locations based on provided parameters.

 */
final class ClosestLocationForm extends Model
{
    /**
     * @var number $latitude
     */
    public $latitude;

    /**
     * @var number $longitude
     */
    public $longitude;

    /**
     * @var number $locationId
     */
    public $locationId;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['latitude', 'longitude'], 'number'],
            [['locationId'], 'validateLocationId'],
            [['latitude', 'longitude'], 'safe']
        ];
    }

    /**
     * Validates location
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateLocationId($attribute, $param)
    {
        $result = Entry::find()
            ->section('locations')
            ->id($param);

        if ($result === NULL) {
            $this->addError('locationId', Craft::t('locations-module', 'The location ID provided is not valid.'));
            return false;
        }

        return true;
    }
}
