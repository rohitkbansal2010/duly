<?php
/**
 * SAML2 module for Craft CMS 3.x
 *
 * Allows for extended management of EPIC endpoints.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\samlmodule\services;

use Craft;
use craft\base\Component;
use modules\DupageCoreModule\forms\LoginForm;
use OneLogin\Saml2\Auth;

/**
 * @author    Wipfli Digital
 * @package   SamnlModuleService
 * @since     1.0.0
 */
class SamlModuleService extends Component
{
    public function getOneLoginSettings()
    {
        define('TOOLKIT_PATH', CRAFT_BASE_PATH . '/vendor/onelogin/php-saml');
        require_once TOOLKIT_PATH . '/_toolkit_loader.php';

        return [
            'compress' => [
                'requests' => getenv('ENVIRONMENT') != 'lxd',
                'responses' => getenv('ENVIRONMENT') != 'lxd'
            ],
            'security' => [
                'nameIdEncrypted' => true,
                'authnRequestsSigned' => true,
                'logoutRequestSigned' => true,
                'logoutResponseSigned' => true,
                'signMetadata' => true,
                'wantMessagesSigned' => true,
                'wantAssertionsEncrypted' => true,
                'wantAssertionsSigned' => true,
            ],
            'baseUrl' => getenv('DEFAULT_SITE_URL'),
            'strict' => false,
            'debug' => getenv('ENVIRONMENT') != 'lxd',
            'sp' => [
                'entityId' => getenv('SAML_SP_CRAFT'),
                'AuthnRequestsSigned' => true,
                'assertionConsumerService' => [
                    'url' => getenv('DEFAULT_SITE_URL') . '/sml/acs',
                    'binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST',
                ],
                'singleLogoutService' => [
                    'url' => getenv('DEFAULT_SITE_URL') . '/sml/slo',
                    'binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect',
                ],
                'NameIDFormat' => 'urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress',
                'x509cert' => \file_get_contents(CRAFT_BASE_PATH . '/config/saml/certs/sp.crt'),
                'privateKey' => \file_get_contents(CRAFT_BASE_PATH. '/config/saml/certs/sp.key'),
            ],
            'idp' => [
                'entityId' => getenv('SAML_ENTITY_ID'),
                'singleSignOnService' => [
                    'url' => getenv('DEFAULT_SITE_URL') . '/saml/saml2/idp/SSOService.php',
                    'binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST',
                ],
                'singleLogoutService' => [
                    'url' => getenv('DEFAULT_SITE_URL') . '/saml/saml2/idp/SingleLogoutService.php',
                    'binding' => 'urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect',
                ],
                'x509cert' => \file_get_contents(CRAFT_BASE_PATH . '/config/saml/certs/saml.crt'),
            ]
        ];
    }

    /**
     * @param OneLogin\Saml2\Response $response
     * @return bool
     */
    public function authenticate(\OneLogin\Saml2\Response $response)
    {
        return LoginForm::loginWithSamlIdentity($response);
    }

    /**
     * @param OneLogin\Saml2\LogoutResponse $response
     * @return bool
     */
    public function deauthenticate(\OneLogin\Saml2\LogoutResponse $response)
    {
        return Craft::$app->patient_user->logout();
    }
}