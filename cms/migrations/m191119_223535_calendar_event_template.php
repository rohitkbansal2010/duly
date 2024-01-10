<?php

namespace craft\contentmigrations;

// Craft classes
use Craft;
use craft\db\Migration;

// 3rd-party classes
use Solspace\Calendar\Records\CalendarRecord;
use Solspace\Calendar\Records\CalendarSiteSettingsRecord;

/**
 * m191119_223535_calendar_event_template migration.
 */
class m191119_223535_calendar_event_template extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $calendar = CalendarRecord::findOne(['handle' => 'dupage_event_calendar']);

        $calendarSite = CalendarSiteSettingsRecord::findOne([
            'siteId' =>  Craft::$app->sites->getPrimarySite()->id,
            'calendarId' => $calendar->id
        ]);

        $calendarSite->enabledByDefault = 1;
        $calendarSite->hasUrls = 1;
        $calendarSite->uriFormat = '/events/{slug}';
        $calendarSite->template = '_events/events.twig';

        $calendarSite->save();
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m191119_223535_calendar_event_template cannot be reverted.\n";
        return false;
    }
}
