<?php

namespace tests\unit\SchedulingModuleEpicService\GetAppointmentSlotsTest;

use modules\schedulingmodule\services\SchedulingModuleEpicService;
use Codeception\Util\JsonType;
use DateTime;
use DateInterval;
use Exception;
use Craft;

class ScheduleAppointmentTest extends \Codeception\Test\Unit
{
    /**
     * @var \UnitTester
     */
    protected $tester;
    
    protected function _before()
    {
        $this->schedulingModuleEpicService = new SchedulingModuleEpicService();

        // find valid appointment times
        $appointmentTimes = $this->schedulingModuleEpicService->getPhysicianAppointmentTimes(['1215986997']);

        $appointmentTime =  $appointmentTimes['1215986997'][0];

        $this->validAppointmentInfo = [
            'PatientID' => '3258539',
            'PatientIDType' => 'EPI',
            'DepartmentID' => $appointmentTime['Department']['ID'],
            'DepartmentIDType' => $appointmentTime['Department']['Type'],
            'VisitTypeID' => $appointmentTime['VisitType']['ID'],
            'VisitTypeIDType' => $appointmentTime['VisitType']['Type'],
            'Date' => $appointmentTime['Time'],
            'ProviderID' => $appointmentTime['Provider']['ID'],
            'ProviderIDType' => $appointmentTime['Provider']['Type'],
            'InsuranceName' => 'UMR',
            'MemberNumber' => '99887766',
            'GroupNumber' => '11-223344',
            'SubscriberName' => 'Wipfli Digital',
            'SubscriberDateOfBirth' => DateTime::createFromFormat('Y-m-d', '2012-01-18'),
            'Comments' => ["Comment #1", "Comment #2"]
        ];
        
        $this->invalidAppointmentInfo = [
            'PatientID' => '',
            'PatientIDType' => '',
            'DepartmentID' => $appointmentTime['Department']['ID'],
            'DepartmentIDType' => $appointmentTime['Department']['Type'],
            'VisitTypeID' => $appointmentTime['VisitType']['ID'],
            'VisitTypeIDType' => '',
            'Date' => $appointmentTime['Time'],
            'ProviderID' => $appointmentTime['Provider']['ID'],
            'ProviderIDType' => $appointmentTime['Provider']['Type'],
            'InsuranceName' => 'UMR',
            'MemberNumber' => '',
            'GroupNumber' => '',
            'SubscriberName' => 'Wipfli Digital',
            'SubscriberDateOfBirth' => DateTime::createFromFormat('Y-m-d', '2012-01-18')
        ];
    }

    protected function _after()
    {
    }

    public function testValidAppointment()
    {
        $scheduledAppointment = $this->schedulingModuleEpicService->scheduleAppointment(
            $this->validAppointmentInfo['PatientID'],
            $this->validAppointmentInfo['PatientIDType'],
            $this->validAppointmentInfo['DepartmentID'],
            $this->validAppointmentInfo['DepartmentIDType'],
            $this->validAppointmentInfo['VisitTypeID'],
            $this->validAppointmentInfo['VisitTypeIDType'],
            $this->validAppointmentInfo['Date'],
            $this->validAppointmentInfo['ProviderID'],
            $this->validAppointmentInfo['ProviderIDType'],
            $this->validAppointmentInfo['InsuranceName'],
            $this->validAppointmentInfo['MemberNumber'],
            $this->validAppointmentInfo['GroupNumber'],
            ...$this->validAppointmentInfo['Comments'],
        );

        $this->assertIsArray($scheduledAppointment);

        $this->assertArrayHasKey('Time', $scheduledAppointment);
        $this->assertArrayHasKey('DurationInMinutes', $scheduledAppointment);
        $this->assertArrayHasKey('Date', $scheduledAppointment);
        $this->assertArrayHasKey('PatientInstructions', $scheduledAppointment);
        $this->assertArrayHasKey('Warnings', $scheduledAppointment);
        $this->assertArrayHasKey('Provider', $scheduledAppointment);
        $this->assertArrayHasKey('Department', $scheduledAppointment);
        $this->assertArrayHasKey('VisitType', $scheduledAppointment);
        $this->assertArrayHasKey('Patient', $scheduledAppointment);
        $this->assertArrayHasKey('ContactIDs', $scheduledAppointment);
    }

    public function testInvalidPatient()
    {
        $patient = $this->schedulingModuleEpicService->scheduleAppointment(
            $this->invalidAppointmentInfo['PatientID'],
            $this->invalidAppointmentInfo['PatientIDType'],
            $this->invalidAppointmentInfo['DepartmentID'],
            $this->invalidAppointmentInfo['DepartmentIDType'],
            $this->invalidAppointmentInfo['VisitTypeID'],
            $this->invalidAppointmentInfo['VisitTypeIDType'],
            $this->invalidAppointmentInfo['Date'],
            $this->invalidAppointmentInfo['ProviderID'],
            $this->invalidAppointmentInfo['ProviderIDType'],
            $this->invalidAppointmentInfo['InsuranceName'],
            $this->invalidAppointmentInfo['MemberNumber'],
            $this->invalidAppointmentInfo['GroupNumber'],
        );

        $this->assertNull($patient);
    }

    public function testRepeatTheSameAppointmentAttempt()
    {
        $scheduledAppointment = $this->schedulingModuleEpicService->scheduleAppointment(
            $this->validAppointmentInfo['PatientID'],
            $this->validAppointmentInfo['PatientIDType'],
            $this->validAppointmentInfo['DepartmentID'],
            $this->validAppointmentInfo['DepartmentIDType'],
            $this->validAppointmentInfo['VisitTypeID'],
            $this->validAppointmentInfo['VisitTypeIDType'],
            $this->validAppointmentInfo['Date'],
            $this->validAppointmentInfo['ProviderID'],
            $this->validAppointmentInfo['ProviderIDType'],
            $this->validAppointmentInfo['InsuranceName'],
            $this->validAppointmentInfo['MemberNumber'],
            $this->validAppointmentInfo['GroupNumber'],
        );

        $this->assertIsArray($scheduledAppointment);

        $this->assertArrayHasKey('Time', $scheduledAppointment);
        $this->assertArrayHasKey('DurationInMinutes', $scheduledAppointment);
        $this->assertArrayHasKey('Date', $scheduledAppointment);
        $this->assertArrayHasKey('PatientInstructions', $scheduledAppointment);
        $this->assertArrayHasKey('Warnings', $scheduledAppointment);
        $this->assertArrayHasKey('Provider', $scheduledAppointment);
        $this->assertArrayHasKey('Department', $scheduledAppointment);
        $this->assertArrayHasKey('VisitType', $scheduledAppointment);
        $this->assertArrayHasKey('Patient', $scheduledAppointment);
        $this->assertArrayHasKey('ContactIDs', $scheduledAppointment);

        $patient = $this->schedulingModuleEpicService->scheduleAppointment(
            $this->validAppointmentInfo['PatientID'],
            $this->validAppointmentInfo['PatientIDType'],
            $this->validAppointmentInfo['DepartmentID'],
            $this->validAppointmentInfo['DepartmentIDType'],
            $this->validAppointmentInfo['VisitTypeID'],
            $this->validAppointmentInfo['VisitTypeIDType'],
            $this->validAppointmentInfo['Date'],
            $this->validAppointmentInfo['ProviderID'],
            $this->validAppointmentInfo['ProviderIDType'],
            $this->validAppointmentInfo['InsuranceName'],
            $this->validAppointmentInfo['MemberNumber'],
            $this->validAppointmentInfo['GroupNumber']
        );

        $this->assertNull($patient);
    }
}
