<?php

namespace craft\contentmigrations;

// Craft classes
use Craft;
use craft\db\Migration;
use craft\elements\Entry;
use craft\records\Section;
use craft\records\EntryType;

/**
 * m190912_202937_insurance_overview_defaults migration.
 */
class m190912_202937_insurance_overview_defaults extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $section = Section::find()
            ->where(['uid' => '69f67b70-8971-4843-8b2b-e7328330e4f7'])
            ->one();

        $entryType = EntryType::find()
            ->where(['uid' => 'ffc5a751-4ce2-4598-bdeb-9fe5f1200171'])
            ->one();

        $entry = Entry::find()
            ->where(['sectionId' => $section->id])
            ->where(['typeId' => $entryType->id])
            ->one();

        $entry->setFieldValues([
            'insuranceProviderHeaderCopy' => '<p>At DMG, we proudly accept plans from the following insurance providers:</p>',
            'insuranceProviderDisclaimerCopy' => '<p>This listing reflects current DMG contracts. However individual physician’s network participation may vary. In order to receive the highest level of benefits, <strong>please check with your health plan by calling the number on the back of your insurance card to verify physician participation.</strong> Please note that i
            nsurance company websites may not be up-to-date.</p>',
            'insuranceProviderMedicareLink' => 'https://www.youtube.com/watch?v=Wy2-GgpA4Gk&feature=youtu.be'
        ]);

        Craft::$app->content->saveContent($entry);
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m190912_202937_insurance_overview_defaults cannot be reverted.\n";
        return false;
    }
}
