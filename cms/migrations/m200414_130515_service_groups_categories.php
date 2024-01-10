<?php

namespace craft\contentmigrations;

use Craft;
use craft\base\Element;
use craft\db\Migration;
use craft\elements\Asset;
use craft\elements\Category;
use craft\elements\Entry;
use craft\Elements\User;
use craft\records\EntryType;
use craft\records\Section;
use yii\helpers\Inflector;

/**
 * m200414_130515_service_groups_categories migration.
 */
class m200414_130515_service_groups_categories extends Migration
{
    private $_entries = [
        [
            'title' => 'Primary Care',
            'image_src' => 'src/img/service_groups/service_group_primary_care.jpg'
        ],
        [
            'title' => 'Multi-Disciplinary Care',
            'image_src' => 'src/img/service_groups/service_group_multi_care.jpg'
        ],
        [
            'title' => 'Specialty Care',
            'image_src' => 'src/img/service_groups/service_group_specialty_care.jpg'
        ],
        [
            'title' => 'Other Services & Clinics',
            'image_src' => 'src/img/service_groups/service_group_other_services.jpg'
        ]
    ];
    
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        // Add the new elements as categories
        $group = Craft::$app->categories->getGroupByHandle('serviceGroups');

        $siteId = Craft::$app->sites->getPrimarySite()->id;

        foreach ($this->_entries as $fields) {
            $model = new Category;

            // Setup the default items
            $model->slug = Inflector::slug($fields['title']);
            $model->title = $fields['title'];
            $model->enabled = true;
            $model->siteId = $siteId;
            $model->groupId = $group->id;

            // Create and build the images
            $v = Craft::$app->volumes->getVolumeByHandle("serviceImage");
            $folders = Craft::$app->assets->getFolderTreeByVolumeIds([$v->id]);
            $folder = $folders[0];
            $asset = new Asset();
            $tmpnme = tempnam('/tmp', 'sg_img_');
            copy(CRAFT_BASE_PATH . '/' . $fields['image_src'], $tmpnme);
            $asset->tempFilePath = $tmpnme;
            $asset->filename = \basename(CRAFT_BASE_PATH . '/' . $fields['image_src']);
            $asset->volumeId = $folder->volumeId;
            $asset->newFolderId = $folder->id;
            $asset->avoidFilenameConflicts = true;
            $asset->enabled = true;
            $asset->setScenario(Asset::SCENARIO_CREATE);
            $result = Craft::$app->getElements()->saveElement($asset);
            $filename = $asset->filename;
            $imageIds = Asset::find()->filename($filename)->ids();

            $model->setFieldValues([
                'serviceHeroImage' => [$imageIds[0]],
            ]);

            $model->setScenario(Element::SCENARIO_LIVE);
            if (!Craft::$app->getElements()->saveElement($model)) {
                return false;
            }
        }        
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m200414_130515_service_groups_categories cannot be reverted.\n";
        return false;
    }
}
