<?php

namespace modules\apimodule\forms;

use Craft;
use yii\base\Model;
use modules\schedulingmodule\SchedulingModule;
use DateTime;
use DateInterval;
use Exception;

class AppointmentForm extends Model
{
    /**
     * @var string $startDate
     */
    public $startDate;

    /**
     * @var string $endDate
     */
    public $endDate;

    /**
     * @var intager $providerId
     */
    public $providerId;

    /**
     * @var string $providerIdType
     */
    public $providerIdType;

    /**
     * @var string $visitTypeId
     */
    public $visitTypeId;

    /**
     * @var string $visitTypeIdType
     */
    public $visitTypeIdType;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            ['endDate', 'date', 'format' => 'php:Y-m-d'],
            ['startDate', 'date', 'format' => 'php:Y-m-d'],
            [['startDate', 'endDate', 'providerId', 'providerIdType'], 'required'],
            [['startDate', 'endDate', 'providerId', 'providerIdType', 'visitTypeId', 'visitTypeIdType'], 'safe']
        ];
    }

    /**
     * Look for providers Scheduling within Epic Service
     *
     * @return array
     */
    public function getAppointments()
    {
        $epicVisitTypeIDs = [];

        $storedData = [
            'startDate' => DateTime::createFromFormat("Y-m-d", $this->startDate),
            'endDate' =>  DateTime::createFromFormat("Y-m-d", $this->endDate),
            'providerId' => $this->providerId,
            'providerIdType' => $this->providerIdType,
            'visitTypeId' => $this->visitTypeId,
            'visitTypeIdType' => $this->visitTypeIdType,
        ];

        $endDate = $this->validateEndDate($storedData['startDate'], $storedData['endDate']);
        $providers = [
            $storedData['providerId'] => ucfirst($storedData['providerIdType'])
        ];

        if ($storedData['visitTypeId'] !== null && $storedData['visitTypeIdType'] !== null) {
            $epicVisitTypeIDs = [
                [
                    "ID" => $storedData['visitTypeId'],
                    "Type" => ucfirst($storedData['visitTypeIdType'])
                ]
            ];
        }

        $data = SchedulingModule::getInstance()
        ->schedulingModuleEpicService
        ->getPhysicianAppointmentTimes(
            $storedData['startDate'],
            $endDate,
            $providers,
            $epicVisitTypeIDs,
        );

        return $data;
    }

    /**
     * Check that endDate is within the 30 day limit
     *
     * @param DateTime $startDate
     * @param DateTime $endDate
     *
     * @throws Exception "Invalid end date" if $endDate is more than 30 days
     *
     * @return DateTime $endDate
     */
    private function validateEndDate(DateTime $startDate = null, DateTime $endDate = null)
    {
        if (!$endDate) {
            $endDate = clone $startDate;
            $endDate->add(new DateInterval("P7D"));
        }
        $endDate->setTime(23, 59, 0, 0);

        // end date shouldn't be less than startDate
        if ($endDate < $startDate) {
            Craft::$app->response->statusCode = 400;
            throw new Exception("End date is in the past");
        }

        // end date must be 30 days or less since start date
        if ($startDate->diff($endDate)->days > 30) {
            Craft::$app->response->statusCode = 400;
            throw new Exception("End date should be within 30's of start Date");
        }

        return  $endDate;
    }

    /**
     * @return string users IP address
     */
    private function getUserForLimit()
    {
        return Craft::$app->request->userIP;
    }
}
