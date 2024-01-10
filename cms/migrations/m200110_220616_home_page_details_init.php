<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\elements\Asset;
use craft\elements\Entry;
use craft\records\EntryType;
use craft\records\Section;
/**
 * m200110_220616_home_page_details_init migration.
 */
class m200110_220616_home_page_details_init extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $section = Section::find()
            ->where(['handle' => 'homePageOverview'])
            ->one();

        $entryType = EntryType::find()
            ->where(['handle' => 'homePageOverview'])
            ->one();

        $entry = Entry::find()
            ->where(['sectionId' => $section->id])
            ->where(['typeId' => $entryType->id])
            ->one();

        $v = Craft::$app->volumes->getVolumeByHandle("heroImage");
        $folders = Craft::$app->assets->getFolderTreeByVolumeIds([$v->id]);
        $folder = $folders[0];
        $asset = new Asset();
        copy(CRAFT_BASE_PATH . '/src/img/home_page_default.png', '/tmp/home_page_default.png');
        $asset->tempFilePath = '/tmp/home_page_default.png';
        $asset->filename = 'home_page_default.png';
        $asset->volumeId = $folder->volumeId;
        $asset->newFolderId = $folder->id;
        $asset->avoidFilenameConflicts = true;
        $asset->enabled = true;
        $asset->setScenario(Asset::SCENARIO_CREATE);
        $result = Craft::$app->getElements()->saveElement($asset);
        $filename = $asset->filename;
        $imageIds = Asset::find()->filename($filename)->ids();

        $fieldValues = [
            'homeHeroImage' => [$imageIds[0]],
            'heroImageText' => 'Physician-Led Care',
            'heroImageSubtext' => 'Personalized Access To',
            'stat' => [
                'new1' => [
                    'type' => 'stat',
                    'enabled' => 1,
                    'fields' => [
                        'headline' => '<p><strong>11,400+</strong> Appointments</p>',
                        'subheadline' => 'when you need them, even on the weekends!',
                        'buttonText' => 'Schedule An Appointment',
                        'buttonUrl' => 'https://www.dupagemedicalgroup.com/'
                    ]
                ],
                'new2' => [
                    'type' => 'stat',
                    'enabled' => 1,
                    'fields' => [
                        'headline' => '<p><strong>790+</strong> Physicians</p>',
                        'subheadline' => 'passionate about your health!',
                        'buttonText' => 'Find A Physician',
                        'buttonUrl' => 'https://www.dupagemedicalgroup.com/'
                    ]
                ],
                'new3' => [
                    'type' => 'stat',
                    'enabled' => 1,
                    'fields' => [
                        'headline' => '<p><strong>0</strong> Miles</p>',
                        'subheadline' => "driven to pick up records, they're all digital",
                        'buttonText' => 'Login To MyChart',
                        'buttonUrl' => 'https://www.dupagemedicalgroup.com/'
                    ]
                ],
            ],
        ];

        $entry->setFieldValues($fieldValues);
        
        if (Craft::$app->elements->saveElement($entry)) {
            return true;
        }

        return false;
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        $section = Craft::$app->sections->getSectionByHandle('homePageOverview');

        $entryType = EntryType::find()
            ->where(['handle' => 'homePageOverview'])
            ->one();

        $entry = Entry::find()
            ->where(['sectionId' => $section->id])
            ->where(['typeId' => $entryType->id])
            ->one();
        
        if (Craft::$app->elements->deleteElement($entry)) {
            return true;
        }

        return false;
    }
}



