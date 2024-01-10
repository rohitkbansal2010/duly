<?php

namespace modules\schedulingmodule\forms;

use yii\base\Model;
use \DateTime;
use craft\elements\Entry;

final class ShareEmailForm extends Model
{
    /**
     * @var string $share_details
     */
    public $share_details;

    /**
     * @var string $physician
     */
    public $physician;

    /**
     * @var string $time
     */
    public $time;

    /**
     * @var string $location
     */
    public $location;

    /**
     * @var string $send_to
     */
    public $send_to;

    /**
     * @var string $sender_name
     */
    public $sender_name;

    /**
     * @var string $sender_email
     */
    public $sender_email;

    /**
     * @var string $visit_type
     */
    public $visit_type;

    /**
     * @inheritdoc
     */
    public function rules()
    {
        return [
            [['physician', 'time', 'location', 'send_to', 'sender_name', 'share_details', 'sender_email', 'visit_type'], 'string'],
            [['physician', 'time', 'location', 'send_to', 'sender_name', 'share_details', 'sender_email', 'visit_type'], 'safe']
        ];
    }
}
