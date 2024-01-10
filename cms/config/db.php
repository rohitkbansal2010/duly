<?php
/**
 * Database Configuration
 *
 * All of your system's database connection settings go in here. You can see a
 * list of the available settings in vendor/craftcms/cms/src/config/DbConfig.php.
 *
 * @see craft\config\DbConfig
 */

$cfg = [
    'driver' => getenv('DB_DRIVER'),
    'server' => getenv('DB_SERVER'),
    'user' => getenv('DB_USER'),
    'password' => getenv('DB_PASSWORD'),
    'database' => getenv('DB_DATABASE'),
    'schema' => getenv('DB_SCHEMA'),
    'tablePrefix' => getenv('DB_TABLE_PREFIX'),
    'port' => getenv('DB_PORT'),
];

// If TLS certificate directories are defined, then populated the MySQL PDO attributes for TLS connectivity
if (getenv('MYSQL_CERT_DIR')) {
    $cfg['attributes'] = [
        PDO::MYSQL_ATTR_SSL_CA => getenv('MYSQL_CERT_DIR') . '/ca.pem',
        // The peer certificate may not be signed for every attribute
        PDO::MYSQL_ATTR_SSL_VERIFY_SERVER_CERT => false
    ];

    if (getenv('MYSQL_CLIENT_CERT')) {
        $cfg['attributes'][PDO::MYSQL_ATTR_SSL_KEY] = getenv('MYSQL_CERT_DIR') . '/client-key.pem';
        $cfg['attributes'][PDO::MYSQL_ATTR_SSL_CERT] = getenv('MYSQL_CERT_DIR') . '/client-cert.pem';
     }
}

return $cfg;
