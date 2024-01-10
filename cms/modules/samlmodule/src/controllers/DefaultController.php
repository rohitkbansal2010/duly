<?php

namespace modules\samlmodule\controllers;

use Craft;
use OneLogin\Saml2\Auth;
use OneLogin\Saml2\AuthnRequest;
use OneLogin\Saml2\LogoutRequest;
use OneLogin\Saml2\LogoutResponse;
use OneLogin\Saml2\Response as SamlResponse;
use OneLogin\Saml2\Settings;
use OneLogin\Saml2\Utils;
use craft\web\Controller;
use craft\web\Response;
use modules\DupageCoreModule\forms\LoginForm;
use modules\samlmodule\SamlModule;
use modules\samlmodule\SamlModuleService;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\filters\AccessControl;
use yii\filters\VerbFilter;
use yii\web\HttpException;

class DefaultController extends Controller
{
    /**
     * Allow anonymous access to all endpoints
     */
    protected $allowAnonymous = self::ALLOW_ANONYMOUS_LIVE;

    /**
     * Disable CSRF as these are called from SAML2 endpoints
     */
    public $enableCsrfValidation = false;

    public function behaviors()
    {
        $behaviors = parent::behaviors();

        $behaviors['verbs'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'auth' => ['POST'],
                'acs' => ['GET', 'POST'],
                'slo' => ['GET', 'POST']
            ]
        ];

        $behaviors['access'] = [
            'class' => AccessControl::class,
            'rules' => [
                [
                    'allow' => true,
                    'actions' => ['auth'],
                    'roles' => ['?', '@']
                ],
                [
                    'allow' => true,
                    'actions' => ['acs'],
                    'roles' => ['?']
                ],
                [
                    'allow' => true,
                    'actions' => ['slo'],
                    'roles' => ['@', '?']
                ]
            ],
            'user' => 'patient_user',
            'denyCallback' => function ($rule, $action) {
                return $this->redirect('/sml/acs');
            }
        ];

        return $behaviors;
    }

    /**
     * Authentication action for SimpleSAML\Module\epic\Auth\Source\Api
     *
     * @return JsonResponse
     * @throws HttpException
     */
    public function actionAuth()
    {
        $form = new LoginForm;

        if (Craft::$app->request->isPost) {
            if ($form->load(['LoginForm' => Craft::$app->request->post()])) {
                if ($form->validate()) {
                    $response = SchedulingModule::getInstance()
                        ->schedulingModuleEpicService
                        ->authenticate($form->username, $form->password);

                    if ($response !== null) {
                        Craft::info(new PsrMessage('Authenticating user via SAML2 API Gateway', [
                            'username' => $form->username
                        ], get_class($this) . '::' . __METHOD__));

                        return $this->asJson($response);
                    }
                }
            }
        }

        Craft::info(new PsrMessage('Authentication failed via SAML2 API Gateway', [
            'username' => $form->username
        ], get_class($this) . '::' . __METHOD__));

        throw new HttpException(401);
    }

    /**
     * Initiates and facilitates and authentication request
     * @return Response
     */
    public function actionAcs()
    {
        $settings = SamlModule::getInstance()
            ->samlModuleService
            ->getOneLoginSettings();

        if ($postBody = Craft::$app->request->post('SAMLResponse')) {
            $response = new SamlResponse(new Settings($settings), $postBody);

            try {
                if ($response->isValid()) {
                    $result = SamlModule::getInstance()
                        ->samlModuleService
                        ->authenticate($response);

                    // Redirect the user to the previously requested page, referrer, or elsewhere
                    return Craft::$app->response->redirect('/');
                } else {
                    throw new \yii\base\Exception('Failed to validate response');
                }
            } catch (\Exception $e) {
                Craft::error(new PsrMessage('SAML2 ACS Login Failed', [
                    'exception' => $response->getErrorException()
                ], get_class($this) . '::' . __METHOD__));
                throw new \yii\web\HttpException(500);
            }
        }

        // If there is not a SAML2 response initiate an ACS request
        $auth = new Auth($settings);
        $auth->login();
    }

    /**
     * Initiates and handles a signed logout request
     * @return Response
     */
    public function actionSlo()
    {
        $settings = SamlModule::getInstance()
            ->samlModuleService
            ->getOneLoginSettings();

        $samlSettings = new Settings($settings);

        // Process an SLO request from the iDP
        if ($samlResponse = Craft::$app->request->get('SAMLResponse', null)) {
            $response = new LogoutResponse($samlSettings, $samlResponse);

            try {
                if ($response->isValid()) {
                    $result = SamlModule::getInstance()
                        ->samlModuleService
                        ->deauthenticate($response);

                    return Craft::$app->response->redirect('/log-out');
                }
                else {
                    throw new \yii\base\Exception('Failed to validate response');
                }
            } catch (\Exception $e) {
                Craft::error(new PsrMessage('SAML2 SLO Logout Failed', [
                    'exception' => $response->getErrorException()
                ], get_class($this) . '::' . __METHOD__));
                throw new \yii\web\HttpException(500);
            }
        }

        // Initiate an SLO request to the iDP
        $logoutRequest = new LogoutRequest($samlSettings);
        $parameters = ['SAMLRequest' => $logoutRequest->getRequest() ];
        $url = Utils::redirect($settings['idp']['singleLogoutService']['url'], $parameters, true);

        return Craft::$app->response->redirect($url);
    }
}