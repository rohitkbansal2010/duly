<?php
/**
 * General Configuration
 *
 * All of your system's general configuration settings go in here. You can see a
 * list of the available settings in vendor/craftcms/cms/src/config/GeneralConfig.php.
 *
 * @see \craft\config\GeneralConfig
 */

return [
    // Global settings
    '*' => [
        // Default Week Start Day (0 = Sunday, 1 = Monday...)
        'defaultWeekStartDay' => 1,
        'allowAdminChanges' => false,

        // Whether generated URLs should omit "index.php"
        'omitScriptNameInUrls' => true,

        // Control Panel trigger word
        'cpTrigger' => 'admin',

        // The secure key Craft will use for hashing and encrypting data
        'securityKey' => getenv('SECURITY_KEY'),

        'errorTemplatePrefix' => "_errors/",
        // Whether to save the project config out to config/project.yaml
        // (see https://docs.craftcms.com/v3/project-config.html)
        'useProjectConfigFile' => true,
        'allowUpdates' => false,
        'allowAdminChanges' => false,
        'sendPoweredByHeader' => false,
        'runQueueAutomatically' => false,
        'siteName' => getenv('DEFAULT_SITE_NAME'),
        'siteNameShort' => getenv('DEFAULT_SITE_NAME_SHORT'),
        'myChartName' => getenv('DEFAULT_MYCHART_NAME'),
        'siteUrl' => getenv('DEFAULT_SITE_URL'),
        'baseUrl' => getenv('DEFAULT_SITE_URL'),
        'baseCpUrl' => getenv('DEFAULT_SITE_URL'),
        'errorTemplatePrefix' => '_errors/',
        'aliases' => [
            '@webroot' => CRAFT_BASE_PATH . '/web',
            '@assetBaseUrl' => 'https://' . getenv('AZURE_STORAGE_ACCOUNT_NAME'). '.blob.core.windows.net/' . getenv('AZURE_STORAGE_CONTAINER_NAME') . '/',
            '@assetBasePath' => getenv('WEB_VOLUMES_DIR'),
            '@modules/DupageCoreModule' => dirname(__DIR__) . '/modules/core'
        ],
        'enableGql' => false,
        'sameSiteCookieValue' => \yii\web\Cookie::SAME_SITE_LAX,
        'useEmailAsUsername' => true,
        'backupOnUpdate' => false,
        'pageTrigger' => 'page/',
        'pressGaneyAppId' => getenv('PRESS_GANEY_APP_ID'),
        'pressGaneyAppSecret' => getenv('PRESS_GANEY_APP_SECRET'),
        'gtm_id' => 'GTM-WTDDCT9',
        'ga_id' => 'UA-159717106-2',
        'gtag_id' => 'UA-159717106-2',
        'epic_client_id' => getenv('EPIC_CLIENT_ID'),
        'csrfTokenName' => '7cbf40c6d657754517c6905b0b65fd1e',
        'contact_us_email' => getenv('CONTACT_US_EMAIL'),
        'phpSessionName' => '374d18ba1470ddd026eed29f136fd8',
        'cspNonce' => \base64_encode(\random_bytes(20)),
        'reCAPTCHASiteKey' => getenv('RECAPTCHA_SITE_KEY'),
        'reCAPTCHASiteSecret' => getenv('RECAPTCHA_SITE_SECRET'),
        'maxRevisions' => 5,
        'micrositeJwt' => [
            'jwtSecretKey' => getenv('JWT_SECRET_KEY'),
            'jwtValidTime' => getenv('JWT_VALID_TIME'),
            'jwtAlgo' => getenv('JWT_ALGO')
        ]
    ], 

    // Dev environment settings
    'dev' => [
        // Dev Mode (see https://craftcms.com/guides/what-dev-mode-does)
        'devMode' => true,
        'allowUpdates' => true,
        'allowAdminChanges' => true,
        'facebook_app_id' => '2673377229447981',
        'imgproxy_key' => 'fc8ecb679987734895656b54f1efd581352daadd0831b0841015808ba3d0595c85425784f5c009855c1d5e7b62038c2b5e937fe49c003f544dafad48b1b37a2f',
        'imgproxy_salt' => '74f57173bf8b447e775ca645e3620ea5f549dc06c4441ecc3982db171386e49cd518f343f3bf6369cd3136566b9c09ac059ca2e104c405153ba13b76a9a0e564'
    ],

    // Staging environment settings
    '6aac88b258202c4b1774c8362ca3be63.t73.pkiapps.com' => [
        'facebook_app_id' => '2673377229447981',
        'imgproxy_key' => 'fc8ecb679987734895656b54f1efd581352daadd0831b0841015808ba3d0595c85425784f5c009855c1d5e7b62038c2b5e937fe49c003f544dafad48b1b37a2f',
        'imgproxy_salt' => '74f57173bf8b447e775ca645e3620ea5f549dc06c4441ecc3982db171386e49cd518f343f3bf6369cd3136566b9c09ac059ca2e104c405153ba13b76a9a0e564'
    ],

    'lxc' => [
        'facebook_app_id' => getenv('FACEBOOK_APP_ID'),
        'imgproxy_key' => getenv('IMGPROXY_KEY'),
        'imgproxy_salt' => getenv('IMGPROXY_SALT'),
        'gtag_id' => 'UA-159717106-1',
        'ga_id' => 'UA-159717106-1',
        'contact_us_email' => getenv('CONTACT_US_EMAIL'),
    ]
];
