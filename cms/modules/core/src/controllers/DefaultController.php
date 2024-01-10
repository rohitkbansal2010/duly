<?php

namespace modules\DupageCoreModule\controllers;

use \yii\filters\VerbFilter;
use Craft;
use craft\elements\Entry;
use craft\web\Controller;
use GuzzleHttp\Exception\RequestException;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\forms\ContactUsForm;
use modules\DupageCoreModule\forms\ProductsListForm;
use modules\DupageCoreModule\forms\LoginForm;
use modules\DupageCoreModule\forms\SiteWideSearchForm;
use modules\DupageCoreModule\models\PatientUser;
use modules\DupageCoreModule\queue\EmailJob;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\data\ArrayDataProvider;
use yii\filters\AccessControl;
use yii\filters\Cors;
use yii\httpclient\Client as YiiHttpClient;
use yii\web\BadRequestHttpException;
use yii\web\UrlManager;

class DefaultController extends Controller
{
    /**
     * @var    bool|array Allows anonymous access to this controller's actions.
     *         The actions must be in 'kebab-case'
     * @access protected
     */
    protected $allowAnonymous = self::ALLOW_ANONYMOUS_LIVE;

    public function behaviors()
    {
        $behaviors = parent::behaviors();

        $behaviors['access'] = [
            'class' => AccessControl::class,
            'rules' => [
                [
                    'allow' => true,
                    'actions' => ['login', 'login-plain', 'search', 'auto-suggestions', 'oauth2-redirect', 'logout', 'contact-us', 'version', 'check-symptoms', 'deeplink-redirect', 'products'],
                    'roles' => ['?', '@'],
                ]
            ],
            'user' => 'patient_user',
            'denyCallback' => function ($rule, $action) {
                return $this->redirect('/');
            }
        ];

        $behaviors['corsFilter'] = [
            'class' => Cors::class,
            'cors' => [
                'Origin' => [getenv('DEFAULT_SITE_URL')],
                'Access-Control-Request-Method' => ['GET', 'POST'],
                'Access-Control-Request-Headers' => ['*'],
            ]
        ];

        $behaviors['verbs'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'login' => ['GET', 'POST'],
                'search'  => ['GET'],
                'auto-suggestions'  => ['GET'],
                'oauth2-redirect'  => ['GET'],
                'logout'  => ['GET'],
            ],
        ];

