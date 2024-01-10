<?php

namespace craft\contentmigrations;

// Craft classes
use Craft;
use craft\db\Migration;

/**
 * m190923_201134_setting_event_table_heading migration.
 */
class m190923_201134_setting_event_table_heading extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        Craft::$app->elementIndexes->saveSettings('Solspace\Calendar\Elements\Event', [
            'sources' => [
                'calendar:1' => [
                    'tableAttributes' => [
                        1 => 'calendar',
                        2 => 'allDay',
                        3 => 'dateCreated',
                        5 => 'locations'
                    ]
                ]
            ]
        ]);
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m190923_201134_setting_event_table_heading cannot be reverted.\n";
        return false;
    }
}
