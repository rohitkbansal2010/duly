<?php
/**
 * Yii Application Config
 *
 * Edit this file at your own risk!
 *
 * The array returned by this file will get merged with
 * vendor/craftcms/cms/src/config/app.php and app.[web|console].php, when
 * Craft's bootstrap script is defining the configuration for the entire
 * application.
 *
 * You can define custom modules and system components, and even override the
 * built-in system components.
 *
 * If you want to modify the application config for *only* web requests or
 * *only* console requests, create an app.web.php or app.console.php file in
 * your config/ folder, alongside this one.
 */

$logger = new \Monolog\Logger("Craft_DMG");
$handler = new \Monolog\Handler\RotatingFileHandler(dirname(__DIR__) . '/storage/logs/application.log', 3, \Monolog\Logger::INFO);
$formatter = new \Monolog\Formatter\LineFormatter("[%datetime%] %channel%.%level_name%: [%message%] %context%\n");
$formatter->includeStacktraces(true);
$handler->setFormatter($formatter);
$logger->pushHandler($handler, 'INFO');

return [
    'components' => [
        'db' => function () {
            $config = \craft\helpers\App::dbConfig();

            // If we're connecting to the production consul instance by hostname, dynamically remap everything
            if (getenv('DB_SERVER') === 'primary.mariadb.service.consul') {
                // Fetch the consul DNS SRV records, and sort them by hostname.
                $records = dns_get_record('replica.mariadb.service.consul', DNS_SRV);

                // If we don't get any records back... give up and left Craft fail.
                if ($records === false || count($records) == 0) {
                    return Craft::createObject($config);
                }

                usort($records, function ($a, $b) { return $a['target'] > $b['target']; });

                // Setup the replica config
                $config['replicaConfig'] = [
                    'username' => $config['username'],
                    'password' => $config['password'],
                    'tablePrefix' => $config['tablePrefix']
                ];

                if (getenv('MYSQL_CERT_DIR')) {
                    $config['replicaConfig']['attributes'] = [
                        PDO::MYSQL_ATTR_SSL_CA => getenv('MYSQL_CERT_DIR') . '/ca.pem',
                        // The peer certificate may not be signed for every attribute
                        PDO::MYSQL_ATTR_SSL_VERIFY_SERVER_CERT => false,
                        PDO::ATTR_TIMEOUT => 8
                    ];

                    if (getenv('MYSQL_CLIENT_CERT')) {
                        $config['replicaConfig']['attributes'][PDO::MYSQL_ATTR_SSL_KEY] = getenv('MYSQL_CERT_DIR') . '/client-key.pem';
                        $config['replicaConfig']['attributes'][PDO::MYSQL_ATTR_SSL_CERT] = getenv('MYSQL_CERT_DIR') . '/client-cert.pem';
                    }
                }

                // Add all other nodes as replicas.
                $replicas = [];
                foreach ($records as $record) {
                    $replicas[] = [
                        'dsn' => 'mysql:host=' . $record['target'] . ';dbname=' . getenv('DB_DATABASE') . ';port=' . getenv('DB_PORT')
                    ];
                }

                $config['replicas'] = $replicas;
            }

            return Craft::createObject($config);
        },
        'redis' => [
            'class' => \yii\redis\Connection::class,
            'hostname' => getenv('REDIS_HOST'),
            'port' => getenv('REDIS_PORT'),
            //'password' => getenv('REDIS_PASSWORD') ?? null,
        ],
        'mutex' => [
            'mutex' => 'yii\redis\Mutex',
        ],
        'cache' => [
            'class' => \yii\redis\Cache::class,
            'defaultDuration' => 86400,
        ],
        'queue' => [
            'ttr' => 120,
            'attempts' => 1
        ],
        'httpclient' => [
            'class' => \modules\DupageCoreModule\components\HttpClientComponent::class,
            'clientOptions' => [
                'transport' => \yii\httpclient\CurlTransport::class,
            ],
            'options' => [
                'timeout' => 5
            ]
        ],
        'mailer' => function () {
            $settings = \craft\helpers\App::mailSettings();

            $settings->transportType = \craft\mail\transportadapters\Smtp::class;

            $settings->transportSettings = [
                'timeout' => getenv('SMTP_TIMEOUT'),
                'encryptionMethod' => getenv('SMTP_ENCRYPTION_METHOD'),
                'username' => getenv('SMTP_USERNAME'),
                'password' => getenv('SMTP_PASSWORD'),
                'host' => getenv('SMTP_HOSTNAME'),
                'port' => getenv('SMTP_PORT'),
                'useAuthentication' => getenv('SMTP_USE_AUTHENTICATION')
            ];

            $config = \craft\helpers\App::mailerConfig($settings);
            $config['from'] = [
                getenv('SMTP_SENDFROM_ADDRESS') => getenv('SMTP_SENDFROM_NAME')
            ];

            return Craft::createObject($config);
        },
        'log' => [
            'targets' => [
                [
                    'class' => \modules\DupageCoreModule\components\log\PsrTarget::class,
                    'levels' => [\yii\log\Logger::LEVEL_INFO, \yii\log\Logger::LEVEL_ERROR, \yii\log\Logger::LEVEL_WARNING],
                    'addTimestampToContext' => true,
                    'logger' => $logger,
                    'extractExceptionTrace' => true,
                    'except' => [
                        'yii\db\Command:*',
                        'yrc\filters\auth\HMACSignatureAuth:*',
                        'yii\db\Connection:*',
                        'yii\filters\RateLimiter:*',
                        'yii\mail\BaseMailer::send',
                        'yii\web\User::login',
                        'yii\web\Session::open',
                        "yii\\web\\HttpException:404",
                        "yii\web\NotFoundHttpException",
                        "modules\\physiciansmodule\\PhysiciansModule::init",
                        "modules\\schedulingmodule\\SchedulingModule::init",
                        "modules\\samlmodule\\SamlModule::init",
                        "modules\\apimodule\\ApiModule::init",
                        "modules\\locationsmodule\\LocationsModule::init",
                        "shennyg\\azureblobremotevolume\\AzureBlobRemoteVolume::init",
                        "nystudio107\\cookies\\Cookies::init",
                        "punchkick\\maintenance\\MaintenancePlugin::init",
                        "punchkick\\eventregistration\\EventRegistration::init",
                        "mmikkel\\incognitofield\\IncognitoField::init",
                        "monachilada\\matrixtoolbar\\MatrixToolbar::init",
                        "cavellblood\\stringbase64\\StringBase64::init",
                        "nystudio107\\twigprofiler\\TwigProfiler::init",
                        "nystudio107\\twigpack\\Twigpack::init",
                        "nystudio107\\typogrify\\Typogrify::init",
                        "yii\\redis\\Connection::redirect",
                        'yii\httpclient\CurlTransport::send',
                        'yii\httpclient\\CurlTransport::batchSend',
                        'craft\\queue\\QueueLogBehavior::afterExec',
                        'craft\\queue\\QueueLogBehavior::beforeExec'
                    ],
                    'logVars' => [],
                ]
            ]
        ],
    ],
    'modules' => [
        'scheduling-module' => [
            'class' => \modules\schedulingmodule\SchedulingModule::class,
            'components' => [
                'schedulingModuleService' => [
                    'class' => \modules\schedulingmodule\services\SchedulingModuleService::class
                ],
                'schedulingModuleEpicService' => [
                    'class' => \modules\schedulingmodule\services\SchedulingModuleEpicService::class,
                ],
            ],
        ]
    ],
    'bootstrap' => [
        'scheduling-module'
    ],
];
