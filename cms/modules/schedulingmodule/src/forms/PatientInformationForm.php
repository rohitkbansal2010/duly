<?php

namespace modules\schedulingmodule\forms;

use Craft;
use yii\base\Model;
use craft\elements\Entry;
use craft\elements\Category;

final class PatientInformationForm extends Model
{
	/**
	 * @var string $firstName
	 */
	public $firstName;

	/**
	 * @var string $firstName
	 */
	public $lastName;

	/**
	 * @var string $gender
	 */
	public $gender;

	/**
	 * @var string $emailAddress
	 */
	public $emailAddress;

	/**
	 * @var string $phoneNumber
	 */
	public $phoneNumber;

	/**
	 * @var string $primaryAddress
	 */
	public $primaryAddress;

	/**
	 * @var string $secondaryAddress
	 */
	public $secondaryAddress;

	/**
	 * @var string $city
	 */
	public $city;

	/**
	 * @var string $state
	 */
	public $state;

	/**
	 * @var number $zipcode
	 */
	public $zipcode;

	/**
	 * @var string $patientReasonForVisit
	 */
	public $patientReasonForVisit;

	/**
	 * @var string $additionalReasonComment
	 */
	public $additionalReasonComment;

	/**
	 * @var string $date_of_birth_month
	 */
	public $date_of_birth_month;

	/**
	 * @var string $date_of_birth_day
	 */
	public $date_of_birth_day;

	/**
	 * @var string $date_of_birth_year
	 */
	public $date_of_birth_year;

	/**
	 * @var string $insurance_member_id
	 */
	public $insurance_member_id;

	/**
	 * @var string $insurance_group_id
	 */
	public $insurance_group_id;

	/**
	 * @var string $appointment_insurance_provider_id
	 */
	public $appointment_insurance_provider_id;

	/**
	 * @inheritdoc
	 */
	public function load($data, $formName = null)
	{
		if (!parent::load($data, $formName)) {
			return false;
		}

		$user = Craft::$app->patient_user->identity;

		if (isset($this->date_of_birth_day)) {
			$user->date_of_birth_day = $this->date_of_birth_day;
		}

		if (isset($this->appointment_insurance_provider_id)) {
			$user->appointment_insurance_provider_id = $this->appointment_insurance_provider_id;
		}

		if (isset($this->date_of_birth_year)) {
			$user->date_of_birth_year = $this->date_of_birth_year;
		}

		if (isset($this->date_of_birth_month)) {
			$user->date_of_birth_month = $this->date_of_birth_month;
		}

		$user->save();

		return true;
	}

	/**
	 * @inheritdoc
	 */
	public function rules()
	{
		return [
			[['firstName', 'lastName', 'emailAddress', 'gender', 'phoneNumber', 'emailAddress', 'primaryAddress', 'secondaryAddress', 'city', 'state', 'patientReasonForVisit', 'additionalReasonComment', 'date_of_birth_month', 'date_of_birth_day', 'date_of_birth_year', 'insurance_member_id', 'insurance_group_id', 'appointment_insurance_provider_id'], 'string'],
			['patientReasonForVisit', 'validateReason'],
			[['zipcode'], 'number'],
			[['firstName', 'lastName', 'emailAddress', 'gender', 'phoneNumber', 'emailAddress', 'primaryAddress', 'secondaryAddress', 'city', 'state', 'zipcode', 'patientReasonForVisit', 'additionalReasonComment', 'date_of_birth_month', 'date_of_birth_day', 'date_of_birth_year', 'insurance_member_id', 'insurance_group_id', 'appointment_insurance_provider_id'], 'safe']
		];
	}

	/**
	 * Validates a reason
	 *
	 * @param mixed $attribute
	 * @param string $param
	 * @return boolean
	 */
	public function validateReason($attribute, $param)
	{
		$user = Craft::$app->patient_user->identity;

		$service = Entry::find()
			->section('services')
			->id($user->getAppointmentServiceIds())
			->one();

		if ($service != null) {
			$reasons = $service->servicePatientReasonsForVisit->all();
			foreach ($reasons as $reason) {
				if ($reason->title == $this->$attribute || $this->$attribute == "Other") {
					return true;
				}
			}
		}

		$this->addError('patientReasonForVisit', Craft::t('scheduling-module', 'The reason for visit provided is not valid.'));
		return false;
	}
}
