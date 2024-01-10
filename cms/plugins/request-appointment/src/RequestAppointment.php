<?php
/**
 * Request Appointment plugin for Craft CMS 3.3
 *
 * Allows for extended management of submitting request form for appointment.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\requestappointment;

use Craft;
use DateTime;
use craft\base\Plugin;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\events\TemplateEvent;
use craft\web\UrlManager;
use craft\web\View;
use gftp\FtpComponent;
use punchkick\requestappointment\records\RequestAppointmentRecord;
use punchkick\requestappointment\twigextensions\RequestAppointmentTwigExtension;
use yii\base\Event;

/**
 * @author    Punchkick Interactive
 * @package   Request Appointment
 * @since     0.0.1
 *
 * @property  RequestAppointmentService $requestAppointmentService
 */
class RequestAppointment extends Plugin
{
    /**
     * Static property that is an instance of this plugin class so that it can be accessed via
     * RequestAppointment::$plugin
     *
     * @var RequestAppointment
     */
    public static $plugin;

    /**
     * To execute your plugin’s migrations, you’ll need to increase its schema version.
     *
     * @var string
     */
    public $schemaVersion = '0.1.0';

    /**
     * Set our $plugin static property to this class so that it can be accessed via
     * EventRegistration::$plugin
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
        Craft::setAlias('@plugins/RequestAppointmentPlugin', $this->getBasePath());
        parent::init();

        $config = require_once __DIR__ . '/config.php';
        if (\file_exists(Craft::getAlias('@config/request-appointment.php'))) {
            $overwrite = Craft::getAlias('@config/request-appointment.php');
            $config = ArrayHelper::merge($config, $overwrite);
        }

        $this->setComponents($config['components']);

        self::$plugin = $this;

        // Add in our Twig extensions
        Craft::$app->view->registerTwigExtension(new RequestAppointmentTwigExtension);

        $this->registerRoutes();
    }

    private function registerRoutes()
    {
        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['schedule/physical-occupational-therapy'] = 'request-appointment/default';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['schedule/cosmetic-dermatology'] = 'request-appointment/default';
            }
        );
    }
}
