<?php

namespace modules\DupageCoreModule\components\log;

use \Psr\Log\LogLevel as PsrLogLevel;
use Craft;
use modules\DupageCoreModule\models\PatientUser;
use Psr\Log\LoggerAwareInterface;
use Psr\Log\LoggerAwareTrait;
use Psr\Log\LoggerInterface;
use Psr\Log\LogLevel;
use samdark\log\PsrMessage;
use samdark\log\PsrTarget as Target;
use Yii;
use yii\base\InvalidConfigException;
use yii\helpers\VarDumper;
use yii\log\Logger;

final class PsrTarget extends Target
{
    /**
     * @var bool If enabled, logger use original timestamp from buffer
     * @since 1.1.0
     */
    public $addTimestampToContext = false;

    /**
     * @var bool If enabled, exception's trace will extract into `trace` property
     */
    public $extractExceptionTrace = false;

    private $_levelMap = [
        Logger::LEVEL_ERROR => LogLevel::ERROR,
        Logger::LEVEL_WARNING => LogLevel::WARNING,
        Logger::LEVEL_INFO => LogLevel::INFO,
        Logger::LEVEL_TRACE => LogLevel::DEBUG,
        Logger::LEVEL_PROFILE => LogLevel::DEBUG,
        Logger::LEVEL_PROFILE_BEGIN => LogLevel::DEBUG,
        Logger::LEVEL_PROFILE_END => LogLevel::DEBUG,

        // Psr Levels
        LogLevel::EMERGENCY => LogLevel::EMERGENCY,
        LogLevel::ALERT => LogLevel::ALERT,
        LogLevel::CRITICAL => LogLevel::CRITICAL,
        LogLevel::ERROR => LogLevel::ERROR,
        LogLevel::WARNING => LogLevel::WARNING,
        LogLevel::NOTICE => LogLevel::NOTICE,
        LogLevel::INFO => LogLevel::INFO,
        LogLevel::DEBUG => LogLevel::DEBUG,
    ];

    /**
     * @var array
     */
    private $_levels = [];
    /**
     * @inheritdoc
     */
    public function export()
    {
        foreach ($this->messages as $message) {
            $level = $message[1];

            $context = [];
            if (isset($message[4])) {
                $context['trace'] = $message[4];
            }

            if (isset($message[5])) {
                $context['memory'] = $message[5];
            }

            if (isset($message[2])) {
                $context['category'] = $message[2];
            }

            if ($this->addTimestampToContext && isset($message[3])) {
                $context['timestamp'] = $message[3];
                $context['@timestamp'] = $message[3];
            }

            $text = $message[0];

            if (!is_string($text)) {
                // exceptions may not be serializable if in the call stack somewhere is a Closure
                if ($text instanceof \Throwable || $text instanceof \Exception) {
                    if ($this->extractExceptionTrace) {
                        $context['exception'] = [
                            'trace' => $text->getTrace(),
                            'file' => $text->getFile(),
                            'line' => $text->getLine(),
                            'code' => $text->getCode(),
                            'class' => \get_class($text)
                        ];
                        
                        $text = $text->getMessage();
                    } else {
                        $text = (string)$text;
                    }
                } elseif ($text instanceof PsrMessage) {
                    $context = array_merge($text->getContext(), $context); // Will not replace standard context keys
                    $text = $text->getMessage();
                } else {
                    $text = VarDumper::export($text);
                }
            }


            // If the user_id is not passed, dynamically set it from the user identity object
            if (!isset($context['user_id'])) {
                if (Craft::$app !== null && Craft::$app->has('patient_user')) {
                    $context['user_id'] = Craft::$app->patient_user->id ?? 'anonymous';
                } else {
                    $context['user_id'] = 'anonymous';
                }
            }

            if (!isset($context['timestamp'])) {
                $context['timestamp'] = \microtime(true);
            }

            if (!isset($context['@timestamp'])) {
                $context['@timestamp'] = $context['timestamp'];
            }

            if (Craft::$app !== null && Craft::$app->has('request')) {
                if (get_class(Craft::$app->request) !== 'craft\console\Request') {
                    $context['uri'] = (function () {
                        $url = Craft::$app->request->getUrl();
                        if (\stripos($url, '?')) {
                            $parts = \explode('?', $url);
                            return $parts[0];
                        }

                        return $url;
                    })();
                }
            }

            $this->getLogger()->log($this->_levelMap[$level], $text, $context);
        }
    }
}
