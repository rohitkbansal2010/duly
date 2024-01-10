<?php

/**
 * punchkick/dupage-core-module module for Craft CMS 3.x
 *
 * Core CMS of functionality for DuPage Medical Group
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace modules\DupageCoreModule;

use Craft;
use craft\base\Element;
use craft\base\Field;
use craft\elements\Category;
use craft\elements\Entry;
use craft\events\ElementEvent;
use craft\events\ModelEvent;
use craft\events\RegisterTemplateRootsEvent;
use craft\events\RegisterUrlRulesEvent;
use craft\events\TemplateEvent;
use craft\helpers\ElementHelper;
use craft\i18n\PhpMessageSource;
use craft\services\Elements;
use craft\web\Response;
use craft\web\twig\variables\CraftVariable;
use craft\web\UrlManager;
use craft\web\View;
use Datetime;
use modules\DupageCoreModule\assetbundles\DupageCoreModule\DupageCoreModuleAsset;
use modules\DupageCoreModule\queue\UpdateTNTIndexJob;
use modules\DupageCoreModule\twigextensions\DupageCoreModuleTwigExtension;
use modules\DupageCoreModule\variables\DupageCoreModuleVariable;
use modules\physiciansmodule\controllers\tntsearch\TNTSearchDMG;
use samdark\log\PsrMessage;
use TeamTNT\TNTSearch\Connectors\MySqlConnector;
use yii\base\Event;
use yii\base\InvalidConfigException;

use yii\base\Module;
use yii\db\Query;

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
 * @package   DupageCoreModule
 * @since     0.0.1
 *
 */
class DupageCoreModule extends Module
{
    // Static Properties
    // =========================================================================

    /**
     * Static property that is an instance of this module class so that it can be accessed via
     * DupageCoreModule::$instance
     *
     * @var DupageCoreModule
     */
    public static $instance;

    // Public Methods
    // =========================================================================

    /**
     * @inheritdoc
     */
    public function __construct($id, $parent = null, array $config = [])
    {
        Craft::setAlias('@modules/DupageCoreModule', $this->getBasePath());
        $this->controllerNamespace = 'modules\DupageCoreModule\controllers';

        // Translation category
        $i18n = Craft::$app->getI18n();
        /** @noinspection UnSafeIsSetOverArrayInspection */
        if (!isset($i18n->translations[$id]) && !isset($i18n->translations[$id . '*'])) {
            $i18n->translations[$id] = [
                'class' => PhpMessageSource::class,
                'sourceLanguage' => 'en-US',
                'basePath' => '@modules/DupageCoreModule/translations',
                'forceTranslation' => true,
                'allowOverrides' => true,
            ];
        }

        // Base template directory
        Event::on(View::class, View::EVENT_REGISTER_CP_TEMPLATE_ROOTS, function (RegisterTemplateRootsEvent $e) {
            if (is_dir($baseDir = $this->getBasePath() . DIRECTORY_SEPARATOR . 'templates')) {
                $e->roots[$this->id] = $baseDir;
            }
        });

        Event::on(View::class, View::EVENT_REGISTER_SITE_TEMPLATE_ROOTS, function (RegisterTemplateRootsEvent $event) {
            $event->roots[] = CRAFT_BASE_PATH . DIRECTORY_SEPARATOR . 'templates';
        });

        // Set this as the global instance of this module class
        static::setInstance($this);

        parent::__construct($id, $parent, $config);
    }

