<?php

namespace tests\unit\SchedulingModuleEpicService\GetAppointmentSlotsTest;

use Codeception\Util\JsonType;
use Craft;
use DateInterval;
use DateTime;
use Exception;
use modules\schedulingmodule\services\SchedulingModuleEpicService;

class PatientLookupTest extends \Codeception\Test\Unit
{
    /**
     * @var \UnitTester
     */
    protected $tester;

    public function testFindPatientByPhone()
    {
        $patient = (new SchedulingModuleEpicService)
            ->findPatient(
                DateTime::createFromFormat('Y-m-d', '2012-01-14'),
                'M',
                'Charles',
                'Portwood',
                'test@example.com',
                '31460440679'
            );
        
        var_dump($patient);
    }
}
