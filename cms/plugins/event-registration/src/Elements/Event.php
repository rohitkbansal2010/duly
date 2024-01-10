<?php

namespace punchkick\eventregistration\Elements;

use Solspace\Calendar\Elements\Event as BaseEvent;
use punchkick\eventregistration\Elements\Db\EventQuery;
use craft\elements\db\ElementQueryInterface;
use Symfony\Component\PropertyAccess\PropertyAccessor;
use Solspace\Calendar\Calendar;

class Event extends BaseEvent
{
    public $internalLocationId;

    /**
     * @return EventQuery|ElementQueryInterface
     */
    public static function find(): ElementQueryInterface
    {
        return new EventQuery(self::class);
    }

    /**
     * @param array $config
     *
     * @return EventQuery|ElementQueryInterface
     */
    public static function buildQuery(array $config = null): ElementQueryInterface
    {
        $query = static::find();

        if (null !== $config) {
            $propertyAccessor = new PropertyAccessor();

            foreach ($config as $key => $value) {
                if ($propertyAccessor->isWritable($query, $key)) {
                    $propertyAccessor->setValue($query, $key, $value);
                }
            }
        }

        $query->setOverlapThreshold(Calendar::getInstance()->settings->getOverlapThreshold());
        $query->siteId = $query->siteId ?? \Craft::$app->sites->currentSite->id;

        return $query;
    }
}
