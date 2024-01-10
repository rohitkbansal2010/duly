<?php

namespace craft\contentmigrations;

use Craft;
use craft\base\Element;
use craft\db\Migration;
use craft\db\Query;
use craft\elements\Category;
use yii\helpers\Inflector;

/**
 * m191108_160924_healthtopic_categories migration.
 */
class m191108_160924_healthtopic_categories extends Migration
{
    private $categories = [
        'Addiction',
        'Allergy',
        'Arteries & Veins',
        'Asthma',
        'Autoimmune Diseases',
        'Bladder Health',
        'Cancer',
        'Children\'s Health',
        'Chronic Disease Management',
        'Cold & Flu',
        'Cosmetic & Reconstructive Survery',
        'Diabetes Care',
        'Diagnostic Testing',
        'Digestive Disorders',
        'Ear, Nose, & Throat',
        'Endocrine System Disorder',
        'Eye Care',
        'Fitness & Exercise',
        'Foot & Ankle Problems',
        'General Health',
        'Health & Diet',
        'Hearing Disorders',
        'Heart Health',
        'Infant Care',
        'Infectious Disease',
        'Injury & Pain Management',
        'Joints & Tissue',
        'Kidney Disease',
        'Men\'s Health',
        'Mental Health',
        'Physical & Occupational Therapy',
        'Pregnancy',
        'Radiology',
        'Respiratory Problems',
        'Senior\'s Health',
        'Skin Health',
        'Sleep',
        'Spine & Back Problems',
        'Women\'s Health'
    ];

    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $siteId = Craft::$app->sites->getPrimarySite()->id;
        $group = Craft::$app->categories->getGroupByHandle('healthTopicCategories');

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
        echo "m191108_160924_healthtopic_categories cannot be reverted.\n";
        return false;
    }
}

