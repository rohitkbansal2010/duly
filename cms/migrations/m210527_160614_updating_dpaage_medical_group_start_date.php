<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\elements\Entry;
use DateTime;
use Exception;
use League\Csv\Reader;
use Throwable;

/**
 * m210527_160614_updating_dpaage_medical_group_start_date migration.
 */
class m210527_160614_updating_dpaage_medical_group_start_date extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $transaction = Craft::$app->db->beginTransaction();
        try {
            // Loads the CSV file and offsets the first line (header)
            $csv = Reader::createFromPath(__DIR__ . '/../data/dmg_start_date_report.csv', 'r');
            $csv->setHeaderOffset(0);

            $columns = ['field_nationalProviderIdentifier', 'field_dupageMedicalGroupStartDate'];
            $rows = [];

            foreach ($csv->getRecords() as $row) {
                // change d/m/y date format to d/m/Y (i.e. 23/05/13 to 23/05/2013)
                $date = $row['Start Date'];
                $date = DateTime::createFromFormat('d/m/y', $date);
                $date = $date->format('d/m/Y');

                $npi = $row['NPI'];

                $rows[] = [
                    $npi,
                    $date
                ];
            }

            Craft::$app->db->createCommand(
                    <<<EOD
                    CREATE TABLE IF NOT EXISTS npi_and_start_date
                        (
                            field_nationalProviderIdentifier VARCHAR(255) NOT NULL DEFAULT '',
                            field_dupageMedicalGroupStartDate VARCHAR(255) NOT NULL DEFAULT ''
                        );
                    EOD
                )
                ->execute();

            $sql = Craft::$app->db->queryBuilder->batchInsert(
                'npi_and_start_date',
                $columns,
                $rows
            );
            Craft::$app->db->createCommand($sql)->execute();

            $sql = <<<EOD
                UPDATE content
                SET field_dupageMedicalGroupStartDate = (
                        SELECT field_dupageMedicalGroupStartDate
                        FROM npi_and_start_date
                        WHERE field_nationalProviderIdentifier = content.field_nationalProviderIdentifier
                        LIMIT 1
                    )
                ;
            EOD;
            Craft::$app->db->createCommand($sql)->execute();

            $transaction->commit();
        } catch (Exception | Throwable $e) {
            $transaction->rollBack();
            throw $e;
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        return true;
    }
}
