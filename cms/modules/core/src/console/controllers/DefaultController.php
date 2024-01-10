<?php
namespace modules\DupageCoreModule\console\controllers;

use Craft;

use DateInterval;
use DateTime;
use League\Csv\Writer;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\queue\EmailJob;
use modules\DupageCoreModule\queue\UpdatePhysicianJob;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;
use yii\console\Controller;
use yii\console\ExitCode;
use yii\console\widgets\Table;
use yii\helpers\Console;
use yii\helpers\Inflector;

/**
 * Console commands to enable management of physician appointments
 */
class DefaultController extends Controller
{
    /**
     * Loads provided ElementExporter class, generates the results
     * and sends an email to an address from the general config "physiciansExportToEmail" variable.
     *
     * @param string $elementExporterClassName     class name and path to the ElementExporter exporter class
     * @return void
     */
    public function actionEmailCustomEntriesExportAsCsv(string $elementExporterClassName)
    {
        if (!\class_exists($elementExporterClassName)) {
            Craft::error(new PsrMessage("Invalid ElementExporter class. Check your parameter and try again.", [
                'elementExporterClassName' => $elementExporterClassName
            ]), get_class($this) . '::' . __METHOD__);
            die();
        }
        
        $toEmailAddress = Craft::$app->globals->getSetByHandle('generalSiteConfig')['physiciansExportEmailRecipient'] ?? 
            "anna.vriesema@dupagemd.com,5798e05d.dupagemd.onmicrosoft.com@amer.teams.ms";
        $toEmailAddress = array_map('trim', explode(',', $toEmailAddress));

        Craft::info(new PsrMessage("Generating results..."), get_class($this) . '::' . __METHOD__);
        $exporter = new $elementExporterClassName;
        $results = $exporter->export();

        $results = $this->generateCSVWithResults($results);
        $this->sendEmail($results, $toEmailAddress);
        
        Craft::info(new PsrMessage("Done!"), get_class($this) . '::' . __METHOD__);
    }

    private function generateCSVWithResults(array $results)
    {
        $csv = Writer::createFromString('');
        $csv->insertOne(\array_keys($results[0]));
        $csv->insertAll($results);
        return $csv->getContent();
    }

    private function sendEmail(string $results, array $toEmailAddress)
    {
        $timestamp = new \DateTime('now');
        $timestamp = $timestamp->format('D M d, Y G:i');
        $exportFileName = getenv('DEFAULT_SITE_NAME_SHORT') . " Export {$timestamp}.csv";

        if (count($toEmailAddress) > 0) {
            $sentEmails = implode(", ", $toEmailAddress);

            Craft::info(new PsrMessage("Scheduling an email to {$sentEmails} with the results..."), get_class($this) . '::' . __METHOD__);
            foreach($toEmailAddress as $email) {
                Craft::$app->queue->push(new EmailJob([
                    'template' => 'cms-entries-export.twig',
                    'subject' => Craft::t('dupage-core-module', "Weekly " . getenv('DEFAULT_SITE_NAME_SHORT') . " CMS Export"),
                    'to' => $email,
                    'attachmentFileContent' => $results,
                    'attachmentFileName' => $exportFileName,
                    'attachmentFileMimeType' => 'text/csv',
                ]));
            }
        }
    }
}
