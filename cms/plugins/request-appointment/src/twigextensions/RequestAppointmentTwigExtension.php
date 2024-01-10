<?php
namespace punchkick\requestappointment\twigextensions;

use punchkick\requestappointment\RequestAppointment;
use DateTime;
use Twig_SimpleFilter;
use Twig_SimpleFunction;

class RequestAppointmentTwigExtension extends \Twig_Extension
{
    /**
     * @inheritdoc
     */
    public function getFunctions()
    {
        return [
            new Twig_SimpleFunction('showPhysicalAndOccupationalAppointmentForm', [$this, 'showPhysicalAndOccupationalAppointmentForm'])
        ];
    }

    /**
     * @param string $handle
     * @param string $entryId
     *
     * @return bool
     */
    public function showPhysicalAndOccupationalAppointmentForm(string $handle, string $entryId)
    {
        return RequestAppointment::getInstance()
            ->requestAppointmentService
            ->showPhysicalAndOccupationalAppointmentForm($handle, $entryId);
    }
}
