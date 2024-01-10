<?php

namespace modules\apimodule\forms;

use Craft;
use DateTime;
use craft\elements\Category;
use craft\elements\Entry;
use modules\apimodule\ApiModule;
use yii\base\Model;
use yii\data\ActiveDataProvider;

use yii\web\UrlManager;

/**
 * Hopspital Form
 *
 * This form wraps the logic for requesting a list of hospitals based on provided parameters.
 *
 * This form is tailored for use in /api endpoints.
 */
final class HospitalForm extends Model
{
    /**
     * @var int $zip_code
     */
    public $zip_code;

    /**
     * @var float $latitude
     */
    public $latitude;

    /**
     * @var float $longitude
     */
    public $longitude;

    /**
     * @var int $pageSize
     */
    public $pageSize = 15;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['latitude', 'longitude', 'zip_code'], 'number'],
            [['latitude', 'longitude', 'zip_code'], 'safe']
        ];
    }

    /**
     * This method returns a list of hospital objects and their data.
     *
     * @return array
     */
    public function getHospitals()
    {
        if (!$this->validate()) {
            return [];
        }

        $query = Entry::find()->section('hospitals')->with(['phoneList']);

        if ($this->zip_code) {
            ApiModule::getInstance()
                ->apiModuleService
                ->sortByZipCode($query, $this->zip_code);
        } elseif ($this->latitude && $this->longitude) {
            ApiModule::getInstance()
                ->apiModuleService
                ->sortByCoordinates($query, (float) $this->latitude, (float) $this->longitude);
        }

        $hospitals = new ActiveDataProvider([
            'query' => $query,
            'pagination' => [
                'pageSize' => $this->pageSize,
                'totalCount' => $query->count()
            ]
        ]);

        // convert Hospital Models into simple objects with only supported keys
        return \array_map(
            function ($hospital) {
                $hospitalInfo = [
                    'name' => $hospital->title,
                    'phone' => [],
                    'address' => [
                        'number' => $hospital->address->parts->number,
                        'street' => $hospital->address->parts->address,
                        'city' => $hospital->address->parts->city,
                        'postcode' => $hospital->address->parts->postcode,
                        'county' => $hospital->address->parts->county,
                        'state' => $hospital->address->parts->state,
                        'lat' => (float) $hospital->address->lat,
                        'lng' => (float) $hospital->address->lng
                    ]
                ];

                foreach ($hospital->phoneList as $phone) {
                    $hospitalInfo['phone'][] = $phone->phoneListNumber->number;
                }

                return $hospitalInfo;
            },
            $hospitals->getModels()
        );
    }
}
