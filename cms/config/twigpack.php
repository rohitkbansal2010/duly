<?php

return [
    '*' => [
        'useDevServer' => false,
        // The JavaScript entry from the manifest.json to inject on Twig error pages
        'errorEntry' => '',
        // Manifest file names
        'manifest' => [
            'legacy' => 'manifest.json',
            'modern' => 'manifest.json',
        ],
        // Public server config
        'server' => [
            'manifestPath' => Craft::getAlias('@webroot') . '/dist',
            'publicPath' => getenv('DEFAULT_SITE_URL'),
        ],
        // webpack-dev-server config
        'devServer' => [
            'manifestPath' => 'http://webpack:8080/dist',
            'publicPath' => getenv('DEFAULT_SITE_URL'),
        ],
        // Local files config
        'localFiles' => [
            'basePath' => Craft::getAlias('@webroot') . '/dist',
            'criticalPrefix' => 'dist/criticalcss/',
            'criticalSuffix' => '_critical.min.css',
        ],
    ],
    'dev' => [
        'useDevServer' => true
    ]
];
