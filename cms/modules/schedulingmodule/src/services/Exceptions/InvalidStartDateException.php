<?php

namespace modules\schedulingmodule\services\Exceptions;

use Exception;

class InvalidStartDateException extends Exception
{
    protected $message = 'Must be at least today and in the ISO (yyyy-MM-dd) format.';
}
