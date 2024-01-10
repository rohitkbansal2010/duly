<?php

namespace modules\schedulingmodule\services;

use Craft;
use craft\base\Component;
use Firebase\JWT\JWT;
use Firebase\JWT\Key;
use Firebase\JWT\SignatureInvalidException;
use yii\web\HttpException;

class MicrositesAuthenticationService extends Component {

    /**
     * Validate incoming jwtToken from microsites
     * @throws HttpException
     */
    public function validateJwtToken() {
        try{
            $micrositeJwt = Craft::$app->config->general->micrositeJwt;
            $auth = Craft::$app->getRequest()->getHeaders()->get('Authorization');
            $jwt= explode(' ',$auth)[1];
            // $jwt = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.5FobtfXUAU017iJ1ceiN2V0TevSRDw9n5hgyLQj1GMM';
            $decoded = JWT::decode($jwt, new Key($micrositeJwt['jwtSecretKey'], $micrositeJwt['jwtAlgo']));
            if((time()- $decoded->iat) > $micrositeJwt['jwtValidTime']) {
                throw new SignatureInvalidException();
            }
        } catch (SignatureInvalidException $e) {
            throw new HttpException(401);
        }
    }
}