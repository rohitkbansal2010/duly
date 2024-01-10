<?php

namespace modules\schedulingmodule\forms;

use yii\base\Model;
use \DateTime;
use craft\elements\Entry;

final class ServiceSelection extends Model
{
    /**
     * @var number $serviceId
     */
    public $serviceId;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['serviceId'], 'each', 'rule' => ['number']],
            ['serviceId', 'validateService'],
            [['serviceId'], 'safe']
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
            ->id($param)
            ->one();

        if ($result === NULL) {
            $this->addError('service', Craft::t('scheduling-module', 'The service ID provided is not valid.'));
            return false;
        }

        return true;
    }
}
