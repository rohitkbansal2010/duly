<?php
/**
 * Yii Application Config
 *
 * Edit this file at your own risk!
 *
 * The array returned by this file will get merged with
 * vendor/craftcms/cms/src/config/app/main.php and [web|console].php, when
 * Craft's bootstrap script is defining the configuration for the entire
 * application.
 *
 * You can define custom modules and system components, and even override the
 * built-in system components.
 */
return [
    'modules' => [
        'locations-module' => [
            'class' => \modules\locationsmodule\LocationsModule::class,
            'components' => [
                'locationsModuleService' => [
                    'class' => 'modules\locationsmodule\services\LocationsModuleService',
                ],
            ],
        ],
    ],
    'bootstrap' => ['locations-module'],
];
