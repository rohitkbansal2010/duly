<?php

namespace modules\apimodule\forms;

use ArrayObject;
use Craft;
use DateTime;
use craft\elements\Category;
use craft\elements\Entry;
use modules\apimodule\ApiModule;
use modules\locationsmodule\LocationsModule;
use modules\locationsmodule\forms\LocationSearchForm;
use modules\physiciansmodule\PhysiciansModule;

use yii\base\Model;
use yii\data\ActiveDataProvider;
use yii\data\ArrayDataProvider;
use yii\web\UrlManager;

/**
 * Location Form
 *
 * This form wraps the logic for requesting a list of locations based on provided parameters.
 * It extends the existing LocationSearchForm that performs most of the search.
 *
 * This form is tailored for use in /api endpoints.
 */
final class LocationForm extends LocationSearchForm
{
    /**
     * @var int $service_id
     */
    public $service_id;

    /**
     * @var int $zip_code
     */
    public $zip_code;

    /**
     * @var float $latitude
     */
    public $latitude;

    /**
     * @var float $longitude
     */
    public $longitude;

    /**
     * @var int $pageSize
     */
    public $pageSize = 15;

    /**
     * @var int $visit_reason_id
     */
    public $visit_reason_id;

    /**
     * @var int $is_new_patient
     */
    public $is_new_patient;

    /**
     * @var Entry $serviceEntry
     */
    private $serviceEntry = null;

