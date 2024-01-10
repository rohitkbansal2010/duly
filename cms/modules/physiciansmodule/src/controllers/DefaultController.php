<?php
/**
 * Physicians module for Craft CMS 3.x
 *
 * Allows for extended management of the physicians section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\physiciansmodule\controllers;

use Craft;
use craft\elements\Entry;
use craft\web\Controller;
use DateInterval;
use DatePeriod;
use DateTime;
use DateTimezone;
use modules\DupageCoreModule\DupageCoreModule;
use modules\physiciansmodule\forms\PhysicianSearchForm;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\data\ActiveDataProvider;
use yii\web\UrlManager;
use yii\widgets\LinkPager;

/**
 * @author    Wipfli Digital
 * @package   PhysiciansModule
 * @since     1.0.0
 */
class DefaultController extends Controller
{
    /**
     * @var    bool|array Allows anonymous access to this controller's actions.
     *         The actions must be in 'kebab-case'
     * @access protected
     */
    protected $allowAnonymous = ['index', 'auto-suggestions'];

    /**
     * @return mixed
     */
    public function actionIndex()
    {
        $form = new PhysicianSearchForm();
        $form->load([
            'PhysicianSearchForm' => Craft::$app->request->get()
        ]);

        $query = PhysiciansModule::getInstance()->physiciansModuleService->queryPhysicians($form);
        $query->distinct();
        
        $pageSize = 10;
        $dataProvider = new ActiveDataProvider([
            'query' => $query,
            'pagination' => [
                'pageSize' => $pageSize,
                'totalCount' => $query->count(),
                'urlManager' => new UrlManager([
                    'enablePrettyUrl'=> true,
                    'showScriptName' => false,
                    'rules'=>[
                        '/<p>' => 'physicians-module/default/index',
                    ]
                ])
            ]
        ]);

        // update "next available appointment" fields
        $nextPageOfPhysicians = $dataProvider->getModels();
        $this->updateNextAvailableAppointmentFields($nextPageOfPhysicians);
        $query->offset(null)->limit(null);

        // refresh data provider to refresh the models
        $dataProvider->refresh();

        // calculate two next days since beginDate
        $dates = [];
        for ($i = 0; $i < 3; $i++) {
            $date = DateTime::createFromFormat($form->dateFormat, $form->beginDate);
            $date->modify("+{$i} day");
            \array_push(
                $dates,
                $date
            );
        }

        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);

        $services = Entry::find()
            ->section('services')
            ->select(['title', 'elements.id'])
            ->level(1)
            ->displayInProviderSpecialtiesFilter(1)
            ->distinct()
            ->orderBy('title ASC')
            ->all();

        if ($isAjax) {
            return $this->renderTemplate('_physicians/_physicians-list-container.twig', [
                'physicians' => $dataProvider->getModels(),
                'services' => $services,
                'pagination' => $dataProvider->pagination,
                'dates' => $dates,
                'form' => $form,
                'isAjax' => $isAjax
            ]);
        }
        
        return $this->renderTemplate('_physicians/physicians.twig', [
            'physicians' => $dataProvider->getModels(),
            'services' => $services,
            'pagination' => $dataProvider->pagination,
            'dates' => $dates,
            'form' => $form,
            'isAjax' => $isAjax
        ]);
    }
    
    private function updateNextAvailableAppointmentFields($physicians)
    {
        // ensure one-round trip
        $transaction = Craft::$app->db->beginTransaction();
        try {
            // for each physician, check if the currently-saved physicianNextAvailableAppointmentTime date is in the past
            // if it is, try to find the next available date (from cache) that is not in the past
            foreach ($physicians as $physician) {
                // stored in the CMS with Central timezone
                $savedPhysicianNextAvailableAppointmentTime = DateTime::createFromFormat("Y-m-d H:i:s", $physician->physicianNextAvailableAppointmentTime, new DateTimezone("US/Central"));

                // if saved next avilable date is in the past, purge it and update with the next available date (if any)
                $now = new DateTime(null, new DateTimezone("US/Central"));

                if ($savedPhysicianNextAvailableAppointmentTime < $now) {
                    $newDate = PhysiciansModule::getInstance()->physiciansModuleService->getAvailablePhysicianAppointmentFromDate($physician, $now);
                    
                    if ($newDate != null) {
                        $newDate = $newDate->format("Y-m-d H:i:s");
                    }

                    if ($savedPhysicianNextAvailableAppointmentTime != null) {
                        $savedPhysicianNextAvailableAppointmentTime = $savedPhysicianNextAvailableAppointmentTime->format("Y-m-d H:i:s");
                    }

                    $cmd = Craft::$app->db->createCommand()
                        ->update(
                            'content',
                            ['field_physicianNextAvailableAppointmentTime' => $newDate],
                            'elementId = :physicianId',
                            [':physicianId' => $physician->id]
                        );
                    if (!$cmd->execute()) {
                        Craft::error(new PsrMessage('Unable to update field_physicianNextAvailableAppointmentTime.', [
                            'previousFieldValue' => $savedPhysicianNextAvailableAppointmentTime,
                            'newFieldValue' => $newDate,
                            'physicianId' => $physician->id
                        ]));
                    }
                }
            }

            // issue the updates
            $transaction->commit();
        } catch (\Exception | \Throwable $e) {
            $transaction->rollBack();
            throw $e;
        }
    }

    /**
     * @return mixed
     */
    public function actionAutoSuggestions()
    {
        $form = new PhysicianSearchForm();
        $form->load([
            'PhysicianSearchForm' => Craft::$app->request->get()
        ]);

        $entries = [];
        if ($form->search_physician_attribute) {
            $entries = DupageCoreModule::getInstance()->dupageCoreModuleService->getAutoSuggestions(
                $form->search_physician_attribute,
                ['physicians', 'services', 'procedures', 'conditions']
            );
        } elseif ($form->address) {
            $entries = DupageCoreModule::getInstance()->dupageCoreModuleService->getAutoSuggestions(
                $form->address,
                ['locations']
            );
        }

        $this->asJson($entries);
    }
}
