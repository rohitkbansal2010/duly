<?php
/**
 * craft3-maintenance plugin for Craft CMS 3.x
 *
 * A maintenance mode plugin for Craft 3 developed by Punchkick Interactive
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\maintenance\services;

use Craft;

use craft\base\Component;
use craft\web\ServiceUnavailableHttpException;
use craft\web\UrlManager;
use punchkick\maintenance\MaintenancePlugin;
use punchkick\maintenance\records\MaintenanceRecord;
use yii\base\Event;

/**
 * MaintenanceService Service
 *
 * All of your plugin’s business logic should go in services, including saving data,
 * retrieving data, etc. They provide APIs that your controllers, template variables,
 * and other plugins can interact with.
 *
 * https://craftcms.com/docs/plugins/services
 *
 * @author    Punchkick Interactive
 * @package   MaintenancePlugin
 * @since     0.0.1
 */
class MaintenanceService extends Component
{
    public function handleMaintenanceMode(MaintenanceRecord $settings)
    {
        if ($this->doesCurrentUserHaveAccess()) {
            return;
        }

        if (Craft::$app->request->isSiteRequest && $settings->enabled) {
            if (Craft::$app->request->url == '/offline') {
                $headers = Craft::$app->response->headers;
                $headers->add('Retry-After', '900');
                $headers->add('Status', 503);
            } else {
                Craft::$app->response->redirect('/offline');
            }
        }
    }

    /**
     * Returns `true` if the curent user has permissions to access the site
     *
     * @return boolean
     */
    private function doesCurrentUserHaveAccess()
    {
        if (Craft::$app->user->getIsAdmin()) {
            return true;
        }

        if (Craft::$app->user->checkPermission(MaintenancePlugin::MAINTENANCE_MODE_BYPASS_PERMISSION)) {
            return true;
        }

        return false;
    }
}
