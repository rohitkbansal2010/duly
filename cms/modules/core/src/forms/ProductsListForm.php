<?php

/**
 * DupageCoreModule module for Craft CMS 3.x
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\DupageCoreModule\forms;

use Craft;
use craft\elements\Entry;
use yii\base\Model;
use yii\data\ActiveDataProvider;
use yii\web\UrlManager;

/**
 * @author    Wipfli Digital
 * @package   DupageCoreModule
 * @since     1.0.0
 */
class ProductsListForm extends Model
{
    /**
     * @var array $brand
     */
    public $brand;

    /**
     * @var array $conditionId
     */
    public $conditionId;

    /**
     * @var string $query
     */
    public $query;

    /**
     * @var int $pageSize
     */
    public $pageSize = 20;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            ['query', 'string'],
            ['brand', 'each', 'rule' => ['string']],
            ['conditionId', 'each', 'rule' => ['number']],
            [['brand', 'query', 'conditionId'], 'safe']
        ];
    }

    /**
     * Fetches a list of products given the parameters
     * @return array
     */
    public function getProducts()
    {
        if ($this->validate()) {
            $query = Entry::find()->section('products')->orderBy('title');

            if (\is_array($this->brand) && count($this->brand) > 0) {
                $query->where(['in', 'field_brand', $this->brand]);
            }

            if (\is_array($this->conditionId) && count($this->conditionId) > 0) {
                $query->relatedTo($this->conditionId);
            }

            if ($this->query !== null) {
                $query->search("*" . $this->query . "*");
            }

            $dataProvider = new ActiveDataProvider([
                'query' => $query,
                'pagination' => [
                    'pageSize' => $this->pageSize,
                    'totalCount' => $query->count(),
                    'urlManager' => new UrlManager([
                        'enablePrettyUrl'=> true,
                        'showScriptName' => false,
                        'rules'=>[
                            '/cosmetic-dermatology-products' => 'dupage-core-module/default/products',
                        ]
                    ])
                ]
            ]);
            
            return $dataProvider;
        }

        $this->addError('products', Craft::t('dupage-core-module', 'We were unable to fetch a list of products.'));
        return [];
    }

    /**
     * Returns an array of strings of brand names assigned to product entries
     * and removes empty/duplicate values
     *
     * @param Array $products
     */
    public function getBrands()
    {
        return array_unique(
            array_filter(array_map(
                function ($product) {
                    return $product->brand;
                },
                Entry::find()->section('products')->all()
            ))
        );
    }

    /**
     * Returns an array of conditions related to the list of products
     *
     * @param Array $products
     */
    public function getConditions()
    {
        $products = Entry::find()->section('products')->all();
        if (empty($products)) {
            return null;
        }
        return Entry::find()
            ->section('conditions')
            ->relatedTo($products);
    }
}
