<?php declare(strict_types=1);

if (!defined('APP_ROOT')) {
    define('APP_ROOT', dirname(dirname(dirname(__DIR__))));
}

require_once APP_ROOT . '/vendor/autoload.php';
require_once APP_ROOT . '/saml/vendor/autoload.php';

$dotenv = new \Dotenv\Dotenv(APP_ROOT);
$dotenv->load();

/**
 * Pulls the entity id from the auth state
 * @param array $request
 * @return string
 */
function getSpEntityIdFromAuthState($request)
{
    if (!isset($request['AuthState'])) {
        return null;
    }

    $url = explode(':', $request['AuthState']);
    unset($url[0]);
    $url = implode(':', $url);
    $queryParams = explode('&', parse_url($url)['query']);
    $spEntityId = null;
    foreach ($queryParams as $query) {
        if (stripos($query, 'spentityid') !== false) {
            return explode('=', $query)[1];
            break;
        }
    }

    return null;
}

$config = [
    'epic' => [
        'epic:Api',
        'entityID'      => getenv("SAML_ENTITY_ID"),
        'debug'         => getenv('ENVIRONMENT') == 'dev',
        'spEntityId'    => getSpEntityIdFromAuthState($_REQUEST)
    ]
];

// Allow admin logins in debug mode
if (getenv('ENVIRONMENT') != 'lxd') {
    $config['admin'] = [
        'core:AdminPassword',
    ];
}