    /**
     * Set our $instance static property to this class so that it can be accessed via
     * DupageCoreModule::$instance
     *
     * Called after the module class is instantiated; do any one-time initialization
     * here such as hooks and events.
     *
     * If you have a '/vendor/autoload.php' file, it will be loaded for you automatically;
     * you do not need to load it in your init() method.
     */
    public function init()
    {
        parent::init();
        // Set the controllerNamespace based on whether this is a console or web request
        if (Craft::$app->getRequest()->getIsConsoleRequest()) {
            $this->controllerNamespace = 'modules\\DupageCoreModule\\console\\controllers';
        }
        self::$instance = $this;

        // Load our AssetBundle
        if (Craft::$app->getRequest()->getIsCpRequest()) {
            Event::on(
                View::class,
                View::EVENT_BEFORE_RENDER_TEMPLATE,
                function (TemplateEvent $event) {
                    try {
                        Craft::$app->getView()->registerAssetBundle(DupageCoreModuleAsset::class);
                    } catch (InvalidConfigException $e) {
                        Craft::error(
                            'Error registering AssetBundle - ' . $e->getMessage(),
                            __METHOD__
                        );
                    }
                }
            );
        }

        // Add in our Twig extensions
        Craft::$app->view->registerTwigExtension(new DupageCoreModuleTwigExtension);

        $this->eventAfterSaveElement();

        // Measure the page render time
        Event::on(
            Response::class,
            Response::EVENT_AFTER_SEND,
            function (Event $event) {
                Craft::info(new PsrMessage('Page Response Sent', [
                    'time' => microtime(true) - YII_BEGIN_TIME,
                    'url' => Craft::$app->request->url,
                    'method' => Craft::$app->request->method,
                    'status' => $event->sender->statusCode,
                    'authenticated' => Craft::$app->patient_user->identity !== null,
                    'admin_authenticated' => Craft::$app->user->identity !== null
                ]));
            }
        );

        Event::on(
            Elements::class,
            Elements::EVENT_AFTER_DELETE_ELEMENT,
            function (ElementEvent $event) {
                $physicianType = Craft::$app->sections->getSectionByHandle('physicians');
                if (!$event->element->getIsRevision() && isset($event->element->sectionId) && ($physicianType->id === $event->element->sectionId)) {
                    $this->updatePhysiciansLocationsTable($event->element, false);
                }
            }
        );

        Event::on(
            Entry::class,
            Entry::EVENT_BEFORE_SAVE,
            function (ModelEvent $event) {
                $entry = $event->sender;
                if (isset($entry->locationSpecificAlert->id)) {
                    $location = $entry->locationSpecificAlert->id;
                    
                    if ($location != null && !ElementHelper::isDraftOrRevision($entry)) {
                        $locationId = $location[0];

                        $alert = Entry::find()
                            ->id(['not', $entry->id])
                            ->section('alerts')
                            ->type('locationSpecific')
                            ->relatedTo([
                                'targetElement' => $locationId
                            ])
                            ->one();

                        if ($alert) {
                            $entry->addError('locationSpecificAlert', 'This location already has an alert assigned it.');
                            $event->isValid = false;
                        }
                    }
                }
            }
        );

        // Register our variables
        Event::on(
            CraftVariable::class,
            CraftVariable::EVENT_INIT,
            function (Event $event) {
                /** @var CraftVariable $variable */
                $variable = $event->sender;
                $variable->set('DupageCoreModule', DupageCoreModuleVariable::class);
            }
        );

        Event::on(
            UrlManager::class,
            UrlManager::EVENT_REGISTER_SITE_URL_RULES,
            function (RegisterUrlRulesEvent $event) {
                $event->rules['search'] = 'dupage-core-module/default/search';
                $event->rules['search/auto-suggestions'] = 'dupage-core-module/default/auto-suggestions';
                $event->rules['oauth2/redirect'] = 'dupage-core-module/default/oauth2-redirect';
                $event->rules['log-out'] = 'dupage-core-module/default/logout';
                $event->rules['check-symptoms'] = 'dupage-core-module/default/check-symptoms';
                $event->rules['contact-us'] = 'dupage-core-module/default/contact-us';
                $event->rules['login-customer'] = 'dupage-core-module/default/login-plain';
                $event->rules['login-portal'] = 'dupage-core-module/default/login';
                $event->rules['version'] = 'dupage-core-module/default/version';
                $event->rules['deeplink-redirect/<id>'] = 'dupage-core-module/default/deeplink-redirect';
                $event->rules['cosmetic-dermatology-products'] = 'dupage-core-module/default/products';
            }
        );

        Event::on(
            Entry::class,
            Element::EVENT_SET_TABLE_ATTRIBUTE_HTML,
            function (\craft\events\SetElementTableAttributeHtmlEvent $e) {
                $fieldId = (int)\str_replace('field:', '', $e->attribute);
                $field = Craft::$app->fields->getFieldById($fieldId);
                if ($field != null && $field->handle = 'parentFieldRef') {
                    $entry = Entry::find()
                        ->id($e->sender->id)
                        ->one();
                    if ($entry !== null && $entry->parent !== null) {
                        $e->html = "<a href='{$entry->parent->cpEditUrl}'>{$entry->parent->title}</a>";
                    }
                }
            }
        );

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
    }

