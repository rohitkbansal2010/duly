<?php

namespace punchkick\requestappointment\migrations;

use Craft;
use craft\db\Migration;

/**
 * m201029_200256_update_datetime_fields migration.
 */
class m201029_200256_update_datetime_fields extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $tableSchema = Craft::$app->db->schema->getTableSchema('{{%requestappointment_requestappointmentrecord}}');
    
        if ($tableSchema !== null) {
            $this->alterColumn('requestappointment_requestappointmentrecord', 'dob', $this->date()->notNull());
            $this->alterColumn('requestappointment_requestappointmentrecord', 'date1', $this->date()->notNull());
            $this->alterColumn('requestappointment_requestappointmentrecord', 'date2', $this->date()->notNull());
            $this->alterColumn('requestappointment_requestappointmentrecord', 'date3', $this->date()->notNull());
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m201029_200256_update_datetime_fields cannot be reverted.\n";
        return false;
    }
}
