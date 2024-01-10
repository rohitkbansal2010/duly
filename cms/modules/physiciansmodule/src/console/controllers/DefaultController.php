<?php
namespace modules\physiciansmodule\console\controllers;

use Craft;

use craft\elements\Entry;
use craft\records\EntryType;
use modules\DupageCoreModule\DupageCoreModule;
use modules\physiciansmodule\PhysiciansModule;
use samdark\log\PsrMessage;
use yii\console\Controller;
use yii\console\ExitCode;
use yii\helpers\Console;
use yii\helpers\Inflector;

/**
 * Console commands to enable management of physicians
*/

class DefaultController extends Controller
{
    /**
     * Converts all physician types who have field_physicianCategory = 'midLevelProvider' to entry type "Mid-Level Provider"
     *
     *  @return void
     * */

    public function actionConvertPhysicianEntryTypes()
    {
        $midLevelEntryType = EntryType::find()
            ->where(['handle' => 'midLevelProvider'])
            ->one();

        $physicianEntryType = EntryType::find()
            ->where(['handle' => 'physicians'])
            ->one();

        $eligiblePhysicians = Entry::find()
            ->section('physicians')
            ->where(['typeId' => $physicianEntryType->id])
            ->andWhere(['field_physicianCategory' => 'midLevelProvider']);

        if ($eligiblePhysicians) {
            if ($midLevelEntryType) {
                $count = $eligiblePhysicians->count();
                Console::startProgress(0, $count, 'Converting Physicians');
                $i = 0;

                foreach ($eligiblePhysicians->batch(10) as $row) {
                    foreach ($row as $provider) {
                        $provider->typeId = $midLevelEntryType->id;
                        Craft::$app->elements->saveElement($provider);
                        Console::updateProgress((++$i), $count);
                    }
                }

                Console::endProgress(false, true);
                Console::output("{$count} physicians were converted to Mid-Level Providers");
                return ExitCode::OK;
            }
        }

        Console::output("No eligible physicians have been found.");
        return ExitCode::OK;
    }
}