<?php
/**
 * Request Appointment for Craft CMS 3.x
 *
 * Allows for extended management of submitting request form for appointment.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace punchkick\requestappointment\controllers;

use Craft;
use craft\elements\Entry;
use craft\web\Controller;
use craft\web\Response;
use DateTime;
use punchkick\requestappointment\forms\RequestAppointmentForm;
use punchkick\requestappointment\records\RequestAppointmentRecord;
use punchkick\requestappointment\RequestAppointment;
use punchkick\requestappointment\services\RequestAppointmentService;
use yii\base\Model;
use yii\data\ActiveDataProvider;
use yii\web\UrlManager;
use yii\widgets\LinkPager;

/**
 * @author    Wipfli Digital
 * @package   LocationsModule
 * @since     1.0.0
 */
class DefaultController extends Controller
{
    /**
     * @var    bool|array Allows anonymous access to this controller's actions.
     *         The actions must be in 'kebab-case'
     * @access protected
     */
    protected $allowAnonymous = ['index'];

    /**
     * @return mixed
     */
    public function actionIndex()
    {
        $model = new RequestAppointmentForm;
        $showSuccessMessage = false;
        $queryParam = Craft::$app->request->getQueryParam('returnUrl', null);
        $returnUrl = null;
        $path = Craft::$app->request->getPathInfo();
        $form = null;

        if ($queryParam) {
            if (\strpos($queryParam, getenv('DEFAULT_SITE_URL')) !== false && filter_var($queryParam, FILTER_VALIDATE_URL)) {
                $returnUrl = $queryParam;
            }
        }
        // this is only used in the PT/OT for now
        $locations = RequestAppointment::getInstance()
            ->requestAppointmentService
            ->getServiceLocationsForRequestForm('physicalAndOccupationalTherapyServices');

        if (Craft::$app->request->isPost) {
            if ($path == 'schedule/physical-occupational-therapy') {
                if ($model->load(['RequestAppointmentForm' => Craft::$app->request->post()])) {
                    if ($model->save()) {
                        $showSuccessMessage = true;
                    }
                }
            } elseif ($path == 'schedule/cosmetic-dermatology') {
                if ($model->load(['RequestAppointmentForm' => Craft::$app->request->post()])) {
                    if ($model->sendCosmeticDermatologyEmail()) {
                        $showSuccessMessage = true;
                    }
                }
            }
        }

        if ($path == 'schedule/physical-occupational-therapy') {
            $form = './_scheduling/forms/_physical-occupational-form.twig';
        } elseif ($path == 'schedule/cosmetic-dermatology') {
            $form = './_scheduling/forms/_cosmetic-dermatology-form.twig';
        }

        return $this->renderTemplate($form, [
            'locations' => $locations,
            'model' => $model,
            'returnUrl' => $returnUrl,
            'showSuccessMessage' => $showSuccessMessage
        ]);
    }
}