    // Protected Methods
    // =========================================================================

    // Private Methods
    // =========================================================================

    /**
     * This method encapsulates logic that happens on the EVENT_AFTER_SAVE_ELEMENT event;
     * This event is triggered after an element is saved.
     */
    private function eventAfterSaveElement()
    {
        Event::on(
            Elements::class,
            Elements::EVENT_AFTER_SAVE_ELEMENT,
            function (ElementEvent $event) {

                // Set Default value for ExpiryDate for alerts
                $alertType = Craft::$app->sections->getSectionByHandle('alerts');
                if (isset($event->element->sectionId) && ($alertType->id === $event->element->sectionId)) {
                    if ($event->element->expiryDate == null) {
                        $event->element->expiryDate = DateTime::createFromFormat('Y/m/d H:i:s', '2099/01/01 00:00:00');
                    }
                }

                if (!$event->element->getIsRevision() && is_a($event->element, 'craft\elements\Entry')) {
                    // enqueue a job to update the TNT search index for this entry
                    Craft::$app->queue->push(new UpdateTNTIndexJob([
                        'entryId' => $event->element->id
                    ]));
                }

                $this->purgeAppointmentSchedulingVisitTypeCodesCache($event->element);
                $this->purgeMiscellaneousCacheEntries();

                // Only physical locations (parents) have an address field in the Craft CP
                // For the purposes of backend geo sorting and filtering, we also need this field to be available for suites (children)
                // We can update the table directly.
                $locationSuiteType = Craft::$app->sections->getSectionByHandle('locations');
                if (!$event->element->getIsRevision() && isset($event->element->sectionId) && ($locationSuiteType->id === $event->element->sectionId)) {
                    $this->updateLocationSuitesFieldAddress($event->element);
                }

                $serviceType = Craft::$app->sections->getSectionByHandle('services');
                if (!$event->element->getIsRevision() && isset($event->element->sectionId) && ($serviceType->id === $event->element->sectionId)) {
                    foreach (Entry::find()->section('locations')->ids() as $id) {
                        $this->updateLocationSuitesFieldAddress(null, $id, true);
                    }
                }

                $physicianType = Craft::$app->sections->getSectionByHandle('physicians');
                if (!$event->element->getIsRevision() && isset($event->element->sectionId) && ($physicianType->id === $event->element->sectionId)) {
                    $this->updatePhysiciansLocationsTable($event->element);
                }

                $schedulingDeeplinkType = Craft::$app->sections->getSectionByHandle('schedulingDeeplink');
                if (!$event->element->getIsRevision() && isset($event->element->sectionId) && ($schedulingDeeplinkType->id === $event->element->sectionId)) {
                    Craft::$app->db->createCommand()
                        ->update(
                            'content',
                            ['field_scheduleDeeplinkUrl' => $event->element->url],
                            'elementId = :childId',
                            [':childId' => $event->element->id]
                        )->execute();
                }
            }
        );
    }

