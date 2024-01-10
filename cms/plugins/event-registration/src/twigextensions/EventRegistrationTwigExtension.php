<?php
/**
 * Event Registration plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage event registrants.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2020 Punchkick Interactive
 */

namespace punchkick\eventregistration\twigextensions;

use craft\elements\Entry;

use craft\elements\MatrixBlock;
use craft\records\MatrixBlockType;
use DateTime;
use punchkick\eventregistration\EventRegistration;

use Solspace\Calendar\Elements\Event;
/**
 * Twig can be extended in many ways; you can add extra tags, filters, tests, operators,
 * global variables, and functions. You can even extend the parser itself with
 * node visitors.
 *
 * http://twig.sensiolabs.org/doc/advanced.html
 *
 * @author    Punchkick Interactive
 * @package   EventRegistration
 * @since     0.0.1
 */
class EventRegistrationTwigExtension extends \Twig_Extension
{
    // Public Methods
    // =========================================================================

    /**
     * Returns the name of the extension.
     *
     * @return string The extension name
     */
    public function getName()
    {
        return 'EventRegistrationPlugin';
    }

    /**
     * Returns an array of Twig filters, used in Twig templates via:
     *
     *      {{ 'something' | someFilter }}
     *
     * @return array
     */
    public function getFilters()
    {
        return [
        ];
    }

    /**
     * Returns an array of Twig functions, used in Twig templates via:
     *
     *      {% set this = someFunction('something') %}
     *
    * @return array
     */
    public function getFunctions()
    {
        return [
            new \Twig_SimpleFunction('getRegistrantsCountForEventByLocationAndTime', [$this, 'getRegistrantsCountForEventByLocationAndTime']),
            new \Twig_SimpleFunction('getServicesForEvents', [$this, 'getServicesForEvents']),
        ];
    }

    public function getRegistrantsCountForEventByLocationAndTime(Event $event, MatrixBlock $location, DateTime $date = null)
    {
        return EventRegistration::$plugin->eventRegistrationService->getRegistrantsCountForEventByLocationAndTime($event, $location, $date);
    }

    public function getServicesForEvents(array $eventParams)
    {
        return EventRegistration::$plugin->eventRegistrationService->getServicesForEvents($eventParams);
    }
}
