<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\elements\Asset;
use craft\elements\Entry;
use craft\records\EntryType;
use craft\records\Section;

/**
 * m200309_193112_immediate_care_details_init migration.
 */
class m200309_193112_immediate_care_details_init extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $section = Section::find()
            ->where(['handle' => 'immediateCare'])
            ->one();

        $entryType = EntryType::find()
            ->where(['handle' => 'immediateCare'])
            ->one();

        $entry = Entry::find()
            ->where(['sectionId' => $section->id])
            ->where(['typeId' => $entryType->id])
            ->one();

        $v = Craft::$app->volumes->getVolumeByHandle("heroImage");
        $folders = Craft::$app->assets->getFolderTreeByVolumeIds([$v->id]);
        $folder = $folders[0];
        $asset = new Asset();
        copy(CRAFT_BASE_PATH . '/src/img/immediate_care_page_hero_default.png', '/tmp/immediate_care_page_hero_default.png');
        $asset->tempFilePath = '/tmp/immediate_care_page_hero_default.png';
        $asset->filename = 'immediate_care_page_hero_default.png';
        $asset->volumeId = $folder->volumeId;
        $asset->newFolderId = $folder->id;
        $asset->avoidFilenameConflicts = true;
        $asset->enabled = true;
        $asset->setScenario(Asset::SCENARIO_CREATE);
        $result = Craft::$app->getElements()->saveElement($asset);
        $filename = $asset->filename;
        $imageIds = Asset::find()->filename($filename)->ids();

        $fieldValues = [
            'heroImage' => [$imageIds[0]],
            'expressCarePrice' => '70',
            'expressCareDescription' => 'Walk-in or schedule an appointment for physicals, illness and minor injury.',
            'immediateCarePrice' => '120',
            'immediateCareDescription' => 'Staffed by board-certified emergency medicine physicians for urgent illness or injury.',
            'emergencyRoomPrice' => '2300',
            'emergencyRoomDescription' => 'For patients experiencing life-threatening medical emergencies and need urgent care.',
            'servicesPriceDisclaimer' => '*Cost varies based on your insurance coverage'
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
        $section = Craft::$app->sections->getSectionByHandle('immediateCare');

        $entryType = EntryType::find()
            ->where(['handle' => 'immediateCare'])
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