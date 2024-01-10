<?php

namespace modules\DupageCoreModule\queue;

use Craft;
use craft\elements\Entry;
use craft\queue\BaseJob;
use modules\DupageCoreModule\DupageCoreModule;

/**
 * punchkick/dupage-core-module module for Craft CMS 3.x
 *
 * Facilitates updating the TNT search index through the background job queue as opposed to controller events.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2020 Punchkick Interactive
 *
 * Usage:
 *
 * Craft::$app->queue->push(new \modules\DupageCoreModule\queue\UpdateTNTIndexJob([
 *    'entryId' => '12345'
 * ]));
*/
class UpdateTNTIndexJob extends BaseJob
{
    /**
     * Entry ID of the CMS entry that should be included in the TXT search index
     * @var int $entryId
     */
    public $entryId;

    public function execute($queue)
    {
        try {
            $entry = Entry::find()
                ->id($this->entryId)
                ->one();
            
            // If there's not an entry, abort
            if ($entry === null) {
                return;
            }

            DupageCoreModule::getInstance()
                ->dupageCoreModuleService
                ->updateIndexForEntry($entry);
            
            Craft::info(
                Craft::t(
                    'dupage-core-module',
                    "Updated TNT search index for entry ID {id}",
                    [
                        'id' => $this->entryId
                    ]
                ),
                __METHOD__
            );
        } catch (\Exception $e) {
            Craft::error(
                Craft::t(
                    'dupage-core-module',
                    "Failed updating TNT search index for entry ID {id} with error: {e}",
                    [
                        'id' => $this->entryId,
                        'e' => $e->getMessage()
                    ]
                ),
                __METHOD__
            );
            throw $e;
        }
    }

    protected function defaultDescription()
    {
        return 'Updates the TNT search index for given entry.';
    }
}