        return $behaviors;
    }
    
    /**
     * handles redirecting users to a specific URL from the CMS during epic outages
     */
    public function beforeAction($action)
    {
        $loginUrlIds = ['login', 'login-plain'];
        if (in_array($action->id, $loginUrlIds)) {
            $schedulingLoginRedirect = Craft::$app->globals->getSetByHandle('generalSiteConfig')['schedulingLoginRedirectUrl'];
            if (!empty($schedulingLoginRedirect)) {
                return $this->redirect($schedulingLoginRedirect, 302);
            } 
        }
        return parent::beforeAction($action);
    }

    /**
     * Performs a site-wide search and returns a list of matches.
     *
     * @return array
     */
    public function actionSearch()
    {
        $form = new SiteWideSearchForm();
        $form->load([
            'SiteWideSearchForm' => Craft::$app->request->get()
        ]);

        $results = [];
        $totalCount = 0;

        if ($form->validate()) {
            $searchResults = Craft::$app->cache->getOrSet("site_wide_search_for_{$form->query}, {$form->section}, {$form->page}, {$form->perPage}", function () use ($form) {
                return DupageCoreModule::getInstance()->dupageCoreModuleService->siteWideSearch($form->query, $form->section, $form->page, $form->perPage);
            }, 900);
            $totalCount = count($searchResults);
            $dataProvider = new ArrayDataProvider([
                'allModels' => $searchResults,
                'pagination' => [
                    'pageSize' => $form->perPage,
                    'page' => $form->page - 1,
                    'totalCount' => $totalCount,
                    'urlManager' => new UrlManager([
                        'enablePrettyUrl' => true,
                        'showScriptName' => false,
                        'rules' => [
                            '/<p>' => 'dupage-core-module/default/search',
                        ]
                    ])
                ]
            ]);
            $results = $dataProvider->getModels();
        }

        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        $query = $form->query;
        $section = Craft::$app->request->getQueryParam('section', '');

        if ($isAjax) {
            return $this->renderTemplate('_search-results/_results', [
                'results' => $results,
                'isAjax' => true,
                'query' => $query,
                'section' => $section,
                'totalCount' => $totalCount,
                'pagination' => $dataProvider->pagination
            ]);
        } else {
            return $this->renderTemplate('/search', [
                'results' => $results,
                'isAjax' => false,
                'query' => $query,
                'section' => $section,
                'totalCount' => $totalCount,
                'pagination' => $dataProvider->pagination
            ]);
        }
    }

    /**
     * @return mixed
     */
    public function actionAutoSuggestions()
    {
        $form = new SiteWideSearchForm();
        $form->load([
            'SiteWideSearchForm' => Craft::$app->request->get()
        ]);

        $entries = Craft::$app->cache->getOrSet("site_wide_search_suggestions_for_{$form->query}", function () use ($form) {
            return DupageCoreModule::getInstance()->dupageCoreModuleService->getAutoSuggestions(
                $form->query,
                ['physicians', 'blog', 'services', 'conditions', 'events']
            );
        }, 900);

        foreach ($entries as &$entry) {
            if (isset($entry['img']) && !empty($entry['img'])) {
                $entry['img'] = DupageCoreModule::getInstance()->dupageCoreModuleService->getOptimizedImage($entry['img'], 'jpg', true, [
                    ['settings' => ['gravity:sm', 'resize:fill']]
                ])[0];
            }

            if (isset($entry['physicianSpeciality']) && !empty($entry['physicianSpeciality'])) {
                $element = $entry['physicianSpeciality']->one();
                $entry['physicianSpeciality'] = [
                    'title' => $element->title,
                    'url' => $element->url
                ];
            }
        }

        $this->asJson($entries);
    }

    /**
     * @return mixed
     */
    public function actionOauth2Redirect()
    {
        // exchange the authorization code for patient ID
        $patientId = SchedulingModule::getInstance()->schedulingModuleEpicService->exchangeOauthCode(Craft::$app->request->get('code'));

        if (!$patientId) {
            Craft::error(
                Craft::t(
                    'dupage-core-module',
                    'Unable to exchange oauth2 authorization code.'
                ),
                __METHOD__
            );

            Craft::$app->session->setFlash('login-error', "Something went wrong. Please try again.");
            return $this->redirect('/login-portal');
        }

        // create a new PatientUser object
        $user = new PatientUser(['scenario' => PatientUser::SCENARIO_DEFAULT]);
        $user->id = $patientId;
        $user->anonymous = false;

        if ($user->save()) {
            // login using the new object
            Craft::$app->patient_user->login($user, (60 * 20));
            // on success, return the user back to the main scheduling page
            return $this->redirect('/schedule');
        } else {
            Craft::error(
                Craft::t(
                    'dupage-core-module',
                    'Unable to save PatientUser for given epic patient ID.'
                ),
                __METHOD__
            );

            Craft::$app->session->setFlash('login-error', "Something went wrong. Please try again.");
            return $this->redirect('/login-portal');
        }
    }

    /**
     * Facilitates authentication through the AuthenticateWebAccount API
     */
    public function actionLoginPlain()
    {
        Craft::$app->patient_user->setReturnUrl(null);
        return $this->login('_login.twig');
    }


    public function actionLogin()
    {
        return $this->login('_login-portal.twig');
    }

    /**
     * Facilitates authentication through the AuthenticateWebAccount API
     */
    private function login($viewFile = '_login-portal.twig')
    {
        $referrer = $this->getReferrer();

        $form = new LoginForm;

        if (Craft::$app->request->isPost) {
            if ($form->load(['LoginForm' => Craft::$app->request->post()])) {
                if ($form->login()) {
                    Craft::$app->session->setFlash('login-success', true);
                    Craft::$app->session->setFlash('show-patient-picker', true);
                    Craft::$app->patient_user->setReturnUrl(null);
                    return $this->redirect($referrer);
                } else {
                    Craft::$app->session->setFlash('login-failure', true);
                }
            }
        }

        $view = Craft::$app->getView();
        $view->setTemplatesPath(CRAFT_BASE_PATH . DIRECTORY_SEPARATOR . 'templates');

        return $this->renderTemplate($viewFile, [
            'form' => $form,
            'referrer' => $referrer
        ]);
    }

    /**
     * @return mixed
     */
    private function getReferrer()
    {
        $referrer = Craft::$app->request->referrer;
        $returnUrl = Craft::$app->patient_user->getReturnUrl();

        // ignore default yii\web\Application::$homeUrl redirect
        $returnUrl = $returnUrl == '/' ? null : $returnUrl;

        if ($returnUrl) {
            $referrer = $returnUrl;
        } elseif (isset($referrer) && \strpos($referrer, 'login-portal') === false && \strpos($referrer, 'login-customer') === false) {
            Craft::$app->patient_user->setReturnUrl($referrer);
        } else {
            $referrer =  Craft::$app->homeUrl;
        }

        $returnUrl = Craft::$app->request->get('return_uri');
        if ($returnUrl !== null && filter_var($returnUrl, FILTER_VALIDATE_URL)) {
            $referrer = $returnUrl;
        }

        return $referrer;
    }

    /**
     * @return mixed
     */
    public function actionLogout()
    {
        if (Craft::$app->patient_user->identity) {
            Craft::$app->patient_user->identity->resetUserData();
        }
        Craft::$app->patient_user->logout();
        $referrer = $this->getReferrer();
        Craft::$app->patient_user->setReturnUrl(null);

        if (getenv('ENABLE_SAML2') == true) {
            return $this->renderTemplate('/logout');
        } else {
            return $this->redirect($referrer);
        }
    }

    /**
     * @return mixed
     */
    public function actionVersion()
    {
        if (file_exists(Craft::getAlias('@root') . '/VERSION')) {
            $version = \json_decode(\file_get_contents(Craft::getAlias('@root') . '/VERSION'), true);
            Craft::$app->response->format = \yii\web\Response::FORMAT_JSON;
            return $version;
        } else {
            return $this->redirect('/');
        }
    }

    /**
     * @return mixed
     */
    public function actionContactUs()
    {
        $form = new ContactUsForm();

        if (Craft::$app->request->isPost) {
            $form->load([
                'ContactUsForm' => Craft::$app->request->post()
            ]);

            if ($form->validate()) {
                Craft::$app->queue->push(new EmailJob([
                    'template' => 'contact-us.twig',
                    'subject' => Craft::t('dupage-core-module', 'Contact Us Form Submission'),
                    'to' => Craft::$app->config->general->contact_us_email,
                    'templateData' => [
                        'emailAddress' => $form->emailAddress,
                        'name' => $form->name,
                        'address' => $form->address,
                        'city' => $form->city,
                        'state' => $form->state,
                        'zipcode' => $form->zipcode,
                        'phoneNumber' => $form->phoneNumber,
                        'subject' => $form->subject,
                        'comments' => $form->comments,
                    ],
                ]));
                return $this->redirect('/thank-you');
            }
        }

        return $this->renderTemplate('contact-us', [
            'model' => $form
        ]);
    }

    /**
     * @return mixed
     */
    public function actionCheckSymptoms()
    {
        if (Craft::$app->request->isPost) {
            $data = Craft::$app->request->post();
            unset($data[Craft::$app->config->general->csrfTokenName]);

            $immediateCareEntry = Entry::find()
                ->section('immediateCare')
                ->one();

            $request = Craft::$app->httpclient->getClient()->createRequest()
                ->setFormat(YiiHttpClient::FORMAT_JSON)
                ->setMethod("POST")
                ->addHeaders([
                    'Content-Type' => 'application/json',
                    'Accept' => 'application/json'
                ])
                ->setUrl($immediateCareEntry->immediateCareCheckSymptomsClearStepURL)
                ->setData($data);

            try {
                $response = Craft::$app->httpclient->getClient()->send($request);
                $response = $response->getContent();

                $ob = json_decode($response);
                if ($ob === null) {
                    Craft::error(
                        new PsrMessage('Unable to make a ClearStep Symptom Checker API call. Bad response.', [
                            'response' => $response
                        ])
                    );

                    throw new BadRequestHttpException(400);
                }

                return $response;
            } catch (\Exception $e) {
                Craft::error(
                    new PsrMessage('Unable to make a ClearStep Symptom Checker API call.', [
                        'error' => $e->getMessage()
                    ])
                );

                throw new BadRequestHttpException(400);
            }
        } else {
            Craft::error(
                new PsrMessage('Unable to make a ClearStep Symptom Checker API call. Not a POST call.')
            );
            throw new BadRequestHttpException(400);
        }
    }

    /**
     * @return mixed
     */
    public function actionDeeplinkRedirect($id = -1)
    {
        $deeplinkEntry = Entry::find()
            ->section('schedulingDeeplink')
            ->id($id)
            ->one();

        if ($deeplinkEntry === null) {
            Craft::error(
                new PsrMessage('Unable to parse this scheduling deeplinkg.', [
                    'id' => $id
                ])
            );
            throw new BadRequestHttpException(400);
        }

        $locationId = $deeplinkEntry->singleLocation ? $deeplinkEntry->singleLocation->one()->id : null;
        $physicianId = $deeplinkEntry->singlePhysician ? $deeplinkEntry->singlePhysician->one()->id : null;
        $serviceId = $deeplinkEntry->singleService ? $deeplinkEntry->singleService->one()->id : null;

        $user = Craft::$app->patient_user->identity;
        if (!$user) {
            $user = PatientUser::anonymousUser();
            Craft::$app->patient_user->login($user, (60 * 20));
        }
        // Using a scheduling deeplink, a user is effectively starting a new scheduling flow,
        // thus we should reset.
        $user->resetUserData();

        if ("physicianServiceSchedulingDeeplink" === $deeplinkEntry->type->handle) {
            $user->physician_selected_outside_of_scheduling = "1";

            // populate user object with deeplink data
            $user->appointment_physician_id = (string)$physicianId;
            $user->setAppointmentServiceIds([$serviceId]);
        } else if ("locationServiceSchedulingDeeplink" === $deeplinkEntry->type->handle) {
            $user->location_selected_outside_of_scheduling = "1";

            // populate user object with deeplink data
            $user->location_id = (string)$locationId;
            $user->setAppointmentServiceIds([$serviceId]);
        }

        Craft::$app->response->format = \yii\web\Response::FORMAT_JSON;
        if ($user->save()) {
            return $this->redirect('schedule/insurance');
        } else {
            Craft::error(
                new PsrMessage('Unable to proceed with this scheduling deeplinkg.', [
                    'id' => $id,
                    'singleLocationId' => $locationId,
                    'singlePhysicianId' => $physicianId,
                    'singleServiceId' => $serviceId,
                    'errors' => $user->getErrors()
                ])
            );
            throw new BadRequestHttpException(400);
        }
    }

    /**
     * @return mixed
     */
    public function actionProducts()
    {
        $form = new ProductsListForm();

        $form->load([
            'ProductsListForm' => Craft::$app->request->get()
        ]);

        $dataProvider = $form->getProducts();

        $pagination = $dataProvider->getPagination();

        if (Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false)) {
            return $this->renderTemplate('cosmetic-dermatology-products/products-list/_products-list.twig', [
                'products' => $dataProvider->getModels(),
                'brands' => $form->getBrands(),
                'conditions' => $form->getConditions(),
                'pagination' => $dataProvider->pagination
            ]);
        } else {
            return $this->renderTemplate('cosmetic-dermatology-products/products-list/_main.twig', [
                'products' => $dataProvider->getModels(),
                'brands' => $form->getBrands(),
                'conditions' => $form->getConditions(),
                'pagination' => $dataProvider->pagination
            ]);
        }
    }
}
