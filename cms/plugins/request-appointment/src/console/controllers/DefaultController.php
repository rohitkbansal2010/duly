<?php declare(strict_types=1);
namespace punchkick\requestappointment\console\controllers;

use Craft;

use craft\elements\Entry;
use DateTime;
use League\Csv\Writer;
use samdark\log\PsrMessage;
use yii\db\Query;
use yii\console\Controller;
use punchkick\requestappointment\RequestAppointment;
use punchkick\requestappointment\records\RequestAppointmentRecord;
use yii\helpers\Console;
use gftp\FtpComponent;
use yii\console\ExitCode;

/**
 * Console commands to enable management of physician appointments
 */
class DefaultController extends Controller
{
    const HEADERS = [
        'type',
        'location',
        'name',
        'phone',
        'dob',
        'best_time',
        'date1',
        'window1',
        'date2',
        'window2',
        'date3',
        'window3',
        'insurance',
        'order',
        'work_comp',
        'communication',
        'symptoms',
        'notes',
        'timestamp'
    ];
     /**
     * Generates a CSV output of requested PT/OT appointments
     *  > requested-appointments.csv for output
     *  Note also that CDN urls are _not_ used, and should be changed before sending to the client.
     */
    public function actionGenerateRequestAppointmentData(string $startDate = '', string $endDate = '')
    {
        $rows = RequestAppointmentRecord::find();

        if ($startDate) {
            if ($this->validateDateFormat($startDate)) {
                $rows->andWhere(['>=','dateCreated',(new DateTime($startDate))->format('Y-m-d')]);
            } else {
                Console::error($startDate.' is not valid. Must be in Y-m-d format.');
                return ExitCode::USAGE;
            }
        }

        if ($endDate) {
            if ($this->validateDateFormat($endDate)) {
                $rows->andWhere(['<=','dateCreated',(new DateTime($endDate))->format('Y-m-d')]);
            } else {
                Console::error($endDate.' is not valid. Must be in Y-m-d format.');
                return ExitCode::USAGE;
            }  
        }         

        $rows = $rows->all();

        if (count($rows) >= 1) {
            $records = $this->getRecords($rows);
            $csv = Writer::createFromString('');
            $csv->insertOne(static::HEADERS);
            $csv->insertAll($records);
            echo $csv->getContent();
            return ExitCode::OK;
        }

        Console::output('There are currently no records that were processed.');
        return ExitCode::OK;
    }

    /**
     * Uploads CSV output to an SFTP server
     */
    public function actionSubmitRequestAppointmentData()
    {
        $time = (new DateTime)->format('Y-m-d h:i:s');

        $rows = RequestAppointmentRecord::find()
            ->where(['processed' => 0])
            ->andWhere(['<=', 'dateCreated', $time])
            ->all();

        if (count($rows) >= 1) {                
            $records = $this->getRecords($rows);
            $today = new DateTime('now');
            $now = $today->format('F j, Y, g:i a');

            // colons cause file transfer errors
            $now = str_replace(':', '.', $now);
            $directory = getenv('SFTP_DIRECTORY');
            $fileName = "/{$directory}/Request Appointment Summary For {$now}.csv";
            $mode = FTP_ASCII; 
            $asynchronous = false; 
            $csv = Writer::createFromString('');
            $csv->insertOne(static::HEADERS);
            $csv->insertAll($records);
            $temp = tmpfile();
            fwrite($temp, $csv->getContent());
            fseek($temp, 0);

            RequestAppointment::getInstance()->ftp->put($temp, $fileName, $mode, $asynchronous);
            fclose($temp); // releases the memory (or tempfile)
            $this->updateAffectedRecords($time);

            Console::output('File was successfully uploaded.');
            return ExitCode::OK;
        }

        Console::output('There are no new submissions at this time.');
        return ExitCode::OK;
    }

    /**
     * Updates records that were recently uploaded to SFTP server
     */
    private function updateAffectedRecords(string $time) 
    {
        RequestAppointmentRecord::updateAll(['processed' => 1], ['and', ['processed' => 0], ['<=', 'dateCreated', $time]]);
    }

    /**
     * Assigns proper values to record
     */
    private function getRecords(array $rows) {
        return \array_map(function ($record) {
            return [
                'type' => $record['type'],
                'location' => $record['location'],
                'name' => $record['first_name'].' '.$record['last_name'],
                'phone' => $record['phone_number'],
                'dob' => $record['dob'],
                'best_time' => $record['best_time'],
                'date1' => $record['date1'],
                'window1' => $record['window1'],
                'date2' => $record['date2'],
                'window2' => $record['window2'],
                'date3' => $record['date3'],
                'window3' => $record['window3'],
                'insurance' => $record['insurance'],
                'order' => $record['order'],
                'work_comp' => $record['work_comp'],
                'communication' => $record['communication'],
                'symptoms' => $record['symptoms'],
                'notes' => '',
                'timestamp' => $this->formatTimeStamp($record['dateCreated'])
            ];
        }, $rows);
    }

    private function formatTimeStamp(string $dateString) 
    {
        $dateTime = new DateTime($dateString); 
        return $dateTime->format("Y-m-d  H:ia"); 
    }

    private function validateDateFormat($date, $format = 'Y-m-d')
    {
        $d = DateTime::createFromFormat($format, $date);
        return $d && $d->format($format) === $date;
    }
}