    /**
     * @inheritdoc
     */
    public function load($data, $formName = null)
    {
        if (!parent::load($data, $formName)) {
            return false;
        }

        if (isset($this->service_id)) {
            $service = Entry::find()->id($this->service_id)->one();
            if ($service) {
                $this->setServiceEntry($service);
                $this->service = [$this->getServiceEntry()->title];
                $this->search_service_id = $this->getServiceEntry()->id;
            } else {
                $this->service = [null];
            }
        }

        if (isset($this->zip_code)) {
            $this->search_location = $this->zip_code;
        }

        if (isset($this->visit_reason_id)) {
            $this->reasonForVisitId = $this->visit_reason_id;
        }

        if (isset($this->is_new_patient)) {
            $this->is_new_patient = $this->is_new_patient === 'true' ? true: false;
        }

        return true;
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['service_id', 'latitude', 'longitude', 'zip_code', 'visit_reason_id'], 'number'],
            [['is_new_patient'], 'boolean'],
            [['visit_reason_id'], 'required', 'when' => fn ($model) => $this->serviceRequiresVisitReasonId($model)],
            [['service_id'], 'validateServiceId'],
            [['visit_reason_id'], 'validateVisitReasonId'],
            [['service_id', 'latitude', 'longitude', 'zip_code', 'visit_reason_id', 'is_new_patient'], 'safe']
        ];
    }

    /**
     * Determines if the visit reason parameter is required.
     *
     * Certain specialties will require a reason for visit.
     * This enforces that when such a service is supplied, a reason for visit is supplied as well.
     *
     * @return boolean
     */
    private function serviceRequiresVisitReasonId($model)
    {
        if (!$this->getServiceEntry()) {
            return false;
        }

        $isExpressCareService = $this->service_id == Craft::$app->cache->getOrSet("express_care_service_id", function () {
            $expressCareServiceIds = Category::find()->group("expressCareServices")->one()->assignedService->ids();
            return $expressCareServiceIds[0] ?? null;
        }, 3600 * 24);

        $isImmediateCareService = $this->service_id == Craft::$app->cache->getOrSet("immediate_care_service_id", function () {
            $immediateCareServiceIds = Category::find()->group("immediateCareServices")->one()->assignedService->ids();
            return $immediateCareServiceIds[0] ?? null;
        }, 3600 * 24);

        if (
            ($this->getServiceEntry()->appointmentSchedulingReasonsForVisit->count()) > 0
            && !isset($model->visit_reason_id)
            && !$isExpressCareService
            && !$isImmediateCareService
        ) {
            $this->addError('visit_reason_id', 'Visit Reason Id is required for this service');
            return true;
        }

        return false;
    }

    /**
     * Validates the provided visit reason id.
     *
     * Certain specialties will require a reason for visit.
     * This enforces that when such a service is supplied, it is valid.
     *
     * @return boolean
     */
    public function validateServiceId()
    {
        if ($this->getServiceEntry() && !$this->getServiceEntry()->schedulingWithoutPhysicians) {
            $this->addError('service_id', 'This service does not offer scheduling with a location.');
            return false;
        }

        return true;
    }

    /**
     * Validates the provided visit reason id.
     *
     * Certain specialties will require a reason for visit.
     * This enforces that when such a service is supplied, it is valid.
     *
     * @return boolean
     */
    public function validateVisitReasonId()
    {
        if (!$this->getServiceEntry()) {
            return true;
        }

        if (
            $this->visit_reason_id
            && !\in_array($this->visit_reason_id, $this->getServiceEntry()->appointmentSchedulingReasonsForVisit->ids())
        ) {
            $this->addError('visit_reason_id', 'Invalid Visit Reason Id for this service.');
            return false;
        }

        return true;
    }

    /**
     * This method returns a list of location objects and their data.
     *
     * @return array
     */
    public function getLocations()
    {
        if (!$this->validate()) {
            return [];
        }

        $allLocations = $query = LocationsModule::getInstance()
            ->locationsModuleService
            ->queryLocations($this);

        $attributes = $this->getAttributes();
        unset($attributes['zip']);
        unset($attributes['longitude']);
        unset($attributes['latitude']);
        $hashSlot2 = hash('sha512', 'LocationsForm_allLocationsWithSchedules__' . igbinary_serialize($attributes));

        $dpData = Craft::$app->cache->getOrSet($hashSlot2, function () use ($allLocations) {
            $locations = [];
            foreach ($allLocations->each() as $location) {
                $appointments = $this->getAppointmentSlots($location);
                if (count($appointments) == 0) {
                    continue;
                }

                $locations[] = [
                    'id' => $location->id,
                    'address' => [
                        'number' => $location->address->parts->number,
                        'street' => $location->address->parts->address,
                        'city' => $location->address->parts->city,
                        'postcode' => $location->address->parts->postcode,
                        'county' => $location->address->parts->county,
                        'state' => $location->address->parts->state,
                        'lat' => (float) $location->address->lat,
                        'lng' => (float) $location->address->lng
                    ],
                    'url' => $location->url,
                    'appointment_slots_per_service' => $appointments
                ];
            }

            return $locations;
        }, 1);

        // this takes care of the pagination for us
        $dataProvider = new ArrayDataProvider([
            'allModels' => $dpData,
            'pagination' => [
                'pageSize' => $this->pageSize,
                'totalCount' => count($dpData)
            ]
        ]);

        return $dataProvider;
    }

    /**
     * This method returns cached appointment times for given location.
     * It takes into account any additionally supplied form parameters and tailors the results.
     * For example, given a specific service, this method will not return appointment times for other services.
     */
    private function getAppointmentSlots($location)
    {
        $slots = new ArrayObject;

        foreach ($location->getChildren()->each() as $suite) {
            foreach ($suite->suiteServices->each() as $service) {
                // if this service does not offer scheduling with external resource, skip
                if ($service->externalProviderResourceId === null) {
                    continue;
                }

                $serviceType = $service->serviceType->one();
                if ($serviceType == null) {
                    continue;
                }

                $serviceId = $serviceType->id;

                // get cached appointment times for this service at this location for this external resource ID
                $appointmentSlotsPerDay = PhysiciansModule::getInstance()
                    ->physiciansModuleService
                    ->getCachedAppointmentTimesForExternalResourceFromDate($service->externalProviderResourceId, new DateTime());

                $visitTypeCode = ApiModule::getInstance()
                    ->apiModuleService
                    ->getVisitTypeCodeForService($serviceId, $this->is_new_patient ?? false);

                $serviceSlots = new ArrayObject;
                $serviceSlots[$serviceId] = [
                    'suite' => $suite->title,
                    'service' => $serviceType->title,
                    'times' => []
                ];

                if ($this->visit_reason_id !== null) {
                    $serviceReasonsForVisit = Entry::find()
                        ->section('serviceReasonsForVisit')
                        ->relatedTo($suite)
                        ->ids();

                    if (!in_array($this->visit_reason_id, $serviceReasonsForVisit)) {
                        continue;
                    }
                }

                foreach ($appointmentSlotsPerDay as $appointmentSlots) {
                    // if there are no appointments for this day key, skip
                    if (!$appointmentSlots) {
                        continue;
                    }

                    foreach ($appointmentSlots as $appointmentSlot) {
                        // skip appointment slots with non-matching visit type code
                        if ($appointmentSlot['VisitType']['ID'] != $visitTypeCode) {
                            continue;
                        }

                        // skip appointment slots older than now; potential side-effect of using cached data
                        if ($appointmentSlot['Time'] < new DateTime()) {
                            continue;
                        }

                        if (count($serviceSlots[$serviceId]['times']) > 27) {
                            break;
                        }

                        if ($this->getServiceEntry()) {
                            // do not include appointments for other services if a specific service was requested
                            if ($serviceId == $this->getServiceEntry()->id) {
                                $serviceSlots[$serviceId]['times'][] = $appointmentSlot['Time']->format('c');
                            }
                            continue;
                        }

                        $serviceSlots[$serviceId]['times'][] = $appointmentSlot['Time']->format('c');
                    }
                }

                if (count($serviceSlots[$serviceId]['times']) != 0) {
                    $slots[] = $serviceSlots;
                }
            }
        }

        return $slots;
    }

    /**
     * Get $serviceEntry
     *
     * @return  Entry
     */
    public function getServiceEntry()
    {
        return $this->serviceEntry;
    }

    /**
     * Set $serviceEntry
     *
     * @param  Entry  $serviceEntry  $serviceEntry
     *
     * @return  self
     */
    public function setServiceEntry(Entry $serviceEntry)
    {
        $this->serviceEntry = $serviceEntry;

        return $this;
    }
}
