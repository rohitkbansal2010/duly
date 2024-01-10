<?php
/**
 * craft3-maintenance plugin for Craft CMS 3.x
 *
 * A maintenance mode plugin for Craft 3 developed by Punchkick Interactive
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\maintenance;

use Craft;
use craft\base\Plugin;

use craft\console\Application as ConsoleApplication;
use craft\events\PluginEvent;
use craft\events\RegisterCpNavItemsEvent;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\events\RegisterUserPermissionsEvent;
use craft\services\Plugins;
use craft\services\UserPermissions;
use craft\web\UrlManager;
use craft\web\View;
use craft\web\twig\variables\Cp;
use punchkick\maintenance\records\MaintenanceRecord;
use punchkick\maintenance\services\MaintenanceService;

use yii\base\Event;

/**
 * Craft plugins are very much like little applications in and of themselves. We’ve made
 * it as simple as we can, but the training wheels are off. A little prior knowledge is
 * going to be required to write a plugin.
 *
 * For the purposes of the plugin docs, we’re going to assume that you know PHP and SQL,
 * as well as some semi-advanced concepts like object-oriented programming and PHP namespaces.
 *
 * https://craftcms.com/docs/plugins/introduction
 *
 * @author    Punchkick Interactive
 * @package   MaintenancePlugin
 * @since     0.0.1
 *
 * @property  MaintenanceService $maintenanceService
 * @property  Settings $settings
 * @method    Settings getSettings()
 */
class MaintenancePlugin extends Plugin
{
    const MAINTENANCE_MODE_BYPASS_PERMISSION = 'maintenanceModeBypassPermissions_pk';
    const MAINTENANCE_MODE_MANAGE_PERMISSION = 'maintenanceModeManagePermissions_pk';

    /**
     * Static property that is an instance of this plugin class so that it can be accessed via
     * Craft3maintenance::$plugin
     *
     * @var instance
     */
    public static $plugin;

    /**
     * To execute your plugin’s migrations, you’ll need to increase its schema version.
     *
     * @var string
     */
    public $schemaVersion = '0.0.1';

    /**
     * Set our $plugin static property to this class so that it can be accessed via
     * Craft3maintenance::$plugin
     *
     * Called after the plugin class is instantiated; do any one-time initialization
     * here such as hooks and events.
     *
     * If you have a '/vendor/autoload.php' file, it will be loaded for you automatically;
     * you do not need to load it in your init() method.
     *
     */
    public function init()
    {
        parent::init();
        self::$plugin = $this;

        // Add in our console commands
        if (Craft::$app instanceof ConsoleApplication) {
            $this->controllerNamespace = 'punchkick\maintenance\console\controllers';
        }

        // Do something after we're installed
        Event::on(
            Plugins::class,
            Plugins::EVENT_AFTER_INSTALL_PLUGIN,
            function (PluginEvent $event) {
                if ($event->plugin === $this) {
                    MaintenanceRecord::loadCurrent();
                }
            }
        );

        Event::on(
            UserPermissions::class,
            UserPermissions::EVENT_REGISTER_PERMISSIONS,
            function (RegisterUserPermissionsEvent $event) {
                $section = Craft::t('craft3-maintenance', 'Maintenance');
                $event->permissions[$section] = [
                    static::MAINTENANCE_MODE_BYPASS_PERMISSION => [
                        'label' => Craft::t('craft3-maintenance', 'Access the site when maintenance mode is enabled.'),
                    ],
                    static::MAINTENANCE_MODE_MANAGE_PERMISSION => [
                        'label' => Craft::t('craft3-maintenance', 'Manage maintenance mode for the site.'),
                    ]
                ];
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_CP_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['craft3-maintenance'] = 'craft3-maintenance/default/index';
            }
        );

        if (Craft::$app->plugins->isPluginInstalled('craft3-maintenance')) {
            $settings = MaintenanceRecord::loadCurrent();
            if ($settings->enabled) {
                if (!Craft::$app->request->isConsoleRequest && !Craft::$app->request->isLivePreview) {
                    $this->maintenanceService->handleMaintenanceMode($settings);
                }
            }
        }

        /**
         * Logging in Craft involves using one of the following methods:
         *
         * Craft::trace(): record a message to trace how a piece of code runs. This is mainly for development use.
         * Craft::info(): record a message that conveys some useful information.
         * Craft::warning(): record a warning message that indicates something unexpected has happened.
         * Craft::error(): record a fatal error that should be investigated as soon as possible.
         *
         * Unless `devMode` is on, only Craft::warning() & Craft::error() will log to `craft/storage/logs/web.log`
         *
         * It's recommended that you pass in the magic constant `__METHOD__` as the second parameter, which sets
         * the category to the method (prefixed with the fully qualified class name) where the constant appears.
         *
         * To enable the Yii debug toolbar, go to your user account in the AdminCP and check the
         * [] Show the debug toolbar on the front end & [] Show the debug toolbar on the Control Panel
         *
         * http://www.yiiframework.com/doc-2.0/guide-runtime-logging.html
         */
        Craft::info(
            Craft::t(
                'craft3-maintenance',
                '{name} plugin loaded',
                ['name' => $this->name]
            ),
            __METHOD__
        );
    }

    public function getSettingsResponse()
    {
        $url = \craft\helpers\UrlHelper::cpUrl('craft3-maintenance');

        return \Craft::$app->controller->redirect($url);
    }
}
