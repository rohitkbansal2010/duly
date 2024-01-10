<?php
namespace punchkick\eventregistration\controllers;

use Craft;
use craft\web\Controller;
use craft\web\Response;
use GuzzleHttp\Client;
use samdark\log\PsrMessage;
use yii\caching\Cache;
use yii\filters\AccessControl;
use yii\filters\Cors;
use yii\filters\RateLimiter;
use yii\filters\VerbFilter;
use yii\web\NotFoundHttpException;
use yii\web\ServerErrorHttpException;

/**
 * Listing Controller
 *
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 */
class ServiceController extends Controller
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
                    'actions' => ['youtube-api'],
                    'roles' => ['?', '@'],
                ],
            ],
            'user' => 'patient_user'
        ];

        $behaviors['corsFilter'] = [
            'class' => Cors::class,
            'cors' => [
                'Origin' => [getenv('DEFAULT_SITE_URL')],
                'Access-Control-Request-Method' => ['GET'],
                'Access-Control-Request-Headers' => ['*'],
            ]
        ];

        $behaviors['verbs'] = [
            'class' => VerbFilter::class,
            'actions' => [
                'youtube-api'  => ['GET'],
            ],
        ];

        $behaviors['rateLimiter'] = [
            'class' => RateLimiter::class,
            'user' => Craft::$app->patient_user->identity,
        ];

        return $behaviors;
    }

    /**
     * YouTube API Call returns json object
     *
     * @return Response
     * @throws NotFoundHttpException
     */
    public function actionYoutubeApi()
    {
        $params = Craft::$app->getRequest()->queryParams;
        
        if (isset($params['id'])) {
            $id = $params['id'];

            $data = Craft::$app->cache->getOrSet(['youtube-api-response', 'id' => $id], function () use ($id) {
                try {
                    return $this->getApiResponse($id);
                } catch (\Exception $e) {
                    return false;
                }
            }, 86400);

            if (!empty($data)) {
                Craft::$app->response->format = Response::FORMAT_JSON;
                return $data;
            }
        }

        throw new NotFoundHttpException;
    }

    /**
     * Retrieves the youtube API response
     *
     * @param string $id
     * @return array
     * @throws ServerErrorHttpException;
     */
    private function getApiResponse(string $id)
    {
        $url = 'https://www.youtube.com/oembed?url=http://www.youtube.com/watch?v='.$id.'&format=json';
        $client = Craft::$app->httpclient->getClient();
        $request = $client->createRequest()
            ->setMethod('GET')
            ->setUrl($url);
        
        $response = $request->send();

        if ($response->getHeaders()->get('http-code') != 200) {
            Craft::error(new PsrMessage('Youtube API Call Failed', [
                'url' =>  $url,
                'headers' => $response->getHeaders()->toArray(),
                'response' => $response->getContent()
            ]), get_class($this) . '::' . __METHOD__);

            throw new ServerErrorHttpException;
        }

        return $response->getData();
    }
}
