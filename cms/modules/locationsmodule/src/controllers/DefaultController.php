<?php
/**
 * Locations module for Craft CMS 3.x
 *
 * Allows for extended management of the locations section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\locationsmodule\controllers;

use Craft;
use craft\web\Controller;
use DateTime;
use modules\locationsmodule\forms\LocationSearchForm;
use modules\locationsmodule\forms\ClosestLocationForm;
use modules\locationsmodule\forms\LocationDetailForm;
use modules\locationsmodule\LocationsModule;
use modules\DupageCoreModule\DupageCoreModule;
use yii\data\ActiveDataProvider;
use yii\web\UrlManager;
use yii\widgets\LinkPager;
use craft\elements\Entry;

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
    protected $allowAnonymous = ['index', 'auto-suggestions','get-closest-locations', 'get-location-details', 'get-location-distance'];

    /**
     * @return mixed
     */
    public function actionIndex()
    {
        $form = new LocationSearchForm();
        $form->load([
            'LocationSearchForm' => Craft::$app->request->get()
        ]);

        $dataProvider = $form->getDataProvider("locations-module/default/index");

        $isAjax = Craft::$app->getRequest()->getHeaders()->get('x-isAjax', false);
        $locations = $dataProvider->getModels();

        $longitude = (float)$form->longitude;
        $latitude = (float)$form->latitude;

        $services = Entry::find()
            ->section('services')
            ->select(['title', 'elements.id'])
            ->level(1)
            ->displayInProviderSpecialtiesFilter(1)
            ->distinct()
            ->orderBy('title ASC')
            ->all();
        
        if ($isAjax) {
            return $this->renderTemplate('_locations/_table', [
                'locations' => $locations,
                'pagination' => $dataProvider->pagination,
                'services' => $services,
                'userLat' => $latitude,
                'userLng' => $longitude
            ]);
        }
        
        return $this->renderTemplate('_locations/_main', [
            'locations' => $locations,
            'pagination' => $dataProvider->pagination,
            'services' => $services,
            'userLat' => $latitude,
            'userLng' => $longitude
        ]);
    }

    /**
    * @return mixed
    */
    public function actionGetLocationDetails()
    {
        $form = new LocationDetailForm();
        $form->load([
            'LocationDetailForm' => Craft::$app->request->get()
        ]);

        $physicians = LocationsModule::getInstance()
            ->locationsModuleService
            ->getLocationDetailPhysicians($form);

        $locations = LocationsModule::getInstance()
            ->locationsModuleService
            ->getLocationDetailRelatedOffices($form);

        $service = LocationsModule::getInstance()
            ->locationsModuleService
            ->getService($form);
    
        return $this->renderTemplate('_locations/single/_single_location_details', [
            'physicians' => $physicians,
            'locations' => $locations,
            'service' => $service,
            'primaryLocationId' => $form->locationId
        ]);
    }

    /**
     * @return mixed
     */
    public function actionAutoSuggestions()
    {
        $form = new LocationSearchForm();
        $form->load([
            'LocationSearchForm' => Craft::$app->request->get()
        ]);

        $entries = [];
        if ($form->search_service_attribute) {
            $entries = DupageCoreModule::getInstance()
                ->dupageCoreModuleService
                ->getAutoSuggestions(
                    $form->search_service_attribute,
                    ['services']
                );
        } elseif ($form->search_location) {
            $entries = DupageCoreModule::getInstance()
                ->dupageCoreModuleService
                ->getAutoSuggestions(
                    $form->search_location,
                    ['locations']
                );
        }

        $this->asJson($entries);
    }

    /**
     * @return mixed
     */
    public function actionGetClosestLocations()
    {
        $form = new ClosestLocationForm();
        $form->load([
            'ClosestLocationForm' => Craft::$app->request->get()
        ]);

        $immediateCareLocation = LocationsModule::getInstance()
            ->locationsModuleService
            ->getClosestLocation($form, true);

        $expressCareLocation = LocationsModule::getInstance()
            ->locationsModuleService
            ->getClosestLocation($form, false);

        $userLat = $form->latitude;
        $userLng = $form->longitude;

        return $this->renderTemplate('_immediate-care/locations/_locations-container', [
            'immediateCareLocation' => $immediateCareLocation,
            'expressCareLocation' => $expressCareLocation,
            'userLat' => $userLat,
            'userLng' => $userLng
        ]);
    }

    /**
     * @return mixed
     */
    public function actionGetLocationDistance()
    {
        $form = new ClosestLocationForm();
        $form->load([
            'ClosestLocationForm' => Craft::$app->request->get()
        ]);

        $location = LocationsModule::getInstance()
            ->locationsModuleService
            ->getLocation($form);
            
        $userLat = $form->latitude;
        $userLng = $form->longitude;

        return $this->renderTemplate('_locations/single/dynamic/_distance', [
            'userLat' => $userLat,
            'userLng' => $userLng,
            'location' => $location
        ]);
    }
}
