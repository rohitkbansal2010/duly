<?php

namespace tests\unit\SchedulingModuleEpicService\GetAppointmentSlotsTest;

use modules\schedulingmodule\services\SchedulingModuleEpicService;
use Codeception\Util\JsonType;
use DateTime;
use DateInterval;
use Exception;
use Craft;

class PatientCreateTest extends \Codeception\Test\Unit
{
    /**
     * @var \UnitTester
     */
    protected $tester;
    
    protected function _before()
    {
        $randomDOB =  rand(1900, 2000) . '-' . \str_pad(\rand(1, 12), 2, '0', STR_PAD_LEFT) . '-' . \str_pad(\rand(1, 28), 2, '0', STR_PAD_LEFT);
        $randomGender = ['Male', 'Female'][rand(0, 1)];

        $this->validPatientInfo = [
            'DOB' => DateTime::createFromFormat('Y-m-d', $randomDOB),
            'Gender' => $randomGender,
            'FirstName' => 'Wipfli',
            'LastName' => 'Digital',
            'Street' => '121 N LaSalle St',
            'StreetLine2' => '',
            'State' => 'IL',
            'PostalCode' => '60602',
            'City' => 'Chicago',
            'Email' => 'test@test.com',
            'DepartmentID' => '25069',
            'DepartmentIDType' => 'External'
        ];
        
        $this->invalidPatientInfo = [
            'DOB' => DateTime::createFromFormat('Y-m-d', $randomDOB),
            'Gender' => $randomGender,
            'FirstName' => 'Wrong',
            'LastName' => 'Name',
            'Street' => '121 N LaSalle St',
            'StreetLine2' => '',
            'State' => '',
            'PostalCode' => '',
            'City' => '',
            'Email' => '',
            'DepartmentID' => '',
            'DepartmentIDType' => 'External'
        ];

        $this->schedulingModuleEpicService = new SchedulingModuleEpicService();
    }

    protected function _after()
    {
    }

    public function testValidPatient()
    {
        $createdPatient = $this->schedulingModuleEpicService->createPatient(
            $this->validPatientInfo['DOB'],
            $this->validPatientInfo['Gender'],
            $this->validPatientInfo['FirstName'],
            $this->validPatientInfo['LastName'],
            $this->validPatientInfo['DepartmentID'],
            $this->validPatientInfo['DepartmentIDType'],
            $this->validPatientInfo['Street'],
            $this->validPatientInfo['StreetLine2'],
            $this->validPatientInfo['State'],
            $this->validPatientInfo['PostalCode'],
            $this->validPatientInfo['City'],
            $this->validPatientInfo['Email']
        );

        $this->assertIsArray($createdPatient);

        $this->assertArrayHasKey('ID', $createdPatient);
        $this->assertArrayHasKey('Type', $createdPatient);

        // validate that the patient can be found
        $foundPatient = $this->schedulingModuleEpicService->findPatient(
            $this->validPatientInfo['DOB'],
            $this->validPatientInfo['Gender'],
            $this->validPatientInfo['FirstName'],
            $this->validPatientInfo['LastName'],
            $this->validPatientInfo['Street'],
            $this->validPatientInfo['StreetLine2'],
            $this->validPatientInfo['State'],
            $this->validPatientInfo['PostalCode'],
            $this->validPatientInfo['City'],
            $this->validPatientInfo['Email']
        );

        $this->assertIsArray($foundPatient);

        $this->assertArrayHasKey('ID', $foundPatient);
        $this->assertArrayHasKey('Type', $foundPatient);

        $this->assertEquals($createdPatient['ID'], $foundPatient['ID']);
        $this->assertEquals($createdPatient['Type'], $foundPatient['Type']);
    }

    public function testInvalidPatient()
    {
        $patient = $this->schedulingModuleEpicService->createPatient(
            $this->invalidPatientInfo['DOB'],
            $this->invalidPatientInfo['Gender'],
            $this->invalidPatientInfo['FirstName'],
            $this->invalidPatientInfo['LastName'],
            $this->invalidPatientInfo['DepartmentID'],
            $this->invalidPatientInfo['DepartmentIDType'],
            $this->invalidPatientInfo['Street'],
            $this->invalidPatientInfo['StreetLine2'],
            $this->invalidPatientInfo['State'],
            $this->invalidPatientInfo['PostalCode'],
            $this->invalidPatientInfo['City'],
            $this->invalidPatientInfo['Email']
        );

        $this->assertNull($patient);
    }
}
