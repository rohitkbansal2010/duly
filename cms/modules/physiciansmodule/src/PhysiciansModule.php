<?php
/**
 * Physicians module for Craft CMS 3.x
 *
 * Allows for extended management of the physicians section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\physiciansmodule;

use Craft;
use craft\console\Application as ConsoleApplication;
use craft\events\RegisterUrlRulesEvent;
use craft\events\RegisterElementExportersEvent;
use craft\base\Element;
use craft\elements\Entry;
use craft\i18n\PhpMessageSource;
use craft\web\UrlManager;
use modules\physiciansmodule\twigextensions\PhysiciansModuleTwigExtension;
use modules\physiciansmodule\exporters\PhysicianExporter;
use yii\base\Event;
use yii\base\Module;

/**
 * Class PhysiciansModule
 *
 * @author    Wipfli Digital
 * @package   PhysiciansModule
 * @since     1.0.0
 *
 * @property  PhysiciansModuleServiceService $physiciansModuleService
 */
class PhysiciansModule extends Module
{
    /**
     * @var PhysiciansModule
     */
    public static $instance;

    /**
     * @inheritdoc
     */
    public function __construct($id, $parent = null, array $config = [])
    {
        Craft::setAlias('@modules/physiciansmodule', $this->getBasePath());
        $this->controllerNamespace = 'modules\physiciansmodule\controllers';

        // Translation category
        $i18n = Craft::$app->getI18n();
        /** @noinspection UnSafeIsSetOverArrayInspection */
        if (!isset($i18n->translations[$id]) && !isset($i18n->translations[$id.'*'])) {
            $i18n->translations[$id] = [
                'class' => PhpMessageSource::class,
                'sourceLanguage' => 'en-US',
                'basePath' => '@modules/physiciansmodule/translations',
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
         // Set the controllerNamespace based on whether this is a console or web request
        if (Craft::$app instanceof ConsoleApplication) {
            $this->controllerNamespace = 'modules\\physiciansmodule\\console\\controllers';
        }

        self::$instance = $this;

        Craft::$app->view->registerTwigExtension(new PhysiciansModuleTwigExtension);

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['physicians'] = 'physicians-module/default';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['physicians/auto-suggestions'] = 'physicians-module/default/auto-suggestions';
            }
        );

        Craft::info(
            Craft::t(
                'physicians-module',
                '{name} module loaded',
                ['name' => 'Physicians']
            ),
            __METHOD__
        );
    }
}
