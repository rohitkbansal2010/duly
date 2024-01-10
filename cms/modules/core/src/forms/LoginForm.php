<?php
/**
 * DupageCoreModule module for Craft CMS 3.x
 *
 * Allows for extended management of the scheduling section of the app.
 *
 * @link      https://digital.wipfli.com/
 * @copyright Copyright (c) 2020 Wipfli Digital
 */

namespace modules\DupageCoreModule\forms;

use Craft;
use modules\DupageCoreModule\models\PatientUser;
use modules\schedulingmodule\SchedulingModule;
use OneLogin\Saml2\Response as SamlResponse;
use yii\base\Model;
use samdark\log\PsrMessage;

/**
 * @author    Wipfli Digital
 * @package   DupageCoreModule
 * @since     1.0.0
 */
class LoginForm extends Model
{
    /**
     * @var string $username
     */
    public $username;

    /**
     * @var $password
     */
    public $password;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['username', 'password'], 'required'],
            [['username', 'password'], 'string'],
            [['username', 'password'], 'safe']
        ];
    }

    public function attributes()
    {
        return [
            'username' => Craft::t('dupage-core-module', 'Username'),
            'password' => Craft::t('dupage-core-module', 'Password')
        ];
    }

    /**
     * Authenticates a user against MyChart and returns true if the Craft session was setup correctly
     * @return true
     */
    public function login()
    {
        if ($this->validate()) {
            try {
                $result = SchedulingModule::getInstance()
                ->schedulingModuleEpicService
                ->authenticate($this->username, $this->password);
            } catch (\Exception $e) {
                Craft::error(new PsrMessage('error authenticating user', [
                    'error' => $e->getMessage()
                ]), get_class($this) . '::' . __METHOD__);        
                $this->addError('password', Craft::t('dupage-core-module', 'We were unable to log you in using the supplied credentials'));
                return false;
            }
            
            if ($result !== null) {
                $user = new PatientUser(['scenario' => PatientUser::SCENARIO_DEFAULT]);
                $user->load(['PatientUser' => [
                    'id' => $result['epi'],
                    'name' => $result['name'],
                    'anonymous' => false,
                ]]);

                if (Craft::$app->patient_user->identity) {
                    // carry over any data needed from the old (anonymous) user to the new (logged-in) user
                    $user->setAppointmentServiceIds(Craft::$app->patient_user->identity->getAppointmentServiceIds());
                    $user->is_video_visit_flow = Craft::$app->patient_user->identity->is_video_visit_flow;
                    $user->appointment_time = Craft::$app->patient_user->identity->appointment_time;
                    $user->appointment_department_id = Craft::$app->patient_user->identity->appointment_department_id;
                    $user->appointment_visit_type_id = Craft::$app->patient_user->identity->appointment_visit_type_id;
                    $user->appointment_physician_id = Craft::$app->patient_user->identity->appointment_physician_id;
                    $user->appointment_selected_recommended_physician_id = Craft::$app->patient_user->identity->appointment_selected_recommended_physician_id;
                }
                
                $user->scenario = PatientUser::SCENARIO_DEFAULT;
                if ($user->save()) {
                    try {
                        $user->getPatients();
                    } catch (\Exception $e) {
                        Craft::error(new PsrMessage('error retrieving user patient data', [
                            'error' => $e->getMessage()
                        ]), get_class($this) . '::' . __METHOD__);
                        $this->addError('password', Craft::t('dupage-core-module', 'We were unable to log you in using the supplied credentials'));
                        return false;
                    }
                    return Craft::$app->patient_user->login($user, (60 * 20));
                }
            }
        }

        $this->addError('password', Craft::t('dupage-core-module', 'We were unable to log you in using the supplied credentials'));
        return false;
    }

    /**
     * Logs the user in using a SAML2 response
     *
     * @param SamlResponse $response
     * @return bool
     */
    public static function loginWithSamlIdentity(SamlResponse $response)
    {
        if ($response->isValid()) {
            $attributes = $response->getAttributes();

            $user = new PatientUser(['scenario' => PatientUser::SCENARIO_DEFAULT]);
            $user->load(['PatientUser' => [
                'id' => $attributes['urn:oid:0.9.2342.19200300.100.1.1'][0],
                'name' => $attributes['urn:oid:2.16.840.1.113730.3.1.241'][0],
                'anonymous' => false,
            ]]);

            if (Craft::$app->patient_user->identity) {
                $user->setAppointmentServiceIds(Craft::$app->patient_user->identity->getAppointmentServiceIds());
            }
            
            $user->scenario = PatientUser::SCENARIO_DEFAULT;
            if ($user->save()) {
                $user->getPatients(); // This is super slow
                
                return Craft::$app->patient_user->login($user, (60 * 20));
            }
        }

        return false;
    }
}
