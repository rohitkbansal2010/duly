<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\eventregistration\controllers;

use Craft;
use craft\elements\MatrixBlock;
use craft\helpers\Html;
use craft\web\Controller;
use craft\web\Response;
use League\Csv\Writer;
use punchkick\eventregistration\EventRegistration;
use punchkick\eventregistration\EventRegistrationPlugin;
use punchkick\eventregistration\forms\EventRegistrationForm;
use punchkick\eventregistration\forms\EventRegistrationsListForm;
use punchkick\eventregistration\records\EventRegistrationRecord;
use Solspace\Calendar\Calendar;
use Solspace\Calendar\Elements\Event;
use yii\data\ActiveDataProvider;
use yii\db\Query;
use yii\helpers\Inflector;
use yii\web\HttpException;
use yii\web\NotFoundHttpException;

/**
 * Default Controller
 *
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 */
class DefaultController extends Controller
{
    /**
     * @var    bool|array Allows anonymous access to this controller's actions.
     *         The actions must be in 'kebab-case'
     * @access protected
     */
    protected $allowAnonymous = [
        'register'
    ];

    /**
     * Lists all available locations for a particular event
     *
     * @param int $eventId
     * @return string
     */
    public function actionIndex($eventId)
    {
        $event = Event::find()
            ->id($eventId)
            ->one();
        
        if ($event === null) {
            throw new NotFoundHttpException;
        }

        return $this->renderTemplate('event-registration/default', [
            'event' => $event
        ]);
    }

    public function actionRegistrationsByLocation(
        int $eventId,
        int $locationId,
        string $minDate = null,
        string $maxDate = null,
        bool $ajax = false
    ) {
        Craft::$app->response->format = Response::FORMAT_JSON;
        $model = new EventRegistrationsListForm;

        $model->load(['EventRegistrationsListForm' => [
            'eventId' => $eventId,
            'locationId' => $locationId,
            'minDate' => $minDate,
            'maxDate' => $maxDate
        ]]);

        if (!$model->validate()) {
            throw new HttpException(400, Craft::t('event-registration', 'Something went wrong. Please try again later.'));
        }

        $query = $model->getQuery(true);

        if (!isset($query)) {
            throw new HttpException(400, Craft::t('event-registration', 'Something went wrong. Please try again later.'));
        }

        $query = $query->orderBy('occurrenceDate')->groupBy('occurrenceDate');
        
        $event = $model->getEvent();
        $location = $model->getLocation();

        $dataProvider = new ActiveDataProvider([
            'query' => $query,
            'pagination' => [
                'pageSize' => 10,
                'totalCount' => $query->count()
            ],
            'sort' => [
                'attributes' => [
                    'occurrenceDate'
                ]
            ],
        ]);

        // fixing broken pagination links
        $dataProvider->pagination->urlManager = new \yii\web\UrlManager();

        // remove ajax query param on full-page pagination
        $dataProvider->pagination->params = $_GET;
        unset($dataProvider->pagination->params['ajax']);

        $pager = \yii\widgets\LinkPager::widget([
            'pagination' => $dataProvider->pagination,
        ]);

        if ($ajax == true) {
            return $this->renderTemplate('event-registration/registrants-list', [
                'event' => $event,
                'location' => $location,
                'dataProvider' => $dataProvider,
                'pager' => $pager,
            ]);
        }
        
        return $this->renderTemplate('event-registration/registrants-by-location', [
            'event' => $event,
            'location' => $location,
            'dataProvider' => $dataProvider,
            'pager' => $pager,
        ]);
    }

    public function actionCsv()
    {
        Craft::$app->response->format = Response::FORMAT_JSON;
        $model = new EventRegistrationsListForm;

        $model->load(['EventRegistrationsListForm' => Craft::$app->request->post()]);

        if (!$model->validate()) {
            throw new HttpException(400, Craft::t('event-registration', 'Something went wrong. Please try again later.'));
        }

        return Craft::$app->response->sendContentAsFile(
            $model->getRegistrationsListAsCSV(),
            $model->getCsvFilename(),
            [
                'mimeType' => 'text/csv'
            ]
        );
    }

    /**
     * Public event registration
     *
     * @return array
     */
    public function actionRegister()
    {
        Craft::$app->response->format = Response::FORMAT_JSON;
        $model = new EventRegistrationForm;

        if (Craft::$app->request->isPost) {
            if ($model->load(['EventRegistrationForm' => Craft::$app->request->post()])) {
                if ($model->save()) {
                    return [
                        'error' => '',
                        'errors' => [],
                        'success' => true
                    ];
                }

                Craft::$app->response->statusCode = 400;
                return [
                    'error' => '',
                    'errors' => $model->getErrors(),
                    'success' => false
                ];
            }
        }

        Craft::$app->response->statusCode = 400;
        return [
            'error' => Craft::t('event-registration', 'Something went wrong. Please try again later.'),
            'success' => false
        ];
    }
}
