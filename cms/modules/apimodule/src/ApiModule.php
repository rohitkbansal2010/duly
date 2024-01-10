<?php
/**
 * API Module for Craft 3 CMS
 *
 * Allows for exposing API endpoints
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2021 Wipfli Digital
 */

namespace modules\apimodule;

use Craft;
use craft\console\Application as ConsoleApplication;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\i18n\PhpMessageSource;
use craft\web\UrlManager;
use craft\web\View;
use yii\base\Event;
use yii\base\Module;
use yii\httpclient\Request;

/**
 * Class ApiModule
 *
 * @author    Wipfli Digital
 * @package   ApiModule
 * @since     1.0.0
 *
 * @property  ApiModule $ApiModule
 */
class ApiModule extends Module
{
    /**
     * @var apimodule
     */
    public static $instance;

    /**
     * @inheritdoc
     */
    public function __construct($id, $parent = null, array $config = [])
    {
        Craft::setAlias('@modules/apimodule', $this->getBasePath());
        $this->controllerNamespace = 'modules\apimodule\controllers';

        // Translation category
        $i18n = Craft::$app->getI18n();
        /** @noinspection UnSafeIsSetOverArrayInspection */
        if (!isset($i18n->translations[$id]) && !isset($i18n->translations[$id.'*'])) {
            $i18n->translations[$id] = [
                'class' => PhpMessageSource::class,
                'sourceLanguage' => 'en-US',
                'basePath' => '@modules/apimodule/translations',
                'forceTranslation' => true,
                'allowOverrides' => true,
            ];
        }

        // Base template directory
        Event::on(View::class, View::EVENT_REGISTER_SITE_TEMPLATE_ROOTS, function (RegisterTemplateRootsEvent $e) {
            if (is_dir($baseDir = $this->getBasePath().DIRECTORY_SEPARATOR.'templates')) {
                $e->roots[$this->id] = $baseDir;
            }
        });

        // Set this as the global instance of this module class
        static::setInstance($this);

        parent::__construct($id, $parent, $config);
    }

    /**
     * @inheritdoc
     */
    public function init()
    {
        parent::init();
        // Set the controllerNamespace based on whether this is a console or web request
        if (Craft::$app instanceof ConsoleApplication) {
            $this->controllerNamespace = 'modules\\apimodule\\console\\controllers';
        }
        self::$instance = $this;

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['api/v1/ping'] = 'api-module/default/ping';
                $event->rules['api/v1/location'] = 'api-module/default/get-location';
                $event->rules['api/v1/hospital'] = 'api-module/default/get-hospital';
                $event->rules['api/v1/physician'] = 'api-module/default/get-physician';
                $event->rules['api/v1/service'] = 'api-module/default/get-service';
                $event->rules['api/v1/telemedicine'] = 'api-module/default/get-telemedicine';
                $event->rules['api/v1/schedule'] = 'api-module/default/schedule';
                $event->rules['api/v1/schedule/token'] = 'api-module/default/generate-scheduling-token';
                $event->rules['api/v1/appointment/'] = 'api-module/default/get-appointment';
            }
        );

        Craft::info(
            Craft::t(
                'api-module',
                '{name} module loaded',
                ['name' => 'API']
            ),
            __METHOD__
        );
    }
}
