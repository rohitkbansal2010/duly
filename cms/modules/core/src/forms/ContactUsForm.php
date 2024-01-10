<?php

namespace modules\DupageCoreModule\forms;

use yii\base\Model;
use modules\DupageCoreModule\DupageCoreModule;

final class ContactUsForm extends Model
{
    /**
     * @var string $emailAddress
     */
    public $emailAddress;
    
    /**
     * @var string $name
     */
    public $name;
    
    /**
     * @var string $address
     */
    public $address;
    
    /**
     * @var string $city
     */
    public $city;
    
    /**
     * @var string $state
     */
    public $state;
    
    /**
     * @var string $zipcode
     */
    public $zipcode;
    
    /**
     * @var string $phoneNumber
     */
    public $phoneNumber;
    
    /**
     * @var string $subject
     */
    public $subject;
    
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
            [['emailAddress'], 'email'],
            [['name', 'address', 'city', 'state', 'subject', 'comments', 'phoneNumber'], 'string'],
            [['zipcode'], 'number'],
            [['emailAddress', 'name', 'phoneNumber', 'comments'], 'required'],
            ['recaptchaToken', 'string'],
            ['recaptchaToken', 'required', 'message' => 'Something went wrong. Please try again.'],
            ['recaptchaToken', 'validateRecaptchaToken'],
            [['emailAddress', 'name', 'address', 'city', 'state', 'subject', 'comments', 'zipcode', 'phoneNumber', 'city', 'state', 'recaptchaToken'], 'safe']
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
}
