<?php
namespace modules\schedulingmodule\twigextensions;

use Craft;
use craft\elements\Entry;
use DateTime;
use modules\DupageCoreModule\models\PatientUser;
use modules\physiciansmodule\PhysiciansModule;
use modules\schedulingmodule\SchedulingModule;
use Twig_SimpleFilter;
use Twig_SimpleFunction;

class SchedulingModuleTwigExtension extends \Twig_Extension
{
    /**
     * @inheritdoc
     */
    public function getName()
    {
        return 'SchedulingModule';
    }

    /**
     * @inheritdoc
     */
    public function getFilters()
    {
        return [
        ];
    }

    /**
     * @inheritdoc
     */
    public function getFunctions()
    {
        return [
            new Twig_SimpleFunction('initWebUser', [$this, 'initWebUser']),
            new Twig_SimpleFunction('getServiceDoctorName', [$this, 'getServiceDoctorName']),
            new Twig_SimpleFunction('determineIfAdditionalPromptNeeded', [$this, 'determineIfAdditionalPromptNeeded']),
            new Twig_SimpleFunction('determineStartDate', [$this, 'determineStartDate']),
            new Twig_SimpleFunction('findSuiteEntryForGivenExternalDepartmentID', [$this, 'findSuiteEntryForGivenExternalDepartmentID']),
            new Twig_SimpleFunction('getAvailablePhysicianAppointmentFromDate', [$this, 'getAvailablePhysicianAppointmentFromDate']),
            new Twig_SimpleFunction('getSchedulableServicesForPhysician', [$this, 'getSchedulableServicesForPhysician']),
            new Twig_SimpleFunction('verifyServicesByAge', [$this, 'verifyServicesByAge'])
        ];
    }

    /**
     * Creates a PatientUser web identity if necessary
     */
    public function initWebUser()
    {
        // if not logged in, log in anonymous user
        $user = Craft::$app->patient_user->identity;
        if (!$user) {
            $user = PatientUser::anonymousUser();
            Craft::$app->patient_user->login($user, (60 * 20));

            if ($user->save()) {
                return true;
            }

            return false;
        }

        return true;
    }

    /**
     * @param array $serviceIds
     *
     * @return string
     */
    public function getSchedulableServicesForPhysician(Entry $physician, Int $limit = null)
    {
        return SchedulingModule::getInstance()
            ->schedulingModuleService
            ->getSchedulableServicesForPhysician($physician, $limit);
    }

    /**
     * @param Entry $physician
     * @param DateTime $date
     *
     * @return mixed
     */
    public function getAvailablePhysicianAppointmentFromDate(Entry $physician, DateTime $date)
    {
        return PhysiciansModule::getInstance()
            ->physiciansModuleService
            ->getAvailablePhysicianAppointmentFromDate($physician, $date);
    }

    /**
     * @param array $serviceIds
     * @param string $handle
     *
     * @return string
     */
    public function determineIfAdditionalPromptNeeded(array $serviceIds = [], string $handle = '')
    {
        return SchedulingModule::getInstance()
            ->schedulingModuleService
            ->determineIfAdditionalPromptNeeded($serviceIds, $handle);
    }

    /**
     *
     * @return mixed
     */
    public function determineStartDate()
    {
        return SchedulingModule::getInstance()
            ->schedulingModuleService
            ->determineStartDate();
    }

    /**
     * @param array $serviceIds
     *
     * @return string
     */
    public function getServiceDoctorName(array $serviceIds = [])
    {
        return SchedulingModule::getInstance()
            ->schedulingModuleService
            ->getServiceDoctorName($serviceIds);
    }

    /**
     * @param string $externalDepartmentId
     *
     * @return Entry|null
     */
    public function findSuiteEntryForGivenExternalDepartmentID(string $externalDepartmentId)
    {
        return SchedulingModule::getInstance()
            ->schedulingModuleService
            ->findSuiteEntryForGivenExternalDepartmentID($externalDepartmentId);
    }

    /**
     * @param array $serviceIds
     * @param int $age
     *
     * @return array|null
     */
    public function verifyServicesByAge(array $serviceIds, int $age)
    {
        return SchedulingModule::getInstance()
            ->schedulingModuleService
            ->verifyServicesByAge($serviceIds, $age);
    }
}
