<?php

namespace modules\apimodule\forms;

use yii\base\Model;

/**
 * Schedule Appointment Form
 *
 */
final class ScheduleAppointmentForm extends Model
{
    /**
     * @var int $patient_id
     */
    public $patient_id;

    /**
     * @var string $patient_id_type
     */
    public $patient_id_type;
    
    /**
     * @var int $department_id
     */
    public $department_id;

    /**
     * @var string $department_id_type
     */
    public $department_id_type;

    /**
     * @var int $visit_type_id
     */
    public $visit_type_id;

    /**
     * @var string $visit_type_id_type
     */
    public $visit_type_id_type;

    /**
     * @var string $appointment_time;
     */
    public $appointment_time;

    /**
     * @var int $provider_id
     */
    public $provider_id;

    /**
     * @var string $provider_id_type
     */
    public $provider_id_type;

    /**
     * @var int $insurance_member_id
     */
    public $insurance_member_id;

    /**
     * @var int $insurance_group_id
     */
    public $insurance_group_id;

    /**
     * @var string $insurance_provider_name
     */
    public $insurance_provider_name;

    /**
     * @var string $comments
     */
    public $comments;

    /**
     * @inheritdoc
     */ 
    public function load($data, $formName = null)
    {
        if (!parent::load($data, $formName)) {
            return false;
        }

        return true;
    }

    /**
     * @inheritdoc
     */
    public function rules()
    {
		return [
			[['patient_id', 'department_id', 'visit_type_id', 'provider_id', 'insurance_member_id', 'insurance_group_id'], 'number'],
            [['patient_id_type', 'department_id_type','visit_type_id_type', 'provider_id_type', 'insurance_provider_name', 'comments'], 'string'],
			['appointment_time', 'date', 'format' => 'php:Y-m-d\TH:i:sO'],
            [['patient_id', 'patient_id_type', 'department_id', 'department_id_type' ,'visit_type_id', 'visit_type_id_type', 'appointment_time', 'provider_id', 'provider_id_type'], 'required']
		];
    }
}
