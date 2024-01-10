<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Query;
use craft\base\Element;
use craft\db\Migration;
use yii\helpers\Inflector;
use craft\elements\Category;

/**
 * m190912_190705_event_catgories migration.
 */
class m190912_190705_event_catgories extends Migration
{
    private $categories = [
        'Clinics',
        'Events',
        'Lunch & Learns',
        'Medicare Seminars',
        'Physician Speaking Engagements',
        'Support Groups',
        'Therapy Appointments'
    ];

    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $siteId = Craft::$app->sites->getPrimarySite()->id;
        $group = Craft::$app->categories->getGroupByHandle('eventCategories');

        foreach ($this->categories as $category) {
            $model = new Category;
            $model->slug = Inflector::slug($category);
            $model->title = $category;
            $model->enabled = true;
            $model->siteId = $siteId;
            $model->groupId = $group->id;
            $model->setScenario(Element::SCENARIO_LIVE);

            Craft::$app->getElements()->saveElement($model);
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m190912_190705_event_catgories cannot be reverted.\n";
        return false;
    }
}
