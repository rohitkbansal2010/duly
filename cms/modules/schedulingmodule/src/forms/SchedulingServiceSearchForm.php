<?php

namespace modules\schedulingmodule\forms;

use yii\base\Model;

/**
 * SchedulingServiceSearchForm Form
 *
 * This form wraps the logic for performing a search based on provided parameters.

 */
final class SchedulingServiceSearchForm extends Model
{
    /**
     * @var string $query
     */
    public $query;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['query'], 'string'],
            [['query'], 'safe']
        ];
    }
}
