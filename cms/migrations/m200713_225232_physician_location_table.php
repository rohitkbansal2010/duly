<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;

/**
 * m200713_225232_physician_location_table migration.
 */
class m200713_225232_physician_location_table extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        // Place migration code here...
        $tableSchema = Craft::$app->db->schema->getTableSchema('physicians_locations');
        if ($tableSchema === null) {
            $tablesCreated = true;
            $this->createTable(
                'physicians_locations',
                [
                    'physicianElementId' => $this->integer()->notNull(),
                    'locationElementId' => $this->integer()->notNull()
                ]
            );
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        $this->dropTable('physicians_locations');
    }
}
