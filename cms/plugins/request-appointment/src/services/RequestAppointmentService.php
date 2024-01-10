<?php
/**
 * Request Appointment for Craft CMS 3.x
 *
 * Allows for extended management of submitting request form for appointment.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace punchkick\requestappointment\services;

use Craft;
use craft\base\Component;
use craft\elements\Category;
use craft\elements\db\EntryQuery;
use craft\elements\Entry;
use DateTime;
use yii\db\ArrayExpression;
use yii\db\Expression;
use yii\db\Query;
use punchkick\requestappointment\RequestAppointment;

/**
 * @author    Wipfli Digital
 * @package   RequestAppointmentPlugin
 * @since     1.0.0
 */
class RequestAppointmentService extends Component
{
        /**
     * Determines locations associated with physical and occupational therapy
     *
     * @param string $handle the handle of category to be searched
     * @return array list of location entries associated with category
     */
    public function getServiceLocationsForRequestForm(string $handle): array
    {
        $locations = [];
        $serviceCategoriesIds = [];

        $serviceCategories = Category::find()
            ->group($handle)
            ->all();

        // look up all ids for services
        foreach ($serviceCategories as $category) {
            if (isset($category->assignedService) && ($category->assignedService->one() != null)) {
                \array_push($serviceCategoriesIds, $category->assignedService->one()->id);
            }
        }

        if (count($serviceCategoriesIds) == 1) {
            $relatedServices = Entry::find()
                ->section('services')
                ->descendantOf($serviceCategoriesIds[0])
                ->ids();

            if ($relatedServices) {
                $serviceCategoriesIds = \array_merge($serviceCategoriesIds, $relatedServices);
            }

            $servicesQuery = Entry::find()
                ->section('services')
                ->id($serviceCategoriesIds);

            $locations = Entry::find()
                ->section('locations')
                ->type('suite')
                ->relatedTo([
                    'field' => 'suiteServices',
                    'targetElement' => $servicesQuery
                ])
                ->unique()
                ->orderBy(new Expression('JSON_EXTRACT(field_address, "$.parts.city")'))
                ->all();
        }

        return $locations;
    }

    /**
    * Returns true/false if entry is considered physical and occupational therapy
    *
    * @param string $handle
    * @param string $entryId
    * @return bool
    */
    public function showPhysicalAndOccupationalAppointmentForm(string $handle, string $entryId)
    {
        $formEnabled = Craft::$app->globals->getSetByHandle('generalSiteConfig')['enablePtOtForm'];

        if (!$formEnabled) {
            return false;
        }

        $serviceCategoriesIds = [];

        $serviceCategories = Category::find()
            ->group($handle)
            ->all();

        // look up all ids for services
        foreach ($serviceCategories as $category) {
            if (isset($category->assignedService) && ($category->assignedService->one() != null)) {
                \array_push($serviceCategoriesIds, $category->assignedService->one()->id);
            }
        }

        if (count($serviceCategoriesIds) == 1) {
            $relatedServices = Entry::find()
                ->section('services')
                ->descendantOf($serviceCategoriesIds[0])
                ->ids();

            if ($relatedServices) {
                $serviceCategoriesIds = \array_merge($serviceCategoriesIds, $relatedServices);
            }
        }

        if (\in_array($entryId, $serviceCategoriesIds)) {
            return true;
        }

        return false;
    }
}
