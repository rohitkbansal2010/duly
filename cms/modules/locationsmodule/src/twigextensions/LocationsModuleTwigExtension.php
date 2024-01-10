<?php
namespace modules\locationsmodule\twigextensions;

use modules\locationsmodule\LocationsModule;
use DateTime;
use Twig_SimpleFilter;
use Twig_SimpleFunction;
use Craft;
use modules\physiciansmodule\PhysiciansModule;

class LocationsModuleTwigExtension extends \Twig_Extension
{
    /**
     * @inheritdoc
     */
    public function getFunctions()
    {
        return [
            new Twig_SimpleFunction('determineClosedDateWithinNextWeek', [$this, 'determineClosedDateWithinNextWeek']),
            new Twig_SimpleFunction('getServiceForClosestLocation', [$this, 'getServiceForClosestLocation']),
            new Twig_SimpleFunction('getNextAvailableAppointment', [$this, 'getNextAvailableAppointment'])
        ];
    }

    public function determineClosedDateWithinNextWeek(array $closedDates, string $dayOfWeek)
    {
        return LocationsModule::getInstance()->locationsModuleService->determineClosedDateWithinNextWeek($closedDates, $dayOfWeek);
    }

    /**
     * Returns a service entry that matches with immediate care / express care location
     *
     * @param array $locationServices
     * @param string $handle
     * @return mixed
     */
    public function getServiceForClosestLocation(array $locationServices, string $handle)
    {
       return LocationsModule::getInstance()->locationsModuleService->getServiceForClosestLocation($locationServices, $handle);
    }

    /**
     * Returns the next available appointment for a service at a suite
     *
     * @param Entry $office
     * @param string $chosenVisitTypeCode
     * @return mixed
     */
    public function getNextAvailableAppointment($office, $chosenVisitTypeCode)
    {
        $user = Craft::$app->patient_user->identity;

        $user->location_id = $office->id;
        $externalProviderResourceId = $user->findExternalProviderResourceIdForChosenServiceEntry();
        $externalProviderResourceId = strpos($externalProviderResourceId, ',') > 0 ? array_map('trim', explode(',', $externalProviderResourceId)) : [$externalProviderResourceId];
        
        $nextAvailableDatesFound = [];

        $now = new DateTime();

        foreach ($externalProviderResourceId as $id) {
            $nextAvailableDate = PhysiciansModule::getInstance()
                ->physiciansModuleService
                ->getNextAppointmentForExternalResourceLocationAndVisitTypeIDFromDate(
                    $id,
                    $chosenVisitTypeCode,
                    $now
                );
            if ($nextAvailableDate) {
                \array_push($nextAvailableDatesFound, $nextAvailableDate);
            }
        }

        $nextAvailableDatesFound = array_filter($nextAvailableDatesFound, function($appt) use($now) {
            return $appt['Time'] > $now;
        });

        usort($nextAvailableDatesFound, function ($a, $b) {
            return $a['Time'] > $b['Time'];
        });

        if(!count($nextAvailableDatesFound)) {
            return null;
        }

        return $nextAvailableDatesFound[0]['Time']->format('D, F j \a\t g:i A');
    }

}
