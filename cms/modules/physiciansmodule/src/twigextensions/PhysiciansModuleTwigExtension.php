<?php
namespace modules\physiciansmodule\twigextensions;

use craft\elements\Entry;
use modules\physiciansmodule\PhysiciansModule;
use Twig\TwigFunction;

class PhysiciansModuleTwigExtension extends \Twig_Extension
{
    /**
     * @inheritdoc
     */
    public function getName()
    {
        return 'PhysiciansModule';
    }

    /**
     * @inheritdoc
     */
    public function getFilters()
    {
        return [];
    }

    /**
     * @inheritdoc
     */
    public function getFunctions()
    {
        return [
            new TwigFunction('getAppointmentSlotsForPhysician', [$this, 'getAppointmentSlotsForPhysician']),
            new TwigFunction('getPhysiciansForService', [$this, 'getPhysiciansForService']),
            new TwigFunction('getPhysiciansByProcedureForService', [$this, 'getPhysiciansByProcedureForService']),
            new TwigFunction('getSimilarPhysicianIds', [$this, 'getSimilarPhysicianIds']),
            new TwigFunction('shuffle', [$this, 'shuffle'])
        ];
    }

    public function getAppointmentSlotsForPhysician(Entry $physician, array $physicianLocations, array $dates, int $index = 0)
    {
        return PhysiciansModule::getInstance()->physiciansModuleService->getAppointmentSlotsForPhysician($physician, $physicianLocations, $dates, $index);
    }

    public function getPhysiciansForService(Entry $service)
    {
        return PhysiciansModule::getInstance()->physiciansModuleService->getPhysiciansForService($service);
    }

    public function getPhysiciansByProcedureForService(Entry $service)
    {
        return PhysiciansModule::getInstance()->physiciansModuleService->getPhysiciansByProcedureForService($service);
    }

    public function getSimilarPhysicianIds(Entry $physician, float $maxDistanceInMiles = PHP_FLOAT_MAX)
    {
        return PhysiciansModule::getInstance()->physiciansModuleService->getSimilarPhysicianIds($physician, $maxDistanceInMiles);
    }

	public function shuffle(array $elements)
    {
		shuffle($elements);
		return $elements;
    }
}
