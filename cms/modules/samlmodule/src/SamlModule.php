<?php
/**
 * SAML2 Module for Craft 3 CMS
 *
 * Allows for extended management of the scheduling section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\samlmodule;

use Craft;
use craft\console\Application as ConsoleApplication;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\events\TemplateEvent;
use craft\i18n\PhpMessageSource;
use craft\web\UrlManager;
use craft\web\View;
use modules\samlmodule\twigextensions\samlmoduleTwigExtension;
use samdark\log\PsrMessage;
use yii\base\Event;
use yii\base\Module;
use yii\httpclient\Request;

/**
 * Class SamlModule
 *
 * @author    Wipfli Digital
 * @package   SamlModule
 * @since     1.0.0
 *
 * @property  SamlModule $SamlModule
 */
class SamlModule extends Module
{
    /**
     * @var samlmodule
     */
    public static $instance;

    /**
     * @inheritdoc
     */
    public function __construct($id, $parent = null, array $config = [])
    {
        Craft::setAlias('@modules/samlmodule', $this->getBasePath());
        $this->controllerNamespace = 'modules\samlmodule\controllers';

        // Translation category
        $i18n = Craft::$app->getI18n();
        /** @noinspection UnSafeIsSetOverArrayInspection */
        if (!isset($i18n->translations[$id]) && !isset($i18n->translations[$id.'*'])) {
            $i18n->translations[$id] = [
                'class' => PhpMessageSource::class,
                'sourceLanguage' => 'en-US',
                'basePath' => '@modules/samlmodule/translations',
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
            $this->controllerNamespace = 'modules\\samlmodule\\console\\controllers';
        }
        self::$instance = $this;

    
        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['sml/auth'] = 'saml-module/default/auth';
                $event->rules['sml/acs'] = 'saml-module/default/acs';
                $event->rules['sml/slo'] = 'saml-module/default/slo';
            }
        );

        Craft::info(
            Craft::t(
                'saml-module',
                '{name} module loaded',
                ['name' => 'SAML']
            ),
            __METHOD__
        );
    }
}
