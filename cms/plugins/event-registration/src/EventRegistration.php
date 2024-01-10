<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to easily view event locations and export event registrants to a CSV file.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\eventregistration;

use Craft;
use craft\base\Plugin;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\events\TemplateEvent;
use craft\web\UrlManager;
use craft\web\View;
use modules\DupageCoreModule\DupageCoreModule;
use punchkick\eventregistration\services\EventRegistrationService;
use punchkick\eventregistration\twigextensions\EventRegistrationTwigExtension;
use yii\base\Event;

/**
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 *
 * @property  EventRegistrationServiceService $eventRegistrationService
 */
class EventRegistration extends Plugin
{
    /**
     * Static property that is an instance of this plugin class so that it can be accessed via
     * EventRegistration::$plugin
     *
     * @var EventRegistration
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
        Craft::setAlias('@plugins/EventRegistrationPlugin', $this->getBasePath());
        parent::init();
        self::$plugin = $this;

        // Add in our Twig extensions
        Craft::$app->view->registerTwigExtension(new EventRegistrationTwigExtension);

        $this->registerRoutes();

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
                'event-registration',
                '{name} plugin loaded',
                ['name' => $this->name]
            ),
            __METHOD__
        );
    }

    private function registerRoutes()
    {
        // Registers the event location list route
        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_CP_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['event-registration/<eventId:\d+>'] = 'event-registration/default/index';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_CP_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['GET event-registration/registrations-by-location/<eventId:\d+>/<locationId:\d+>'] = 'event-registration/default/registrations-by-location';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_CP_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['POST event-registration/registrations-by-location/participants-list'] = 'event-registration/default/csv';
            }
        );

        // Registers the event registration endpoint (=> POST /event-registration/register)
        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['/event-registration/register'] = 'event-registration/default/register';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['/events'] = 'event-registration/listing/index';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['/events/auto-suggestions'] = 'event-registration/listing/auto-suggestions';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['/services/youtube-api'] = 'event-registration/service/youtube-api';
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['/events/search'] = 'event-registration/listing/search';
            }
        );

        // Registers the event to add a label to the Solspace calendar event table
        Event::on(
            \Solspace\Calendar\Elements\Event::class,
            \Solspace\Calendar\Elements\Event::EVENT_REGISTER_TABLE_ATTRIBUTES,
            function (craft\events\RegisterElementTableAttributesEvent $e) {
                $e->tableAttributes['locations'] = [
                    'label' => 'Locations'
                ];
            }
        );

        // Registers the event to add a the value to the newly added 'Locations' label in the Solspace calendar event
        // table
        Event::on(
            \Solspace\Calendar\Elements\Event::class,
            \Solspace\Calendar\Elements\Event::EVENT_SET_TABLE_ATTRIBUTE_HTML,
            function (craft\events\SetElementTableAttributeHtmlEvent $e) {
                if ($e->attribute == 'locations') {
                    $e->html =
                        '<a href="/admin/event-registration/' . $e->sender->id . '"><div data-icon="world"></div></a>';
                }
            }
        );

        // Registers the event to update the search index for a given event
        Event::on(
            \Solspace\Calendar\Services\EventsService::class,
            \Solspace\Calendar\Services\EventsService::EVENT_AFTER_SAVE,
            function (\Solspace\Calendar\Events\SaveElementEvent $e) {
                DupageCoreModule::getInstance()->dupageCoreModuleService->updateIndexForEntry($e->getElement());
            }
        );
    }
}
