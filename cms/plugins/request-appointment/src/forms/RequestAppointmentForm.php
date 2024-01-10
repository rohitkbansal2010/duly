<?php

namespace punchkick\requestappointment\forms;

use Craft;
use yii\base\Model;
use punchkick\requestappointment\records\RequestAppointmentRecord;
use modules\DupageCoreModule\DupageCoreModule;
use modules\DupageCoreModule\queue\EmailJob;
use craft\elements\Entry;

final class RequestAppointmentForm extends Model
{
    /**
     * @var string $type
     */
    public $type;

    /**
     * @var string $location
     */
    public $location;

    /**
     * @var string $locationId
     */
    public $locationId;

    /**
     * @var string $procedure
     */
    public $procedure;

    /**
     * @var string $first_name
     */
    public $first_name;

    /**
     * @var string $last_name
     */
    public $last_name;

    /**
     * @var string $phone_number
     */
    public $phone_number;

    /**
     * @var string $email
     */
    public $email;

    /**
     * @var string $dob
     */
    public $dob;

    /**
     * @var string $best_time
     */
    public $best_time;

    /**
     * @var string $date1
     */
    public $date1;

    /**
     * @var string $window1
     */
    public $window1;

    /**
     * @var string $date2
     */
    public $date2;

    /**
     * @var string $window2
     */
    public $window2;

    /**
     * @var string $date3
     */
    public $date3;

    /**
     * @var string $window3
     */
    public $window3;

    /**
     * @var string $insurance
     */
    public $insurance;

    /**
     * @var string $order
     */
    public $order;

    /**
     * @var string $work_comp
     */
    public $work_comp;

    /**
     * @var string $communication
     */
    public $communication;

    /**
     * @var string $symptoms
     */
    public $symptoms;

    /**
     * @var string $comments
     */
    public $comments;

    /**
     * @var string $recaptchaToken
     */
    public $recaptchaToken;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['type', 'location', 'procedure', 'first_name', 'last_name', 'phone_number', 'email', 'best_time', 'dob', 'date1', 'date2', 'date3', 'window1', 'window2', 'window3', 'insurance', 'order', 'work_comp', 'communication', 'symptoms', 'comments'], 'string'],
            ['locationId', 'number'],
            ['dob', 'date', 'format' => 'php:Y-m-d'],
            ['date1', 'date', 'format' => 'php:Y-m-d'],
            ['date2', 'date', 'format' => 'php:Y-m-d'],
            ['date3', 'date', 'format' => 'php:Y-m-d'],
            ['recaptchaToken', 'required', 'message' => 'Something went wrong. Please try again.'],
            ['recaptchaToken', 'validateRecaptchaToken'],
            [['type', 'location', 'procedure', 'first_name', 'last_name', 'phone_number', 'email', 'best_time', 'window1', 'window2', 'window3', 'dob', 'date1', 'date2', 'date3', 'insurance', 'order', 'work_comp', 'communication', 'symptoms', 'comments', 'locationId'], 'safe']
        ];
    }

    /**
     * Validates reCAPTCHA token
     *
     * @param mixed $attribute
     * @param string $param
     * @return boolean
     */
    public function validateRecaptchaToken($attribute, $param)
    {
        $verified = DupageCoreModule::getInstance()
            ->dupageCoreModuleService
            ->verifyRecaptchaToken($this->$attribute);

        if (!$verified) {
            $this->addError($attribute, 'Something went wrong. Please try again.');
        }
    }

    /**
     * Handles the database interaction
     *
     * @return boolean
     */
    public function save()
    {
        if ($this->validate()) {
            $model = new RequestAppointmentRecord;
            $model->load(['RequestAppointmentRecord' => $this->getAttributes()]);
            $siteId = Craft::$app->sites->currentSite->id;

            if ($siteId) {
                $model->setAttribute('siteId', $siteId);
            }

            if ($model->save()) {
                return true;
            }
        }

        return false;
    }

    public function sendCosmeticDermatologyEmail()
    {
        $email = Entry::find()->id($this->locationId)->one()->cosmeticDermatologyFormRecipientEmail;
        
        Craft::$app->queue->push(new EmailJob([
            'template' => 'cosmetic-dermatology.twig',
            'subject' => Craft::t('dupage-core-module', 'Cosmetic Dermatology Form Submission'),
            'to' => array_map('trim', explode(',', $email)),
            'templateData' => [
                'first_name' => $this->first_name,
                'last_name' => $this->last_name,
                'email' => $this->email,
                'phone_number' => $this->phone_number,
                'location' => $this->location,
                'procedure' => $this->procedure,
                'comments' => $this->comments
            ],
        ]));
    }
}
