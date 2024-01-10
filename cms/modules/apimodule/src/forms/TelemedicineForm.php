<?php

namespace modules\apimodule\forms;

use Craft;
use craft\elements\Category;
use craft\elements\Entry;
use DateTime;
use modules\physiciansmodule\PhysiciansModule;
use yii\base\Model;
use yii\data\ArrayDataProvider;

/**
 * Telemedicine Form
 *
 * This form wraps the logic for requesting a list of telemedicine appointment times based on provided parameters.
 *
 * This form is tailored for use in /api endpoints.
 */
final class TelemedicineForm extends Model
{
    /**
     * @var int $pageSize
     */
    public $pageSize = 15;

    /**
     * This method returns a list of telemedicine appointment times.
     *
     * @return array
     */
    public function getTelemedicineAppointments()
    {
        $telemedicineServiceCategories = Category::find()
            ->group('telemedicineServices')
            ->with('assignedService')
            ->all();

        $telemedicineServices = [];
        
        foreach ($telemedicineServiceCategories as $category) {
            $telemedicineServices = \array_merge($telemedicineServices, $category->assignedService);
        }

        $data = [];

        foreach ($telemedicineServices as $telemedicineService) {
            $appointmentData = $this->getAppointmentSlots($telemedicineService);

            if (count($appointmentData) == 0) {
                continue;
            }

            $data[] = [
                'id' => $appointmentData['locationId'],
                'title' => $telemedicineService->title,
                'appointment_slots' => $appointmentData['appointmentSlots']
            ];
        }

        // this takes care of the pagination for us
        $dataProvider = new ArrayDataProvider([
            'allModels' => $data,
            'pagination' => [
                'pageSize' => $this->pageSize,
                'totalCount' => count($data)
            ]
        ]);

        return $dataProvider;
    }

    /**
     * This method returns cached appointment times for given location.
     */
    private function getAppointmentSlots($service)
    {
        $telemedicineMetadata = Craft::$app->cache->getOrSet("telemedicine_metadata_per_service_{$service->id}", function () use ($service) {
            $suites = Entry::find()
                ->section('locations')
                ->type('suite')
                ->with('suiteServices');

            foreach ($suites->each() as $suite) {
                foreach ($suite->suiteServices as $suiteService) {
                    // if this service does not offer scheduling with external resource, skip
                    if ($suiteService->externalProviderResourceId === null) {
                        continue;
                    }
    
                    $serviceType = $suiteService->serviceType->one();
                    if ($serviceType == null || $serviceType->id !== $service->id) {
                        continue;
                    }

                    return [
                        'externalProviderResourceId' => $suiteService->externalProviderResourceId,
                        'locationId' => $suite->parent->id
                    ];
                }
            }

            return null;
        }, 1);

        $slots = [];

        if ($telemedicineMetadata === null) {
            return $slots;
        }

        $appointmentSlotsPerDay = PhysiciansModule::getInstance()
            ->physiciansModuleService
            ->getCachedAppointmentTimesForExternalResourceFromDate($telemedicineMetadata['externalProviderResourceId'], new DateTime());

        foreach ($appointmentSlotsPerDay as $appointmentSlots) {
            // if there are no appointments for this day key, skip
            if (!$appointmentSlots) {
                continue;
            }

            foreach ($appointmentSlots as $appointmentSlot) {
                // skip appointment slots with non-matching visit type code for video visits
                if ($appointmentSlot['VisitType']['ID'] != 2990) {
                    continue;
                }

                // skip appointment slots older than now; potential side-effect of using cached data
                if ($appointmentSlot['Time'] < new DateTime()) {
                    continue;
                }

                if (count($slots) > 27) {
                    break;
                }

                $slots[] = $appointmentSlot['Time']->format('c');
            }
        }

        return [
            'appointmentSlots' => $slots,
            'locationId' => $telemedicineMetadata['locationId']
        ];
    }
}
