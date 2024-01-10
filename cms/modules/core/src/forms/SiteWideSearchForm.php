<?php

namespace modules\DupageCoreModule\forms;

use yii\base\Model;

/**
 * SiteWideSearchForm Form
 *
 * This form wraps the logic for performing a site-wide search based on provided parameters.

 */
final class SiteWideSearchForm extends Model
{
    /**
     * @var string $query
     */
    public $query;

    /**
     * @var string $section
     */
    public $section;

    /**
     * @var string $page
     */
    public $page = 1;

    /**
     * @var string $perPage
     */
    public $perPage = 10;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['query', 'section'], 'string'],
            [['page', 'perPage'], 'number'],
            [['query', 'section', 'page', 'perPage'], 'safe']
        ];
    }
}
