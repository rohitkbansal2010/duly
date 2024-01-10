<?php

namespace modules\schedulingmodule\console\controllers;

use Craft;

use craft\elements\Entry;
use craft\fields\Entries;
use craft\elements\Category;
use craft\records\Field;
use craft\services\Relations;
use League\Csv\Writer;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\queue\UpdatePhysicianJob;
use samdark\log\PsrMessage;
use yii\console\Controller;
use yii\helpers\Console;
use yii\helpers\Inflector;

/**
 * Console commands to enable management of physician appointments
 */
class DefaultController extends Controller
{
    /**
     * Queries physician NPIs. In batches, it requests appointment times for the batch of NPIs to refresh the cache.
     * Queries appointments times for the next six weeks using three requests, two weeks at a time.
     *
     * @param array $npi            A list of NPIs to restrict the update to.
     *                              Providing a comma-separated list will only
     *                              perform an update for physicians with matching NPIs.
     *
     * @param array $serviceIds     A list of service IDs to restrict the update to.
     *                              Providing a comma-separated list will only
     *                              perform an update for physicians associates to those services.
     *
     * Example command (all "Family Medicine" providers):
     * ./craft scheduling-module/default/cache-physician-appointment-times '' '6702,7022,7139'
     *
     * @return void
     */
    public function actionCachePhysicianAppointmentTimes(array $npi = [], array $serviceIds = [])
    {
        $this->nullifyNextAvailableAppointmentDates($npi);

        $physicians = Entry::find()->section('physicians');
        if (count($npi) > 0) {
            $physicians->nationalProviderIdentifier($npi);
        }
        if (count($serviceIds) > 0) {
            $physicians->relatedTo([
                'and',
                [
                    'targetElement' => array_map(fn($id) => (int)$id, $serviceIds),
                ]
            ]);
        }

        $count = $physicians->count();
        Console::startProgress(0, $count, 'Queueing Physicians');
        $i = 0;
        foreach ($physicians->each() as $physician) {
            Craft::$app->queue->push(new UpdatePhysicianJob([
                'physicianId' => $physician->id
            ]));

            Console::updateProgress((++$i), $count);
        }

        Console::endProgress(false, true);
    }

    /**
     * Finds all location suites that offer a service scheduled with external resource IDs
     * and caches aappointment times for those external resource IDs, similar to actionCachePhysicianAppointmentTimes.
     *
     * @return void
     */
    public function actionCacheExternalResourceAppointmentTimes()
    {
        $externalProviderResourceIds = [];

        $suites = Entry::find()->section('locations')->type('suite');

        foreach ($suites->each() as $suite) {
            foreach ($suite->suiteServices->all() as $service) {
                // if this service does not offer scheduling with external resource, skip
                if ($service->externalProviderResourceId === null) {
                    continue;
                }
                $externalProviderResourceIds[] = $service->externalProviderResourceId;
            }
        }

        $count = count($externalProviderResourceIds);
        Console::startProgress(0, $count, 'Queueing External Resource IDs');
        $i = 0;
        foreach ($externalProviderResourceIds as $externalProviderResourceId) {
            Craft::$app->queue->push(new UpdatePhysicianJob([
                'externalResourceId' => $externalProviderResourceId
            ]));

            Console::updateProgress((++$i), $count);
        }
        Console::endProgress(false, true);
    }

    /**
     * Nullifies field_nationalProviderIdentifier for physicians if the set date.
     * This is done to always ensure the most accurate value after the UpdatePhysicianJob finishes.
     *
     * @param array $npi     A list of NPIs to restrict the update to
     * Providing a comma-separated list will only
     * perform an update for physicians with matching NPIs
     * @return void
     */
    private function nullifyNextAvailableAppointmentDates(array $npi = [])
    {
        $conditions = $npi === [] ? [] : [
            'field_nationalProviderIdentifier' => $npi
        ];

        $cmd = Craft::$app
            ->db
            ->createCommand()
            ->update(
                'content',
                [
                    'field_physicianNextAvailableAppointmentTime' => null
                ],
                $conditions
            );

        if (!$cmd->execute()) {
            Craft::error(
                new PsrMessage('Unable to nullify field_physicianNextAvailableAppointmentTime.', [
                    'npis' => $npi
                ])
            );
        }
    }

    /**
     * Generates a CSV output of all physicians, their NPIS, and non CDN urls
     *  > physicians.csv for output
     *  Note also that CDN urls are _not_ used, and should be changed before sending to the client.
     */
    public function actionGeneratePhysicianDataForMyChart()
    {
        $entries = Entry::find()
            ->section('physicians')
            ->with(['physicianHeadshot']);

        $records = \array_map(function ($record) {
            return [
                'PhysicianName' => $record->title,
                'NPI' => (string)$record->nationalProviderIdentifier,
                'EPI' => (string)$record->epicProviderId,
                'HeadShotURL' => isset($record->physicianHeadshot[0]) ? DupageCoreModule::getInstance()
                    ->dupageCoreModuleService
                    ->getOptimizedImage(
                        $record->physicianHeadshot[0],
                        'jpeg',
                        true,
                        [
                            [
                                'settings' => [
                                    'resize:fill::512:1:1'
                                ]
                            ]
                        ]
                    )[0] : null
            ];
        }, $entries->all());

        $header = [
            'PhysicianName',
            'NPI',
            'EPI',
            'HeadShotURL'
        ];

        $csv = Writer::createFromString('');
        $csv->insertOne($header);
        $csv->insertAll($records);

        echo $csv->getContent();
    }

    /**
     * Assigns services to a category
     */
    public function actionAssignServicesToCategory(string $category_name, array $entryTitles = [])
    {
        $siteId = Craft::$app->sites->getPrimarySite()->id;
        $group = Craft::$app->categories->getGroupByHandle($category_name);

        // Assigned Service field
        $assignedServiceEntryField = Field::find()
            ->where(['handle' => 'assignedService'])
            ->one();

        if ($group != null) {
            foreach ($entryTitles as $title) {
                $entry = Entry::find()
                    ->section('services')
                    ->title($title)
                    ->one();

                if ($entry != null) {
                    $model = new Category;
                    $model->slug = Inflector::slug($title);
                    $model->title = $title;
                    $model->enabled = true;
                    $model->siteId = $siteId;
                    $model->groupId = $group->id;

                    Craft::$app->getElements()->saveElement($model);

                    // Fetches the field type for the relation record between the entry and category
                    $modelServiceField = new Entries();
                    $modelServiceField->id = $assignedServiceEntryField->id;

                    // Creates the relation
                    $relation = new Relations();
                    $relation->init();
                    $relation->saveRelations($modelServiceField, $model, [$entry->id]);
                } else {
                    Console::error($title . ' was not found.');
                }
            }
        } else {
            Console::error($category_name . ' was not found as a category.');
        }
    }
}