    private function purgeAppointmentSchedulingVisitTypeCodesCache(Element $element)
    {
        if (!isset($element->fieldId)) {
            return;
        }

        // get global settings
        $appointmentSchedulingVisitTypeCodes = Craft::$app->globals->getSetByHandle('appointmentSchedulingVisitTypeCodes');

        $fieldIds = $appointmentSchedulingVisitTypeCodes->fieldLayout->fieldIds;
        $currentFieldId = $element->fieldId;

        if (\in_array($currentFieldId, $fieldIds)) {
            Craft::$app->cache->delete("scheduling_visit_type_codes");
        }
    }

    /**
     * This method is executed after every element save.
     * If there are any Redis cache entries that ought to be purged after an element save,
     * perhaps without needing to validate which entry is being saved,
     * those purges can be included here.
     */
    private function purgeMiscellaneousCacheEntries()
    {
        Craft::$app->cache->delete("immediate_care_service_id");
        Craft::$app->cache->delete("express_care_service_id");
    }

    private function updateLocationSuitesFieldAddress(?Element $element, string $elementId = null, bool $onlyCacheClear = false)
    {
        $this->clearRedisCacheDataUsingThisLocation($elementId ?? $element->id ?? "");
        if ($element->parent ?? false) {
            $this->clearRedisCacheDataUsingThisLocation($element->parent->id ?? "");
        }

        if ($onlyCacheClear || !$element) {
            return;
        }

        // if current entry is a suite (a child), only update this suite
        // if current entry is a parent location, update all children suites
        $suitesToUpdate = $element->parent ? [$element] : $element->getChildren();

        if (count($suitesToUpdate) == 0) {
            return;
        }

        // get parent content field_address value
        $parentContentRow = (new Query())->select('field_address')
            ->from('content')
            ->where(['elementId' => $suitesToUpdate[0]->parent->id])
            ->one();

        if (!$parentContentRow) {
            return;
        }

        $parentFieldAddress = $parentContentRow['field_address'];

        $transaction = Craft::$app->db->beginTransaction();
        try {
            foreach ($suitesToUpdate as $child) {
                Craft::$app->db->createCommand()
                    ->update(
                        'content',
                        ['field_address' => $parentFieldAddress],
                        'elementId = :childId',
                        [':childId' => $child->id]
                    )->execute();
            }
            $transaction->commit();
        } catch (\Exception | \Throwable $e) {
            $transaction->rollBack();
            throw $e;
        }
    }

    /**
     * Keeps track of, and cleares, any Redis cache entries that were created using this location's data.
     * Craft does not automatically purge Redis cache when this Element is updated; we must clear it manually
     *
     * @param Element $location          The location
     * @return null
     */
    private function clearRedisCacheDataUsingThisLocation(string $elementId)
    {
        Craft::$app->cache->delete("services_for_location_{$elementId}");
    }

    /**
     * Update custom {{physicians_locations}} table.
     * This table allows for a quick sort of physicians by distance.
     */
    private function updatePhysiciansLocationsTable(Element $eventElement, bool $insert = true)
    {
        $transaction = Craft::$app->db->beginTransaction();
        try {
            // delete old rows for this physician
            Craft::$app->db->createCommand()->delete('physicians_locations', 'physicianElementId = :physicianElementId', [':physicianElementId' => $eventElement->id])->execute();

            if ($insert) {
                $locations = Entry::find()->section('locations')->relatedTo([
                    'and',
                    ['sourceElement' => $eventElement]
                ])->all();

                foreach ($locations as $location) {
                    Craft::$app->db->createCommand()->insert(
                        'physicians_locations',
                        [
                            'physicianElementId' => $eventElement->id,
                            'locationElementId' => $location->id,
                        ],
                        false
                    )->execute();
                }
            }

            $transaction->commit();
        } catch (\Exception | \Throwable $e) {
            $transaction->rollBack();
            throw $e;
        }
    }
}
