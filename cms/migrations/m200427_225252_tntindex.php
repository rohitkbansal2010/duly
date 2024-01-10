<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\elements\Entry;
use modules\DupageCoreModule\services\DupageCoreModuleService;

/**
 * m200427_225252_tntindex migration.
 */
class m200427_225252_tntindex extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $tnt = DupageCoreModuleService::getTntSearchStatic();
        $indexer = $tnt->createIndex('dmgtntindex');

        foreach (Entry::find()->section('physicians')->each() as $elem) {
            $indexer->insert(DupageCoreModuleService::serializeFieldValues($elem));
        }

        foreach (Entry::find()->section('locations')->each() as $elem) {
            $indexer->insert(DupageCoreModuleService::serializeFieldValues($elem));
        }

        foreach (Entry::find()->section('blog')->each() as $elem) {
            $indexer->insert(DupageCoreModuleService::serializeFieldValues($elem));
        }

        foreach (Entry::find()->section('hospitals')->each() as $elem) {
            $indexer->insert(DupageCoreModuleService::serializeFieldValues($elem));
        }

        foreach (Entry::find()->section('services')->each() as $elem) {
            $indexer->insert(DupageCoreModuleService::serializeFieldValues($elem));
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        return true;
    }
}
