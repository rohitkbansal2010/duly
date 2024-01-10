<?php
/**
 * Locations module for Craft CMS 3.x
 *
 * Allows for extended management of the locations section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\locationsmodule;

use Craft;
use yii\base\Event;
use yii\base\Module;
use craft\web\UrlManager;
use craft\i18n\PhpMessageSource;
use craft\events\RegisterUrlRulesEvent;
use modules\locationsmodule\twigextensions\LocationsModuleTwigExtension;

/**
 * Class LocationsModule
 *
 * @author    Wipfli Digital
 * @package   LocationsModule
 * @since     1.0.0
 *
 * @property  LocationsModuleServiceService $locationsModuleService
 */
class LocationsModule extends Module
{
    /**
     * @var LocationsModule
     */
    public static $instance;

    /**
     * @inheritdoc
     */
    public function __construct($id, $parent = null, array $config = [])
    {
        Craft::setAlias('@modules/locationsmodule', $this->getBasePath());
        $this->controllerNamespace = 'modules\locationsmodule\controllers';

        // Translation category
        $i18n = Craft::$app->getI18n();
        /** @noinspection UnSafeIsSetOverArrayInspection */
        if (!isset($i18n->translations[$id]) && !isset($i18n->translations[$id.'*'])) {
            $i18n->translations[$id] = [
                'class' => PhpMessageSource::class,
                'sourceLanguage' => 'en-US',
                'basePath' => '@modules/locationsmodule/translations',
                'forceTranslation' => true,
                'allowOverrides' => true,
            ];
        }

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
        self::$instance = $this;

        Craft::$app->view->registerTwigExtension(new LocationsModuleTwigExtension);

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['locations'] = 'locations-module/default';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['locations/auto-suggestions'] = 'locations-module/default/auto-suggestions';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['locations/detail'] = 'locations-module/default/get-location-details';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['locations/distance'] = 'locations-module/default/get-location-distance';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['locations/closest'] = 'locations-module/default/get-closest-locations';
            }
        );

        Craft::info(
            Craft::t(
                'locations-module',
                '{name} module loaded',
                ['name' => 'Locations']
            ),
            __METHOD__
        );
    }
}
