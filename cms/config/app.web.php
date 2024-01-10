<?php

use Craft;

return [
    'modules' => [
        'dupage-core-module' => [
            'class' => \modules\DupageCoreModule\DupageCoreModule::class,
            'components' => [
                'dupageCoreModuleService' => [
                    'class' => 'modules\DupageCoreModule\services\DupageCoreModuleService',
                ],
            ],
        ],
        'physicians-module' => [
            'class' => \modules\physiciansmodule\PhysiciansModule::class,
            'components' => [
                'physiciansModuleService' => [
                    'class' => 'modules\physiciansmodule\services\PhysiciansModuleService',
                ],
            ],
        ],
        'locations-module' => [
            'class' => \modules\locationsmodule\LocationsModule::class,
            'components' => [
                'locationsModuleService' => [
                    'class' => 'modules\locationsmodule\services\LocationsModuleService',
                ],
            ],
        ],
        'saml-module' => [
            'class' => \modules\samlmodule\SamlModule::class,
            'components' => [
                'samlModuleService' => [
                    'class' => 'modules\samlmodule\services\SamlModuleService',
                ],
            ],
        ],
        'api-module' => [
            'class' => \modules\apimodule\ApiModule::class,
            'components' => [
                'apiModuleService' => [
                    'class' =>  \modules\apimodule\services\ApiModuleService::class,
                ],
            ],
        ],
        
    ],
    'components' => [
        'session' => function () {
            $config = craft\helpers\App::sessionConfig();
            $config['class'] = yii\redis\Session::class;
            return Craft::createObject($config);
        },
        'patient_user' => [
            'class' => \yii\web\User::class,
            'identityClass' => \modules\DupageCoreModule\models\PatientUser::class,
            'enableAutoLogin' => true,
            'enableSession' => true,
            'identityCookie' => [
                'name' => '_puident',
                'httpOnly' => true,
                'secure' => true,
                'sameSite' => \yii\web\Cookie::SAME_SITE_LAX
            ]
        ]
    ],
    'bootstrap' => [
        'dupage-core-module',
        'physicians-module',
        'locations-module',
        'saml-module',
        'api-module'
    ],
];
