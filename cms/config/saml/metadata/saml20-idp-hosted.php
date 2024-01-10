<?php declare(strict_types=1);

if (!defined('APP_ROOT')) {
    define('APP_ROOT', dirname(dirname(dirname(__DIR__))));
}

require_once APP_ROOT . '/vendor/autoload.php';
require_once APP_ROOT . '/saml/vendor/autoload.php';

$dotenv = new \Dotenv\Dotenv(APP_ROOT);
$dotenv->load();

/**
 * SAML 2.0 IdP configuration for SimpleSAMLphp.
 *
 * See: https://simplesamlphp.org/docs/stable/simplesamlphp-reference-idp-hosted
 */

$metadata[getenv('SAML_ENTITY_ID')] = [
    'host' => '__DEFAULT__',
    'privatekey' => 'saml.key',
    'certificate' => 'saml.crt',
    'auth' => 'epic',
    'attributes.NameFormat' => 'urn:oasis:names:tc:SAML:2.0:attrname-format:uri',
    'NameIDFormat' => 'urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress',
    'assertion.encryption' => true,
    'sign.logout' => true,
    'signature.algorithm' => 'http://www.w3.org/2001/04/xmldsig-more#rsa-sha256',
    'simplesaml.nameidattribute' => 'uid',
    'SingleSignOnServiceBinding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST',
    'authproc' => [
        100 => ['class' => 'core:AttributeMap', 'name2oid'],
    ],
];
