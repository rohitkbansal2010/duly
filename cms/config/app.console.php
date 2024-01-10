<?php

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
    ],
    'bootstrap' => [
        'dupage-core-module',
        'physicians-module'
    ],
];
