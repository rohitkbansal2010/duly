<?php

namespace modules\schedulingmodule\forms;

use yii\base\Model;
use \DateTime;
use craft\elements\Entry;

final class CalendarForm extends Model
{
    /**
     * @var string $startTime
     */
    public $startTime;

    /**
     * @var string $location
     */
    public $location;

    /**
     * @var string $description
     */
    public $description;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['location', 'description'], 'string'],
            ['startTime', 'date', 'format' => 'php:D, F j, Y g:ia'],
            [['startTime', 'location', 'description'], 'safe']
        ];
    }
}
