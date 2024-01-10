<?php
/**
 * craft3-maintenance plugin for Craft CMS 3.x
 *
 * A maintenance mode plugin for Craft 3 developed by Punchkick Interactive
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\maintenance\console\controllers;

use Craft;

use punchkick\maintenance\MaintenancePlugin;
use punchkick\maintenance\records\MaintenanceRecord;
use yii\console\Controller;
use yii\console\widgets\Table;
use yii\helpers\Console;

/**
 * Console commands to enable management of maintenance functionality
 *
 * @author    Punchkick Interactive
 * @package   MaintenancePlugin
 * @since     0.0.1
 */
class DefaultController extends Controller
{
    /**
     * Displays a list of sites and their current status
     * @param int $siteId
     * @return void
     */
    public function actionIndex()
    {
        $table = new Table;
        $rows = [];
        foreach (Craft::$app->sites->allSites as $site) {
            $model = MaintenanceRecord::loadSite($site->id);
            $rows[] = [
                $site->id,
                $site->name,
                $model->enabled === 1 ? Craft::t('craft3-maintenance', 'Enabled') : Craft::t('craft3-maintenance', 'Disabled'),
                $site->primary ? Craft::t('craft3-maintenance', 'Yes') : Craft::t('craft3-maintenance', 'No')
            ];
        }

        echo $table->setHeaders([
                Craft::t('craft3-maintenance', 'Site ID'),
                Craft::t('craft3-maintenance', 'Site Name'),
                Craft::t('craft3-maintenance', 'Maintenance Status'),
                Craft::t('craft3-maintenance', 'Is Primary?')
            ])
            ->setRows($rows)
            ->run();
    }

    /**
     * Disables maintenance mode for a given site ID
     *
     * @param int $siteId
     * @return void
     */
    public function actionDisableForSite($siteId)
    {
        $model = MaintenanceRecord::loadSite($siteId);
        $model->disable();
    }

    /**
     * Enables maintenance mode for a given site ID.
     *
     * @param int $siteId
     * @return void
     */
    public function actionEnableForSite($siteId)
    {
        $model = MaintenanceRecord::loadSite($siteId);
        $model->enable();
    }
}
