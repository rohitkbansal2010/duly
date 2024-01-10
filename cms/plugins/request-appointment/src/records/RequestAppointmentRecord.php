<?php
/**
 * Request Appointment plugin for Craft CMS 3.3
 *
 * This plugin provides way to manage requesting an appointment via forms
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\requestappointment\records;

use Craft;
use DateTime;
use yii\base\Exception;
use craft\db\ActiveRecord;

/**
 * RequestAppointmentPlugin Record
 *
 * ActiveRecord is the base class for classes representing relational data in terms of objects.
 *
 * Active Record implements the [Active Record design pattern](http://en.wikipedia.org/wiki/Active_record).
 * The premise behind Active Record is that an individual [[ActiveRecord]] object is associated with a specific
 * row in a database table. The object's attributes are mapped to the columns of the corresponding table.
 * Referencing an Active Record attribute is equivalent to accessing the corresponding table column for that record.
 *
 * http://www.yiiframework.com/doc-2.0/guide-db-active-record.html
 *
 * @author    Punchkick Interactive
 * @package   RequestAppointmentPlugin
 * @since     0.0.1
 */
class RequestAppointmentRecord extends ActiveRecord
{
    // Public Static Methods
    // =========================================================================

     /**
     * Declares the name of the database table associated with this AR class.
     * By default this method returns the class name as the table name by calling [[Inflector::camel2id()]]
     * with prefix [[Connection::tablePrefix]]. For example if [[Connection::tablePrefix]] is `tbl_`,
     * `Customer` becomes `tbl_customer`, and `OrderItem` becomes `tbl_order_item`. You may override this method
     * if the table is not named after this convention.
     *
     * By convention, tables created by plugins should be prefixed with the plugin
     * name and an underscore.
     *
     * @return string the table name
     */
    public static function tableName()
    {
        return'{{%requestappointment_requestappointmentrecord}}';
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['type', 'location', 'first_name', 'last_name', 'best_time','phone_number', 'dob', 'date1', 'date2', 'date3', 'window1', 'window2', 'window3', 'insurance', 'order', 'work_comp', 'communication', 'symptoms'], 'required'],
            [['type', 'location', 'first_name', 'last_name', 'best_time', 'phone_number', 'dob', 'date1', 'date2', 'date3', 'window1', 'window2', 'window3', 'insurance', 'order', 'work_comp', 'communication', 'symptoms'], 'string'],
            ['dob', 'date', 'format' => 'php:Y-m-d'],
            ['date1', 'date', 'format' => 'php:Y-m-d'],
            ['date2', 'date', 'format' => 'php:Y-m-d'],
            ['date3', 'date', 'format' => 'php:Y-m-d'],
            [['processed'], 'default', 'value' => 0],
            [['dateCreated'], 'default', 'value' => (new DateTime)->format('Y-m-d h:i:s')],
            [['processed'], 'boolean'],
            [['type', 'location', 'first_name', 'last_name', 'best_time', 'phone_number', 'window1', 'window2', 'window3', 'dob', 'date1', 'date2', 'date3', 'insurance', 'order', 'work_comp', 'communication', 'symptoms', 'processed'], 'safe']
        ];
    }
}
