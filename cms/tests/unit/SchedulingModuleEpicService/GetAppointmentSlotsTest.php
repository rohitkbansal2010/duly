<?php

namespace tests\unit\SchedulingModuleEpicService\GetAppointmentSlotsTest;

use modules\schedulingmodule\services\SchedulingModuleEpicService;
use modules\schedulingmodule\services\Exceptions\InvalidStartDateException;
use modules\schedulingmodule\services\Exceptions\InvalidEndDateException;
use Codeception\Util\JsonType;
use DateTime;
use DateInterval;
use Exception;
use Craft;

class GetAppointmentSlotsTest extends \Codeception\Test\Unit
{
    /**
     * @var \UnitTester
     */
    protected $tester;
    
    protected function _before()
    {                
        $this->validNPIs = [
            1326025347,
            1942277942,
            1861475121,
            1992918510,
            1184693327
        ];
       
        $this->invalidNPIs = [
            1023067311,
            1174812267,
        ];

        $this->schedulingModuleEpicService = new SchedulingModuleEpicService();
    }

    protected function _after()
    {
    }

    public function testNoPhysiciansProvided()
    {
        $appointmentTimes = $this->schedulingModuleEpicService->getPhysicianAppointmentTimes();

        $this->assertIsArray($appointmentTimes);
        $this->assertEquals(0, count($appointmentTimes));
    }

    public function testBadStartDate()
    {
        $startDate = new DateTime();
        $startDate->sub(new DateInterval("P2D"));

        $this->expectException(InvalidStartDateException::class);
        $this->schedulingModuleEpicService->getPhysicianAppointmentTimes($this->validNPIs, $startDate);
    }

    public function testGoodStartDate()
    {
        $startDate = new DateTime();

        $appointmentTimes = $this->schedulingModuleEpicService->getPhysicianAppointmentTimes($this->validNPIs, $startDate);

        $this->assertIsArray($appointmentTimes);

        foreach ($this->validNPIs as $NPI) {
            $appointmentSlots = $appointmentTimes[$NPI];
            foreach ($appointmentSlots as $slot) {
                $this->assertArrayHasKey('Provider', $slot);
                $this->assertArrayHasKey('ID', $slot['Provider']);
                $this->assertArrayHasKey('Type', $slot['Provider']);
                $this->assertArrayHasKey('Department', $slot);
                $this->assertArrayHasKey('ID', $slot['Department']);
                $this->assertArrayHasKey('Type', $slot['Department']);
                $this->assertArrayHasKey('VisitType', $slot);
                $this->assertArrayHasKey('ID', $slot['VisitType']);
                $this->assertArrayHasKey('Type', $slot['VisitType']);
                $this->assertArrayHasKey('Time', $slot);
            }
        }
    }

    public function testBadEndDate()
    {
        $startDate = new DateTime();

        $endDate = clone $startDate;
        $endDate->add(new DateInterval("P31D"));

        $this->expectException(InvalidEndDateException::class);
        $this->schedulingModuleEpicService->getPhysicianAppointmentTimes($this->validNPIs, $startDate, $endDate);
    }

    public function testGoodEndDate()
    {
        $startDate = new DateTime();

        $endDate = new DateTime();
        $endDate->add(new DateInterval("P10D"));

        $appointmentTimes = $this->schedulingModuleEpicService->getPhysicianAppointmentTimes($this->validNPIs, $startDate, $endDate, false);

        $this->assertIsArray($appointmentTimes);

        foreach ($this->validNPIs as $NPI) {
            $appointmentSlots = $appointmentTimes[$NPI];
            foreach ($appointmentSlots as $slot) {
                $this->assertArrayHasKey('Provider', $slot);
                $this->assertArrayHasKey('ID', $slot['Provider']);
                $this->assertArrayHasKey('Type', $slot['Provider']);
                $this->assertArrayHasKey('Department', $slot);
                $this->assertArrayHasKey('ID', $slot['Department']);
                $this->assertArrayHasKey('Type', $slot['Department']);
                $this->assertArrayHasKey('VisitType', $slot);
                $this->assertArrayHasKey('ID', $slot['VisitType']);
                $this->assertArrayHasKey('Type', $slot['VisitType']);
                $this->assertArrayHasKey('Time', $slot);
            }
        }
    }
}
