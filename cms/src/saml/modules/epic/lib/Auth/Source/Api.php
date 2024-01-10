<?php declare(strict_types=1);

namespace SimpleSAML\Module\epic\Auth\Source;

use Curl\Curl;
use SimpleSAML\Module\core\Auth\UserPassBase;

/**
 * Implements a username/password authenticator that binds from an Epic SOAP API
 * @class Api
 */
final class Api extends UserPassBase
{
    /**
     * Login source handler
     *
     * This method makes a call to /sml/auth and populates the SAML2 assertions and attributes depending upon the response
     *
     * @param string $username
     * @param string $password
     * @throws \SimpleSaml\Error\Error
     */
    protected function login($username, $password)
    {
        error_reporting(-1);
        ini_set('display_errors', 'true');
        $curl = new Curl;

        $curl->setOpt(CURLOPT_FOLLOWLOCATION, true);
        $curl->setOpt(CURLOPT_RESOLVE, [
            'dupagemedicalgroup.com:443:127.0.0.1',
            'www.dupagemedicalgroup.com:443:127.0.0.1',
            'dulyhealthandcare.com:443:127.0.0.1',
            'www.dulyhealthandcare.com:443:127.0.0.1',
            'npd.dupagemedicalgroup.com:443:127.0.0.1'
        ]);

        $response = $curl->post($this->getDomain() . '/sml/auth', [
            'username' => $username,
            'password' => $password
        ]);

        if ($curl->getHttpStatusCode() != 200) {
            throw new \SimpleSAML\Error\Error('Invalid Username/Password');
        }

        return [
            'uid' => [ $response->epi ],
            'userName' => [ $username ],
            'displayName' => [ $response->name ],
            'organizationalUnitName' => [ 'DuPage Medical Group' ]
        ];
    }

    private function getDomain()
    {
        if (getenv('ENVIRONMENT') === '6aac88b258202c4b1774c8362ca3be63.t73.pkiapps.com') {
            return getenv('DEFAULT_SITE_URL');
        }

        $domain = \str_replace([':8443', ':8444'], '', getenv('DEFAULT_SITE_URL'));
        return $domain;
    }
}