<?php

namespace modules\DupageCoreModule\queue;

use Craft;
use craft\elements\Entry;
use craft\queue\BaseJob;
use DateInterval;
use DateTime;
use modules\DupageCoreModule\DupageCoreModule;
use modules\schedulingmodule\forms\GetAppointmentTimesForm;
use modules\schedulingmodule\SchedulingModule;
use samdark\log\PsrMessage;

/**
 * punchkick/dupage-core-module module for Craft CMS 3.x
 *
 * Updates physician data asyncronously with Epic
 *  - Next Available Appointment Time
 *  - PG/Binary Foundation Ratings
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2020 Punchkick Interactive
 *
 * Usage:
 *
 * Craft::$app->queue->push(new \modules\DupageCoreModule\queue\UpdatePhysicianJob([
 *    'physicianId' => $physician->id
 * ]));
 */
class UpdatePhysicianJob extends BaseJob
{
    /**
     * The physicianID to update
     *
     * @var int $physicianId
     */
    public $physicianId;

    /**
     * The externalResourceId to update
     *
     * @var int $externalResourceId
     */
    public $externalResourceId;

    /**
     * @inheritdoc
     *
     * @param Queue $queue
     */
    public function execute($queue)
    {
        if ($this->externalResourceId !== null) {
            $this->cacheExternalResourceAppointmentTimes($this->externalResourceId);
        } else {
            $physician = Entry::find()
                ->section('physicians')
                ->id($this->physicianId)
                ->one();

            if ($physician !== null) {
                $this->updatePhysician($physician);
            }
        }
    }

    /**
     * Updates a single physician entry
     *
     * @param Entry $physician
     */
    private function updatePhysician(Entry $physician)
    {
        // request and cache physicians Press Ganey ratings and comments
        if ($physician->nationalProviderIdentifier !== null && $physician->nationalProviderIdentifier >= 0) {
            $duration = 48 * 3600; // 48 hours
            $npi = $physician->nationalProviderIdentifier;
            Craft::$app->cache->getOrSet(
                "physician_{$npi}_ratings_and_comments",
                function () use ($npi, $physician) {
                    $ratingsAndComments = DupageCoreModule::getInstance()
                        ->dupageCoreModuleService
                        ->getPressGaneyRatingsAndComments($npi);

                    // trigger clearing of template caches where this physician info is used (DMG-1953)
                    /**
                     * 2/16/2021 update:
                     *
                     * Previous implementation of DMG-1953 added a saveElement() function triggered after a successful retrieval of PressGaney ratings and comments. The intention was to trigger invalidating of any template caches using this physician's info (such as a service details page).
                     *
                     * Unfortunately, this causes an issue: when multiple queue jobs attempt to update the same physician element, acquiring of the mutex fails with a fatal "unable to acquire a lock for the structure" error.
                     *
                     * This causes some UpdatePhysicianJob jobs to fail.
                     *
                     *  Solution: invalidating caches for the given element must not be done via saving the element. Changing the "saveElement" fn to a more targeted "invalidateCachesForElement" fn solves the issue, as the "invalidateCachesForElement" fn does not lock the structure.
                     */
                    Craft::$app->elements->invalidateCachesForElement($physician);
                    Craft::info(new PsrMessage('Re-saved physician entry.', [
                        'id' => $physician->id,
                        'npi' => $npi,
                    ]), get_class($this) . '::' . __METHOD__);

                    return $ratingsAndComments;
                },
                $duration
            );
        }

        $providerInfo = SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getProviderIdAndIdType($physician);
        $providerID = $providerInfo['providerID'];
        $providerIDType = $providerInfo['providerIDType'];

        // If there isn't a provider ID defined, give up.
        if (empty($providerID)) {
            Craft::info(new PsrMessage('Provider does not have an ID. Refusing requests.', \array_merge($providerInfo, [
                'providerId' => $physician->id
            ])), get_class($this) . '::' . __METHOD__);
            return;
        }

        if (!$physician->allowsOnlineScheduling) {
            Craft::info(new PsrMessage('Provider does not allow for online scheduling. Skipping request.', \array_merge($providerInfo, [
                'providerId' => $physician->id
            ])), get_class($this) . '::' . __METHOD__);

            return;
        }

        $startDate = new DateTime();
        $endDate = new DateTime();
        $endDate->add(new DateInterval("P7D"));

        (new GetAppointmentTimesForm())->getAppointmentTimes(
            [
                $providerID => $providerIDType
            ],
            $physician,
            $startDate,
            $endDate
        );

        // refresh physician with new db data
        $physician = Entry::find()->id($physician->id)->one();

        if (!$this->physicianHasAppointmentBetween($physician, $startDate, $endDate)) {
            $startDate->add(new DateInterval("P7D"));
            $endDate->add(new DateInterval("P14D"));

            (new GetAppointmentTimesForm())->getAppointmentTimes(
                [
                    $providerID => $providerIDType
                ],
                $physician,
                $startDate,
                $endDate
            );


            // refresh physician with new db data
            $physician = Entry::find()->id($physician->id)->one();

            if (!$this->physicianHasAppointmentBetween($physician, $startDate, $endDate)) {
                $startDate->add(new DateInterval("P14D"));
                $endDate->add(new DateInterval("P21D"));

                (new GetAppointmentTimesForm())->getAppointmentTimes(
                    [
                        $providerID => $providerIDType
                    ],
                    $physician,
                    $startDate,
                    $endDate
                );

                // refresh physician with new db data
                $physician = Entry::find()->id($physician->id)->one();

                if (!$this->physicianHasAppointmentBetween($physician, $startDate, $endDate)) {
                    $startDate->add(new DateInterval("P21D"));
                    $endDate->add(new DateInterval("P28D"));

                    (new GetAppointmentTimesForm())->getAppointmentTimes(
                        [
                            $providerID => $providerIDType
                        ],
                        $physician,
                        $startDate,
                        $endDate
                    );
                }
            }
        }
    }

