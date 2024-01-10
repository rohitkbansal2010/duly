<?php declare(strict_types=1);

if (!defined('APP_ROOT')) {
    define('APP_ROOT', dirname(dirname(dirname(__DIR__))));
}

require_once APP_ROOT . '/vendor/autoload.php';
require_once APP_ROOT . '/saml/vendor/autoload.php';

$dotenv = new \Dotenv\Dotenv(APP_ROOT);
$dotenv->load();

// The built-in Craft 3 CMS Service Provider
$metadata[getenv('SAML_SP_CRAFT')] = [
    'AssertionConsumerService' => getenv('DEFAULT_SITE_URL') . '/sml/acs',
    'SingleLogoutService' => getenv('DEFAULT_SITE_URL') . '/sml/slo',
    'certificate' => 'sp.crt'
];

$metadata['https://dmgmychart.dupagemd.net/mychartdmgtst'] = [
    'entityid' => 'https://dmgmychart.dupagemd.net/mychartdmgtst',
    'contacts' => [
        [
            'contactType' => 'administrative',
            'company' => '',
            'givenName' => '',
            'surName' => '',
            'emailAddress' => [
                '',
            ],
            'telephoneNumber' => [
                '',
            ],
        ],
    ],
    'metadata-set' => 'saml20-sp-remote',
    'AssertionConsumerService' => [
        [
            'Binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST',
            'Location' => 'https://dmgmychart.dupagemd.net/mychartdmgtst/Authentication/Saml/Login',
            'index' => 0,
            'isDefault' => true,
        ],
    ],
    'SingleLogoutService' => [
        [
            'Binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST',
            'Location' => 'https://dmgmychart.dupagemd.net/mychartdmgtst/Authentication/Saml/Logout',
            'ResponseLocation' => 'https://dmgmychart.dupagemd.net/mychartdmgtst/Authentication/Saml/Logout',
        ],
        [
            'Binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect',
            'Location' => 'https://dmgmychart.dupagemd.net/mychartdmgtst/Authentication/Saml/Logout',
            'ResponseLocation' => 'https://dmgmychart.dupagemd.net/mychartdmgtst/Authentication/Saml/Logout',
        ],
    ],
    'keys' => [
        [
            'encryption' => false,
            'signing' => true,
            'type' => 'X509Certificate',
            'X509Certificate' => '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',
        ],
        [
            'encryption' => true,
            'signing' => false,
            'type' => 'X509Certificate',
            'X509Certificate' => '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',
        ],
    ],
    'validate.authnrequest' => true,
    'saml20.sign.assertion' => true,
];