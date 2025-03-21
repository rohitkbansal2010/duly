<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\eventregistration\migrations;

use punchkick\eventregistration\EventRegistration;

use Craft;
use craft\config\DbConfig;
use craft\db\Migration;

/**
 * Event Registration Install Migration
 *
 * If your plugin needs to create any custom database tables when it gets installed,
 * create a migrations/ folder within your plugin folder, and save an Install.php file
 * within it using the following template:
 *
 * If you need to perform any additional actions on install/uninstall, override the
 * safeUp() and safeDown() methods.
 *
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 */
class Install extends Migration
{
    // Public Properties
    // =========================================================================

    /**
     * @var string The database driver to use
     */
    public $driver;

    // Public Methods
    // =========================================================================

    /**
     * This method contains the logic to be executed when applying this migration.
     * This method differs from [[up()]] in that the DB logic implemented here will
     * be enclosed within a DB transaction.
     * Child classes may implement this method instead of [[up()]] if the DB logic
     * needs to be within a transaction.
     *
     * @return boolean return a false value to indicate the migration fails
     * and should not proceed further. All other return values mean the migration succeeds.
     */
    public function safeUp()
    {
        $this->driver = Craft::$app->getConfig()->getDb()->driver;
        if ($this->createTables()) {
            $this->createIndexes();
            $this->addForeignKeys();
            // Refresh the db schema caches
            Craft::$app->db->schema->refresh();
            $this->insertDefaultData();
        }

        return true;
    }

    /**
     * This method contains the logic to be executed when removing this migration.
     * This method differs from [[down()]] in that the DB logic implemented here will
     * be enclosed within a DB transaction.
     * Child classes may implement this method instead of [[down()]] if the DB logic
     * needs to be within a transaction.
     *
     * @return boolean return a false value to indicate the migration fails
     * and should not proceed further. All other return values mean the migration succeeds.
     */
    public function safeDown()
    {
        $this->driver = Craft::$app->getConfig()->getDb()->driver;
        $this->removeTables();

        return true;
    }

    // Protected Methods
    // =========================================================================

    /**
     * Creates the tables needed for the Records used by the plugin
     *
     * @return bool
     */
    protected function createTables()
    {
        $tablesCreated = false;

        $tableSchema = Craft::$app->db->schema->getTableSchema('{{%eventregistration_eventregistrationrecord}}');
        if ($tableSchema === null) {
            $tablesCreated = true;
            $this->createTable(
                '{{%eventregistration_eventregistrationrecord}}',
                [
                    'id' => $this->primaryKey(),
                    'siteId' => $this->integer()->notNull(),
                    'dateCreated' => $this->dateTime()->notNull(),
                    'dateUpdated' => $this->dateTime()->notNull(),
                    'eventId' => $this->integer()->notNull(),
                    'locationId' => $this->integer()->notNull(),
                    'occurrenceDate' => $this->dateTime()->notNull(),
                    'firstName' => $this->string()->notNull(),
                    'lastName' => $this->string()->notNull(),
                    'phoneNumber' => $this->string()->notNull(),
                    'emailAddress' => $this->string()->notNull(),
                    'marketingOptin' => $this->boolean()->notNull(),
                    'uid' => $this->uid(),
                ]
            );
        }

        return $tablesCreated;
    }

    /**
     * Creates the indexes needed for the Records used by the plugin
     *
     * @return void
     */
    protected function createIndexes()
    {
        // Additional commands depending on the db driver
        switch ($this->driver) {
            case DbConfig::DRIVER_MYSQL:
                break;
            case DbConfig::DRIVER_PGSQL:
                break;
        }
    }

    /**
     * Creates the foreign keys needed for the Records used by the plugin
     *
     * @return void
     */
    protected function addForeignKeys()
    {
    // eventregistration_eventregistrationrecord table
        $this->addForeignKey(
            $this->db->getForeignKeyName('{{%eventregistration_eventregistrationrecord}}', 'siteId'),
            '{{%eventregistration_eventregistrationrecord}}',
            'siteId',
            '{{%sites}}',
            'id',
            'CASCADE',
            'CASCADE'
        );

        $this->addForeignKey(
            $this->db->getForeignKeyName('{{%eventregistration_eventregistrationrecord}}', 'eventId'),
            '{{%eventregistration_eventregistrationrecord}}',
            'eventId',
            'calendar_events',
            'id',
            'CASCADE',
            'CASCADE'
        );

        $this->addForeignKey(
            $this->db->getForeignKeyName('{{%eventregistration_eventregistrationrecord}}', 'locationId'),
            '{{%eventregistration_eventregistrationrecord}}',
            'locationId',
            'matrixcontent_eventlocation',
            'elementId',
            'CASCADE',
            'CASCADE'
        );
    }

    /**
     * Populates the DB with the default data.
     *
     * @return void
     */
    protected function insertDefaultData()
    {
    }

    /**
     * Removes the tables needed for the Records used by the plugin
     *
     * @return void
     */
    protected function removeTables()
    {
    // eventregistration_eventregistrationrecord table
        $this->dropTableIfExists('{{%eventregistration_eventregistrationrecord}}');
    }
}
