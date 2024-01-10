<?php
/**
 * craft3-maintenance plugin for Craft CMS 3.x
 *
 * A maintenance mode plugin for Craft 3 developed by Punchkick Interactive
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

namespace punchkick\maintenance\records;

use Craft;

use craft\db\ActiveRecord;
use punchkick\maintenance\MaintenancePlugin;
use yii\base\Exception;

/**
 * MaintenancePlugin Record
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
 * @package   MaintenancePlugin
 * @since     0.0.1
 */
class MaintenanceRecord extends ActiveRecord
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
        return 'maintenance_plugin';
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['enabled'], 'required'],
            [['enabled'], 'boolean'],
            [['enabled'], 'safe']
        ];
    }

    /**
     * @inheritdoc
     */
    public function beforeValidate()
    {
        if (empty($this->enabled)) {
            $this->enabled = 0;
        }

        return parent::beforeValidate();
    }

    /**
     * Loads a site by the correct site id
     *
     * @return MaintenanceRecord
     */
    public static function loadSite($site)
    {
        $model = self::find()
            ->where(['siteId' => $site])
            ->one();

        if (!\in_array($site, Craft::$app->sites->allSiteIds)) {
            throw new Exception('Invalid site ID provided');
        }

        if ($model === null) {
            $model = new self;
            $model->enabled = 0;
            $model->siteId = $site;
            $model->save();
        }

        return $model;
    }

    /**
     * Loads or creates the current site configuration
     *
     * @return MaintenanceRecord
     */
    public static function loadCurrent()
    {
        return self::loadSite(Craft::$app->sites->currentSite->id);
    }

    /**
     * Enables maintenance mode for the site
     *
     * @return boolean
     */
    public function enable()
    {
        $this->enabled = 1;
        return $this->save();
    }

    /**
     * Disables maintenance mode for the site
     *
     * @return boolean
     */
    public function disable()
    {
        $this->enabled = 0;
        return $this->save();
    }
}
