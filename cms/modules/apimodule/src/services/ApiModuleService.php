<?php
/**
 * API Module for Craft 3 CMS
 *
 * Allows for exposing API endpoints
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2021 Wipfli Digital
 */

namespace modules\apimodule\services;

use craft\base\Component;
use modules\locationsmodule\LocationsModule;
use modules\schedulingmodule\SchedulingModule;

use yii\db\Expression;
use yii\db\Query;

/**
 * @author    Wipfli Digital
 * @package   ApiModuleServqwehgtfrcxsaice
 * @since     1.0.0
 */
class ApiModuleService extends Component
{
    /**
     * This method returns a visit type code for given service and patient type (new vs established)
     */
    public function getVisitTypeCodeForService(int $serviceId, bool $isNewPatient)
    {
        $visitTypeCode = null;

        $globalVisitTypeCodes = SchedulingModule::getInstance()
            ->schedulingModuleService
            ->getGlobalSchedulingVisitTypeCodes();

        if ($isNewPatient) {
            $visitTypeCode = $globalVisitTypeCodes['newPatient']['default'];
            foreach ($globalVisitTypeCodes['newPatient']['byService'] as $globalVisitTypeCode) {
                if (isset($globalVisitTypeCode['serviceIds']) && \in_array($serviceId, $globalVisitTypeCode['serviceIds'])) {
                    $visitTypeCode = $globalVisitTypeCode['visitTypeId'];
                    break;
                }
            }
        } else {
            $visitTypeCode = $globalVisitTypeCodes['establishedPatient']['default'];
            foreach ($globalVisitTypeCodes['establishedPatient']['byService'] as $globalVisitTypeCode) {
                if (isset($globalVisitTypeCode['serviceIds']) && \in_array($serviceId, $globalVisitTypeCode['serviceIds'])) {
                    $visitTypeCode = $globalVisitTypeCode['visitTypeId'];
                    break;
                }
            }
        }

        return $visitTypeCode;
    }

    /**
     * Applies a geographical sort by zip code of given model query.
     *
     * @param query - hospitals query
     */
    public function sortByZipCode(Query &$query, string $zipCode)
    {
        $coordinates = LocationsModule::getInstance()
            ->locationsModuleService
            ->getLatLngForAddress($zipCode);
        $this->sortByCoordinates($query, (float) $coordinates['lat'], (float) $coordinates['lng']);
    }

    /**
     * Applies a geographical sort by lat and lng of given model query.
     *
     * @param query - hospitals query
     * @param lat - latitude
     * @param lng - longitude query
     */
    public function sortByCoordinates(Query &$query, float $lat, float $lng)
    {
        $range = new Expression(
            LocationsModule::getInstance()
                ->locationsModuleService
                ->sqlDistanceBetweenTwoGivenPoints(
                    $lng,
                    $lat,
                    "JSON_EXTRACT(field_address, '$.lng')",
                    "JSON_EXTRACT(field_address, '$.lat')"
                )
        );

        $query->orderBy($range);
    }
}
