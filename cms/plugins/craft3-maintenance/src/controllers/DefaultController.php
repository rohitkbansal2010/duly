<?php
/**
 * craft3-maintenance plugin for Craft CMS 3.x
 *
 * A maintenance mode plugin for Craft 3 developed by Punchkick Interactive
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\maintenance\controllers;

use Craft;

use craft\web\Controller;

use punchkick\maintenance\MaintenancePlugin;
use punchkick\maintenance\records\MaintenanceRecord;

/**
 * Default Controller
 *
 * @author    Punchkick Interactive
 * @package   MaintenancePlugin
 * @since     0.0.1
 */
class DefaultController extends Controller
{
    /**
     * Handles management of maintenance mode
     *
     * @return Craft\web\Response
     */
    public function actionIndex()
    {
        if (!Craft::$app->user->checkPermission(MaintenancePlugin::MAINTENANCE_MODE_MANAGE_PERMISSION)) {
            Craft::$app->session->setError(Craft::t('craft3-maintenance', 'You do not have permission to manage maintenance mode settings.'));
            Craft::$app->response->redirect('/admin', 302);
        }

        $model = MaintenanceRecord::loadCurrent();

        if (Craft::$app->request->isPost) {
            $model->load(['MaintenanceRecord' => Craft::$app->request->post()]);

            if ($model->save()) {
                Craft::$app->session->setNotice(Craft::t('craft3-maintenance', 'Maintenance settings were changed.'));
            } else {
                Craft::$app->session->setError(Craft::t('craft3-maintenance', 'An error occured when changing the maintenance settings.'));
            }
        }

        return $this->renderTemplate('craft3-maintenance/default', [
            'model' => $model,
            'html' => new \craft\helpers\Html
        ]);
    }
}