    private function cacheExternalResourceAppointmentTimes(string $externalResourceId)
    {
        $startDate = new DateTime();
        $endDate = new DateTime();
        $endDate->add(new DateInterval("P7D"));

        Craft::info(new PsrMessage('Physician Has Appointment Between.', [
            'startDate' => $startDate->format('Y-m-d H:i:s'),
            'endDate' => $endDate->format('Y-m-d H:i:s'),
            'externalResourceId' => $externalResourceId,
        ]), get_class($this) . '::' . __METHOD__);

        SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getResourcePhysicianAppointmentTimes($startDate, $endDate, $externalResourceId);


        $startDate->add(new DateInterval("P7D"));
        $endDate->add(new DateInterval("P14D"));

        SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getResourcePhysicianAppointmentTimes($startDate, $endDate, $externalResourceId);


        $startDate->add(new DateInterval("P14D"));
        $endDate->add(new DateInterval("P21D"));

        SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getResourcePhysicianAppointmentTimes($startDate, $endDate, $externalResourceId);


        $startDate->add(new DateInterval("P21D"));
        $endDate->add(new DateInterval("P28D"));

        SchedulingModule::getInstance()
            ->schedulingModuleEpicService
            ->getResourcePhysicianAppointmentTimes($startDate, $endDate, $externalResourceId);
    }

    /**
     * Undocumented function
     *
     * @param Entry $physician
     * @param DateTime $start
     * @param DateTime $end
     * @return boolean
     */
    private function physicianHasAppointmentBetween(Entry $physician, DateTime $start, DateTime $end): bool
    {
        $nextAppointment = $physician->physicianNextAvailableAppointmentTime;

        Craft::info(new PsrMessage('Physician Has Appointment Between.', [
            'nextAppointment' => $nextAppointment,
            'requestRangeDateStart' => $start->format('Y-m-d H:i:s'),
            'requestRangeDateEnd' => $end->format('Y-m-d H:i:s'),
            'physicianId' => $physician->id,
            'physicianTitle' => $physician->title,
            'physicianNationalProviderIdentifier' => $physician->nationalProviderIdentifier,
            'physicianEpicProviderId' => $physician->epicProviderId,
        ]), get_class($this) . '::' . __METHOD__);

        if ($nextAppointment === null) {
            return false;
        }

        $nextAppointment = DateTime::createFromFormat('Y-m-d H:i:s', $nextAppointment);

        if ($nextAppointment < $start) {
            return false;
        }

        if ($nextAppointment >= $start && $nextAppointment <= $end) {
            return true;
        }

        return false;
    }

    /**
     * @inheritdoc
     */
    protected function defaultDescription()
    {
        if ($this->physicianId) {
            return "Cache Physician Appointments [{$this->physicianId}]";
        } else {
            return "Cache External Resource Appointments [{$this->externalResourceId}]";
        }
    }
}
