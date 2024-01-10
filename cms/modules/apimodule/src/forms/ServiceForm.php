<?php

namespace modules\apimodule\forms;

use ArrayObject;
use Craft;
use DateTime;
use craft\elements\Category;
use craft\elements\Entry;
use craft\elements\MatrixBlock;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\models\Locations;
use modules\apimodule\ApiModule;

use modules\physiciansmodule\PhysiciansModule;
use yii\base\Model;
use yii\data\ArrayDataProvider;

/**
 * Physician Form
 *
 * This form wraps the logic for requesting a list of physicians based on provided parameters.
 *
 * This form is tailored for use in /api endpoints.
 */
final class ServiceForm extends Model
{
    /**
     * @var int $pageSize
     */
    public $pageSize = 15;

    /**
     * @var Entry $serviceEntry
     */
    private $serviceEntry = null;

    /**
     * This method returns a list of service objects and their data.
     *
     * @return ArrayDataProvider
     */
    public function getServices()
    {
        $query = Entry::find()
            ->section('services')
            ->orderBy('title')
            ->allowOnlineScheduling(1);

        $result = [];
        foreach ($query->each() as $service) {
            $result[] = [
                'id' => $service->id,
                'name' => $service->title,
                'resource' => $this->getServiceResource($service),
                'reasonForVisit' => $this->getReasonsForVisitPerService($service),
                'departments' => $this->getAllDepartmentsForService($service)
            ];
        }

        $dataProvider = new ArrayDataProvider([
            'allModels' => $result,
            'pagination' => [
                'pageSize' => $this->pageSize,
                'totalCount' => $query->count()
            ]
        ]);

        return $dataProvider;
    }

    /**
     * Returns an array of objects that contain the externalDepartmentId field
     * from suiteServices matrix blocks whose serviceType entries field matches
     * the provided service entry with hardcoded type 'External'
     *
     * @param Entry $service - specialty/service Entry object
     * @return array
     */
    private function getAllDepartmentsForService($service) 
    {
        $departments = [];

        $suiteServices = MatrixBlock::find()
            ->field('suiteServices')
            ->relatedTo([
                'field' => 'serviceType',
                'targetElement' => $service
            ])
            ->all();
        
        if (empty($suiteServices)) {
            return $departments;
        }

        foreach ($suiteServices as $suiteService) {
            if (empty($suiteService->externalDepartmentId)) {
                continue;
            }
            $departments[] = [
                'id' => $suiteService->externalDepartmentId,
                'type' => 'External'
            ];
        }

        return $departments;
    }

    /**
     * Maps visit reasons to apropriate objects.
     *
     * @param Entry $service - specialty/service Entry object
     * @return mixed - list of this service's visit reasons
     */
    private function getReasonsForVisitPerService(Entry &$service)
    {
        $result = [];
        foreach ($service->appointmentSchedulingReasonsForVisit->each() as $visitReason) {
            $result[] = [
                'id' => $visitReason->id,
                'name' => $visitReason->title,
                'description' => $visitReason->serviceReasonsForVisitExplanation
            ];
        }

        return $result;
    }

    /**
     * Returns the scheduling resource type based on given service.
     *
     * @param Entry $service - specialty/service Entry object
     * @return string - resource type
     */
    private function getServiceResource(Entry &$service)
    {
        if (!$service->schedulingWithoutPhysicians) {
            return 'physician';
        }

        $isTelemedicine = false;

        $telemedicineServiceCategories = Category::find()
            ->group('telemedicineServices')
            ->with('assignedService')
            ->all();
                
        foreach ($telemedicineServiceCategories as $category) {
            foreach ($category->assignedService as $assignedService) {
                if ($assignedService->id == $service->id) {
                    $isTelemedicine = true;
                    break 2;
                }
            }
        }

        return $isTelemedicine ? 'telemedicine' : 'location';
    }
}
