<?php

namespace modules\DupageCoreModule\queue;

use Craft;
use craft\helpers\App;
use craft\helpers\Template;
use craft\queue\BaseJob;
use yii\base\InvalidConfigException;
use yii\helpers\Markdown;
use yii\mail\MessageInterface;

/**
 * punchkick/dupage-core-module module for Craft CMS 3.x
 *
 * Facilitates sending emails through the background job queue as opposed to controller events.
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2020 Punchkick Interactive
 *
 * Usage:
 *
 * Craft::$app->queue->push(new \modules\DupageCoreModule\queue\EmailJob([
 *    'template' => '/path/to/view/file.twig',
 *    'subject' => 'Email Subject'
 *    'to' => 'customer@example.com', // or ['customer@example.com' => 'Customer']
 *    'from' => 'sender@example.com',
 *    'templateData' => [ ], // Template data
 * ]));
*/
class EmailJob extends BaseJob
{
    /**
     * The HTML template to use
     * @var string $template
     */
    public $template;

    /**
     * The layout file to use
     * @var string $layout
     */
    public $layout = '@modules/DupageCoreModule/views/email/layouts/layout.twig';

    /**
     * The message subject
     * @var string $subject
     */
    public $subject;
    
    /**
     * Data to be passed to the template for rendiner
     * @var array $templateData
     */
    public $templateData = [];

    /**
     * The receipient
     * @var string $to
     */
    public $to;

    /**
     * Attachment file content
     * @var string $attachmentFileContent
     */
    public $attachmentFileContent;

    /**
     * Attachment file name
     * @var string $attachmentFileName
     */
    public $attachmentFileName;

    /**
     * File mime type
     * @var string $attachmentFileMimeType
     */
    public $attachmentFileMimeType;

    /**
     * The sender
     * @var string $from
     */
    public $from;

    public function execute($queue)
    {
        try {
            $mailer = Craft::$app
                ->getMailer();

            $mailer->htmlLayout = Craft::getAlias($this->layout);
            
            $message = $mailer->compose();

            if ($message->language === null) {
                // Default to the current language
                $message->language = Craft::$app->getRequest()->getIsSiteRequest()
                    ? Craft::$app->language
                    : Craft::$app->getSites()->getPrimarySite()->language;
            }

            $view = Craft::$app->getView();
            $templateMode = $view->getTemplateMode();
            $view->setTemplateMode($view::TEMPLATE_MODE_SITE);

            $message->setFrom([getEnv('SMTP_SENDFROM_ADDRESS') => getenv('SMTP_SENDFROM_NAME')]);

            $language = Craft::$app->language;
            Craft::$app->language = $message->language;

            // Render the subject and textBody
            $message->setSubject($this->subject);
            $view->setTemplatesPath(Craft::getAlias('@modules/DupageCoreModule') . '/templates/_email');
            $message->setHtmlBody($view->renderTemplate($this->template, $this->templateData));
            
            Craft::$app->language = $language;

            $message->setTo($this->to)
                ->setSubject($this->subject);
        
            $failures = null;

            if ($this->attachmentFileContent && $this->attachmentFileName && $this->attachmentFileMimeType) {
                $message->attachContent(
                    $this->attachmentFileContent,
                    [
                        'fileName' => $this->attachmentFileName,
                        'contentType' => $this->attachmentFileMimeType
                    ]
                );
            }
            
            if (!$mailer->send($message, $failures)) {
                Craft::error('Failed to send email');
                Craft::error(print_r($failures, true));
            }
        } catch (\Exception $e) {
            Craft::error($e);
            throw $e;
        }
    }

    protected function defaultDescription()
    {
        return 'Sends email notifications to customers using the specified template.';
    }
}
