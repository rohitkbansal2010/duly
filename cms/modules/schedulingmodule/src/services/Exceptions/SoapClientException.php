<?php

namespace modules\schedulingmodule\services\Exceptions;

use \yii\base\Exception;

class SoapClientException extends Exception
{
    protected $message = 'An error occured when making a SOAP call to a backend Epic service.';
}
