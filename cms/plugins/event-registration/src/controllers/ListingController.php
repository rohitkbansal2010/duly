<?php
namespace punchkick\eventregistration\controllers;

use Craft;
use craft\elements\Category;
use craft\elements\Entry;
use craft\elements\MatrixBlock;
use craft\web\Controller;
use craft\web\Response;
use punchkick\eventregistration\forms\EventSearchForm;
use modules\DupageCoreModule\DupageCoreModule;
use Solspace\Calendar\Calendar;
use Solspace\Calendar\Elements\Event;
use yii\db\Expression;
use yii\db\Query;
use yii\data\ArrayDataProvider;
use yii\web\UrlManager;
use yii\widgets\LinkPager;
use punchkick\eventregistration\EventRegistration;
use punchkick\eventregistration\EventRegistrationPlugin;

/**
 * Listing Controller
 *
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 */
class ListingController extends Controller
{
    /**
     * @var    bool|array Allows anonymous access to this controller's actions.
     *         The actions must be in 'kebab-case'
     * @access protected
     */
    protected $allowAnonymous = [
        'index', 'auto-suggestions'
    ];

    /**
     * Lists all available events
     *
     * @return string
     */
    public function actionIndex()
    {
        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);
        $model = new EventSearchForm;

        // Queries for filters
        $query = (new Query)
            ->select(new Expression(
                "TRIM(JSON_UNQUOTE(JSON_EXTRACT(field_location_addressLocation, '$.parts.city'))) AS city"
            ))
            ->from('matrixcontent_eventlocation')
            ->distinct()
            ->leftJoin('matrixblocks', 'matrixblocks.id = matrixcontent_eventlocation.elementId')
            ->leftJoin('elements', 'matrixblocks.ownerId = elements.id')
            ->where(['dateDeleted' => null])
            ->orderBy('city asc')
            ->all();

        $cities = \array_map(function ($location) {
            return $location['city'];
        }, $query);

        $eventTypes = Category::find()
            ->group('eventCategories')
            ->orderBy('title asc')
            ->all();

        $model->loadData(Craft::$app->request->get());

        $events = $model->getQuery();
        
        $pageNumber =Craft::$app->request->get('page') - 1;

        $dataProvider = new ArrayDataProvider([
            'allModels' => $events->all(),
            'pagination' => [
                'pageSize' => 10,
                'page' => $pageNumber,
                'totalCount' => $events->count(),
                'urlManager' => new UrlManager([
                    'enablePrettyUrl'=> true,
                    'enableStrictParsing' => true,
                    'showScriptName' => false,
                    'rules'=>[
                        '/events' => 'event-registration/listing/index',
                    ]
                ])
            ]
        ]);

        $pager = LinkPager::widget([
            'pagination' => $dataProvider->pagination,
        ]);

        $events = $dataProvider->getModels();

        $hasVirtualEvents = \array_filter(
            $events,
            function ($event) {
                return $event->eventLocation->one()->isVirtual;
            }
        );

        if ($hasVirtualEvents) {
            array_push($cities, 'Virtual');
        }

        if ($isAjax) {
            return $this->renderTemplate('/_events/_container', [
                'events' => $events,
                'eventTypes' => $eventTypes,
                'cities' => $cities,
                'pagination' => $dataProvider->pagination
            ]);
        }

        return $this->renderTemplate('_events/_listings', [
            'events' => $events,
            'eventTypes' => $eventTypes,
            'cities' => $cities,
            'pagination' => $dataProvider->pagination
        ]);
    }

    /**
     * @return mixed
     */
    public function actionAutoSuggestions()
    {
        $form = new EventSearchForm();
        $form->load([
            'EventSearchForm' => Craft::$app->request->get()
        ]);

        $entries = [];
        if ($form->search_event_attribute) {
            $entries = DupageCoreModule::getInstance()
                ->dupageCoreModuleService
                ->getAutoSuggestions(
                    $form->search_event_attribute,
                    ['events']
                );
        }

        $this->asJson($entries);
    }
}
