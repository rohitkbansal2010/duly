<?php

namespace craft\contentmigrations;

// Yii classes
use yii\db\Expression;

// Craft classes
use Craft;
use craft\db\Query;
use craft\db\Migration;
use craft\models\FieldLayout;
use craft\models\FieldLayoutTab;
use craft\helpers\StringHelper;
use craft\records\FieldLayoutField;

// 3rd-party classes
use Solspace\Calendar\Records\CalendarRecord;

/**
 * m190917_155154_calendar_events migration.
 */
class m190917_155154_calendar_events extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        // The list of required fields for the calendar (field UIDs were fetched from the 'config/project.yaml' file)
        $fieldUidList = [
            '696db984-4103-4b99-90cb-5f78d8658aef',
            '9b6fe455-d1f9-4a47-a15c-29bfd090cddd',
            'bfd9d59d-fd7a-41e5-a0b6-a585fa6e5cea',
            'a1e85583-8fbe-4ff8-a3f1-6ed03df017e0',
            'd831c234-e6dd-4ba0-a355-914da530cfc1',
            '01084a5c-c07f-4cde-a629-c8156be9f742',
            '9e689281-d90b-4c1f-90f4-ce7c3b9d8d4b',
            'ec029f2c-b3cd-4494-acf2-680c1ec06ee7'
        ];

        // Fetches the fields in the same order that they're in in the $fieldUidList
        $fields = (new Query())
            ->select(['id', 'uid'])
            ->from('fields')
            ->where(['in', 'uid', $fieldUidList])
            ->orderBy([new Expression('FIELD (uid, \'' . implode('\',\'', $fieldUidList) . '\')')])
            ->limit(count($fieldUidList))
            ->all();

        // Creates a list of FieldLayoutField objects
        $fieldLayoutFieldList = [];
        $sortOrder = 1;
        foreach ($fields as $field) {
            $fieldLayoutField = new FieldLayoutField();
            $fieldLayoutField->id = $field['id'];
            $fieldLayoutField->sortOrder = $sortOrder++;
            if ($field['uid'] === 'a1e85583-8fbe-4ff8-a3f1-6ed03df017e0' ||
                $field['uid'] === 'ec029f2c-b3cd-4494-acf2-680c1ec06ee7'
            ) {
                $fieldLayoutField->required = false;
            } else {
                $fieldLayoutField->required = true;
            }

            $fieldLayoutFieldList[] = $fieldLayoutField;
        }

        // Creates a FieldLayoutTab with the list of FieldLayoutField objects
        $fieldLayoutTab = new FieldLayoutTab();
        $fieldLayoutTab->name = 'Events';
        $fieldLayoutTab->sortOrder = 1;
        $fieldLayoutTab->setFields($fieldLayoutFieldList);
        $fieldLayoutTab->uid = StringHelper::UUID();

        // Creates a FieldLayout with the previously created FieldLayoutTab
        $fieldLayout = new FieldLayout();
        $fieldLayout->type = 'Solspace\Calendar\Elements\Event';
        $fieldLayout->setTabs([$fieldLayoutTab]);
        $fieldLayout->uid = StringHelper::UUID();
        Craft::$app->fields->saveLayout($fieldLayout);

        // Updates the 'default' calendar with the newly created FieldLayout and also updates some other params
        $calendar = CalendarRecord::findOne(['handle' => 'default']);
        $calendar->name = 'DuPage Medical Group\'s Event Calendar';
        $calendar->handle = 'dupage_event_calendar';
        $calendar->fieldLayoutId = $fieldLayout->id;
        $calendar->description = 'The event calendar for the DuPage Medical Group.';
        // Bright green
        $calendar->color = '#1BF92A';
        $calendar->titleLabel = 'Event Title';
        $calendar->hasTitleField = 1;
        $calendar->save();
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m190917_155154_calendar_events cannot be reverted.\n";
        return false;
    }
}
