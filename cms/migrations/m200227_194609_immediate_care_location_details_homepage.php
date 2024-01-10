<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\elements\Entry;
use craft\elements\Asset;
use craft\records\Section;
use craft\records\EntryType;
/**
 * m200227_194609_immediate_care_location_details_homepage migration.
 */
class m200227_194609_immediate_care_location_details_homepage extends Migration
{
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

        $fieldValues = [
            'immediateCareLocationDetails' => [
                'new1' => [
                    'type' => 'locationDetail',
                    'enabled' => 1,
                    'fields' => [
                        'headline' => '<p><strong>125+</strong> Locations</p>',
                        'subheadline' => 'ready to serve you!',
                        'buttonText' => 'Browse All Locations',
                        'buttonUrl' => 'https://www.dupagemedicalgroup.com/our-locations/'
                    ]
                ]
            ]
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
        return true;
    }
}
