<?php

namespace craft\contentmigrations;

// Yii classes
use Craft;
use craft\db\Migration;

// Craft classes
use craft\elements\Entry;
use craft\fields\Entries;
use craft\records\Field;
use craft\records\FieldLayout;
use craft\records\Section;
use craft\services\Relations;
use League\Csv\Reader;
use yii\console\Exception;

// 3rd-party classes
use yii\helpers\Inflector;

/**
 * m191210_231640_fixinsuranceplans migration.
 */
class m191210_231640_fixinsuranceplans extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $entries = Entry::find()
          ->section('insurancePlans')
          ->all();

        foreach ($entries as $entry) {
            Craft::$app->elements->deleteElement($entry);
        }

        // Loads the CSV file and offsets the first line (header)
        $csv = Reader::createFromPath(__DIR__ . '/../data/insurance_providers.csv', 'r');
        $csv->setHeaderOffset(0);

        // Insurance Provider field
        $insuranceProviderEntryField = Field::find()
            ->where(['uid' => 'e5654811-e7fa-46e4-b72a-01877e10fb10'])
            ->one();

        // Insurance Provider section
        $insuranceProvidersSection = Section::find()
            ->where(['uid' => '814af26c-9ee6-423b-baaf-6ec26b5399dc'])
            ->one();

        // Insurance Plan section
        $insurancePlansSection = Section::find()
            ->where(['uid' => '0cb638cf-4ccc-4a3d-a396-9a0ad705d575'])
            ->one();

        // Insurance Provider field layout
        $insuranceProviderFieldLayout = FieldLayout::find()
            ->where(['uid' => 'c904f488-dfa0-425a-ab8c-cbbe85b84613'])
            ->one();

        // Insurance Plan field layout
        $insuranceProviderPlanFieldLayout = FieldLayout::find()
            ->where(['uid' => '8e178623-934c-49c6-b338-9c11c23cd46b'])
            ->one();

        // Makes sure that all of the above is fetched and if not,
        if ($insuranceProviderEntryField === null ||
            $insuranceProvidersSection === null ||
            $insurancePlansSection === null ||
            $insuranceProviderFieldLayout === null ||
            $insuranceProviderPlanFieldLayout === null
        ) {
            throw new Exception('error');
        }

        $insuranceProviderEntry = null;
        // Iterates through the insurance provider and plan rows from the CSV
        foreach ($csv->getRecords() as $offset => $row) {
            if (!empty($row['Provider'])) {
                $insuranceProviderEntry = Entry::find()
                    ->where(['title' => $row['Provider']])
                    ->one();
            }

            // Insurance provider creation
            if ($insuranceProviderEntry === null && !empty($row['Provider'])) {
                    $insuranceProviderEntry = new Entry();
                    $insuranceProviderEntry->sectionId = $insuranceProvidersSection->id;
                    $insuranceProviderEntry->typeId = $insuranceProviderFieldLayout->id;
                    $insuranceProviderEntry->title = $row['Provider'];
                    $insuranceProviderEntry->setFieldValue('insuranceProviderURL', $row['Web URL']);
                    Craft::$app->elements->saveElement($insuranceProviderEntry);
            }

            // Insurance plan creation
            if ($insuranceProviderEntry !== null && !empty($row['Plan'])) {
                $insurancePlanEntry = new Entry();
                $insurancePlanEntry->sectionId = $insurancePlansSection->id;
                $insurancePlanEntry->typeId = $insuranceProviderPlanFieldLayout->id;
                $insurancePlanEntry->title = $row['Plan'];
                $insurancePlanEntry->slug = Inflector::slug($row['Plan']) . ' - ' . Inflector::slug($row['Provider']);
                $insurancePlanEntry->setFieldValues([
                    'isPlanMedicaid' => $row['Medicaid? '] === 'Yes' ? true : false,
                    'isPlanMedicare' => $row['Medicare? '] === 'Yes' ? true : false
                ]);


                if (!Craft::$app->elements->saveElement($insurancePlanEntry)) {
                    throw new \Exception('Didnt Save');
                }

                // Fetches the field type for the relation record between the plan and provider
                $planProviderField = new Entries();
                $planProviderField->id = $insuranceProviderEntryField->id;

                // Creates the relation
                $relation = new Relations();
                $relation->init();
                $relation->saveRelations($planProviderField, $insurancePlanEntry, [$insuranceProviderEntry->id]);
            }
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m191210_231640_fixinsuranceplans cannot be reverted.\n";
        return false;
    }
}
