<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\elements\Entry;
use craft\elements\GlobalSet;
use craft\elements\MatrixBlock;

/**
 * m200103_150752_footerdetails_init migration.
 */
class m200103_150752_footerdetails_init extends Migration
{
    private $_globals = [
        'addressList' => [
            [
                'type' => 'addressList',
                'enabled' => 1,
                'fields' => [
                    'locationName' => 'Administration Office',
                    'address' => '{"address":"1100 W 31st St, Suite 300 Downers Grove, IL 60515","zoom":15,"lat":"41.831138507587","lng":"-88.0131563396283","parts":{"number":"1100","address":"31st Street, Suite 300","city":"Downers Grove","postcode":"60515","county":"DuPage County","state":"Illinois","country":"United States"}}'
                ]
            ]
        ],
        'phoneList' => [
            [
                'type' => 'phoneList',
                'enabled' => 1,
                'fields' => [
                    'phoneListName' => 'Main Line',
                    'phoneListNumber' => [
                        'region' => 'US',
                        'number' => '(630) 469 9200'
                    ]
                ]
            ],
            [
                'type' => 'phoneList',
                'enabled' => 1,
                'fields' => [
                    'phoneListName' => 'Customer Service',
                    'phoneListNumber' => [
                        'region' => 'US',
                        'number' => ' (630) 942 7998'
                    ]
                ]
            ]
        ]
    ];

    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $footerDetails = Craft::$app->globals->getSetByHandle('footerDetails');

        foreach ($this->_globals as $field => $global) {
            $footerDetails->setFieldValue($field, $global);
        }

        if (Craft::$app->elements->saveElement($footerDetails)) {
            return true;
        }

        return false;
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        $footerDetails = Craft::$app->globals->getSetByHandle('footerDetails');
        foreach ($this->_globals as $field => $global) {
            $footerDetails->setFieldValue($field, '');
        }

        if (Craft::$app->elements->saveElement($footerDetails)) {
            return true;
        }

        return false;
    }
}
