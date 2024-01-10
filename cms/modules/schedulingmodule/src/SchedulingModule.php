<?php
/**
 * Scheduling module for Craft CMS 3.x
 *
 * Allows for extended management of the scheduling section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\schedulingmodule;

use Craft;
use craft\console\Application as ConsoleApplication;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\events\TemplateEvent;
use craft\i18n\PhpMessageSource;
use craft\web\UrlManager;
use craft\web\View;
use modules\schedulingmodule\twigextensions\SchedulingModuleTwigExtension;
use samdark\log\PsrMessage;
use yii\base\Event;
use yii\base\Module;
use yii\httpclient\Request;

/**
 * Class SchedulingModule
 *
 * @author    Wipfli Digital
 * @package   SchedulingModule
 * @since     1.0.0
 *
 * @property  SchedulingModule $SchedulingModule
 */
class SchedulingModule extends Module
{
    /**
     * @var SchedulingModule
     */
    public static $instance;

    /**
     * @inheritdoc
     */
    public function __construct($id, $parent = null, array $config = [])
    {
        Craft::setAlias('@modules/schedulingmodule', $this->getBasePath());
        $this->controllerNamespace = 'modules\schedulingmodule\controllers';

        // Translation category
        $i18n = Craft::$app->getI18n();
        /** @noinspection UnSafeIsSetOverArrayInspection */
        if (!isset($i18n->translations[$id]) && !isset($i18n->translations[$id.'*'])) {
            $i18n->translations[$id] = [
                'class' => PhpMessageSource::class,
                'sourceLanguage' => 'en-US',
                'basePath' => '@modules/schedulingmodule/translations',
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
            $this->controllerNamespace = 'modules\\schedulingmodule\\console\\controllers';
        }
        self::$instance = $this;

        Craft::$app->view->registerTwigExtension(new SchedulingModuleTwigExtension);

        Event::on(
            Request::class,
            Request::EVENT_AFTER_SEND,
            function (Event $e) {
                Craft::info(new PsrMessage('Request Completed', [
                    'time' => $e->sender->responseTime(),
                    'epic_endpoint' => $e->sender->getUrl()
                ]));
            }
        );
        
        // "main" scheduling flow
        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['schedule'] = 'scheduling-module/default/default';
                $event->rules['schedule/scheduling-from-physician-page'] = 'scheduling-module/default/scheduling-from-physician-page';
                $event->rules['schedule/set-current-patient'] = 'scheduling-module/default/set-current-patient';
                $event->rules['schedule/insurance'] = 'scheduling-module/default/insurance';
                $event->rules['schedule/preliminary'] = 'scheduling-module/default/preliminary';
                $event->rules['schedule/select-appointment'] = 'scheduling-module/default/select-appointment';
                $event->rules['schedule/select-physician'] = 'scheduling-module/default/select-physician';
                $event->rules['schedule/select-location'] = 'scheduling-module/default/select-location';
                $event->rules['schedule/get-locations'] = 'scheduling-module/default/get-locations';
                $event->rules['schedule/physicians/auto-suggestions'] = 'scheduling-module/default/physician-auto-suggestions';
                $event->rules['schedule/visit-reason'] = 'scheduling-module/default/visit-reason';
                $event->rules['schedule/services'] = 'scheduling-module/default/get-services';
                $event->rules['schedule/book'] = 'scheduling-module/default/book';
                $event->rules['schedule/share/email'] = 'scheduling-module/default/share-email';
                $event->rules['schedule/share/calendar'] = 'scheduling-module/default/calendar';
                $event->rules['schedule/book/share'] = 'scheduling-module/default/share';
                $event->rules['schedule/services/auto-suggestions'] = 'scheduling-module/default/auto-suggestions';
                $event->rules['schedule/get-appointment-times'] = 'scheduling-module/default/get-appointment-times';
                $event->rules['schedule/cancel-appointment'] = 'scheduling-module/default/cancel-appointment';
                $event->rules['schedule/deeplink'] = 'scheduling-module/default/schedule-deeplink';
            }
        );

        // video visit scheduling flow
        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['schedule/video-visit'] = 'scheduling-module/video-visit/default';
                $event->rules['schedule/video-visit/select-appointment'] = 'scheduling-module/video-visit/select-appointment';
                $event->rules['schedule/video-visit/patient-info'] = 'scheduling-module/video-visit/patient-info';
                $event->rules['schedule/video-visit/confirm-video-visit'] = 'scheduling-module/video-visit/confirm-video-visit';
            }
        );

        Craft::info(
            Craft::t(
                'scheduling-module',
                '{name} module loaded',
                ['name' => 'Scheduling']
            ),
            __METHOD__
        );
    }
}
