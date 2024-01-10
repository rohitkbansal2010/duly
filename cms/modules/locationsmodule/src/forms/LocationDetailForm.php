<?php

namespace modules\locationsmodule\forms;

use Craft;
use yii\db\Query;
use yii\base\Model;
use craft\elements\Entry;


/**
 * LocationDetail Form
 *
 * This form wraps the logic for requesting a list of locations based on provided parameters.

 */
final class LocationDetailForm extends Model
{
    /**
     * @var number $service
     */
    public $service;

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
            [['service'], 'number'],
            [['locationId'], 'number'],
            [['service'], 'validateService'],
            [['locationId'], 'validateLocationId'],
            [['service', 'locationId'], 'safe']
        ];
    }

    /**
     * Validates service 
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateService($attribute, $param)
    {
        $result = Entry::find()
            ->section('services')
            ->id($param);

        if ($result === NULL) {
            $this->addError('service', Craft::t('locations-module', 'The service ID provided is not valid.'));
            return false;
        }

        return true;
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
