<?php

namespace modules\schedulingmodule\services\Exceptions;

use Exception;

class InvalidEndDateException extends Exception
{
    protected $message = 'Must not be before the start date. Must be in the ISO (yyyy-MM-dd) format.';
}
