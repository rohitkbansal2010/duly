<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use DateTime;
use Exception;
use League\Csv\Reader;
use Throwable;

/**
 * m210610_193846_fix_wrong_physician_start_dates migration.
 */
class m210610_193846_fix_wrong_physician_start_dates extends Migration
{
    /**
     * @inheritdoc
     */
    public function safeUp()
    {

        // Loads the CSV file and offsets the first line (header)
        $csv = Reader::createFromPath(__DIR__ . '/../data/dmg_start_date_report.csv', 'r');
        $csv->setHeaderOffset(0);

        $rows = [];

        foreach ($csv->getRecords() as $row) {
            // change d/m/y date format to d/m/Y (i.e. 23/05/13 to 23/05/2013)
            $date = $row['Start Date'];
            $date = DateTime::createFromFormat('m/d/y', $date);
            $date = $date->format('m/d/Y');

            $npi = $row['NPI'];

            $rows[] = [
                $npi,
                $date
            ];

            // apply migration 200 rows at a time to avoid timeouts
            if (count($rows) >= 200) {
                $this->applyMigration($rows);
                $rows = [];
            }
        }

        // apply migration for the reminder of the rows
        if (count($rows) > 0) {
            $this->applyMigration($rows);
            $rows = [];
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m210610_193846_fix_wrong_physician_start_dates cannot be reverted.\n";
        return true;
    }

    private function applyMigration(array $rows)
    {
        $columns = ['field_nationalProviderIdentifier', 'field_dupageMedicalGroupStartDate'];

        $transaction = Craft::$app->db->beginTransaction();
        try {
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

            Craft::$app->db->createCommand(
                <<<EOD
                        DELETE FROM npi_and_start_date;
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
}
