<?php declare(strict_types=1);

namespace SimpleSAML\Module\epic;

use Craft;
use SimpleSAML\XHTML\TemplateControllerInterface;
use Twig\Environment;
use Twig\Extension\DebugExtension;
use Twig\TwigFilter;
use craft\test\TestSetup;
use craft\web\twig\variables\CraftVariable;
use yii\helpers\Html;

final class DupageController implements TemplateControllerInterface
{
    /**
     * Craft App instance for mocking and interacting with everything
     * @var craft\web\Application
     */
    private $app;

    /**
     * @inheritdoc
     * @param Environment $twig
     */
    public function setUpTwig(Environment &$twig)
    {
        // Mock Craft::$app
        $app = $this->getCraft();

        // Enable debug/dump for non lxd environments
        if (getenv('ENVIRONMENT') != 'lxd') {
            $twig->enableDebug();
        }

        // Add custom twig functions
        foreach ($this->getFunctions() as $function) {
            $twig->addFunction($function);
        }

        // Add custom twig filters
        foreach ($this->getFilters() as $filter) {
            $twig->addFilter($filter);
        }

        // Add custom globals
        foreach ($this->getGlobals() as $k => $global) {
            $twig->addGlobal($k, $global);
        }

        // Add custom extensions
        foreach ($this->getExtensions($twig) as $extension) {
            $twig->addExtension($extension);
        }
    }

    /**
     * @inheritdoc
     * @param array $data
     */
    public function display(&$data)
    {
    }

    /**
     * An array of Twig functions to add
     * @return array
     */
    private function getFunctions()
    {
        return [
        ];
    }

    /**
     * An array of Twig filters to add
     * @return array
     */
    private function getFilters()
    {
        return [

        ];
    }

    /**
     * An array of Twig globals to add
     * @return array
     */
    private function getGlobals()
    {
        return [
            'env' => getenv('ENVIRONMENT'),
            'html' => new Html,
            'view' => (function () {
                $view = Craft::$app->getView();
                $templateMode = $view->getTemplateMode();
                $view->setTemplateMode($view::TEMPLATE_MODE_SITE);
                $view->setTemplatesPath(APP_ROOT . '/templates/');

                return $view;
            })()
        ];
    }

    /**
     * An array of Twig extensions to add
     * @return array
     */
    private function getExtensions(&$twig)
    {
        return [
            // Enable the Twig debug extension
            new \Twig\Extension\DebugExtension,
            // Loads the Craft Twig view extensions so we can utilize Craft's built in Filters, Globals, and Functions
            // @ref: https://craftcms.com/docs/3.x/dev/filters.html
            new \craft\web\twig\Extension(
                $this->getGlobals()['view'],
                $twig
            ),
        ];
    }

    /**
     * Initialize a complete instance of Craft that's not running
     * So that we can hijack the template rendering system
     * and other essential functions to eliminate duplication
     * within the SimpleSAML PHP module
     */
    private function getCraft()
    {
        if (!defined('CRAFT_BASE_PATH')) {
            define('CRAFT_BASE_PATH', APP_ROOT);
            define('CRAFT_VENDOR_PATH', CRAFT_BASE_PATH . '/vendor');
        }

        define('CRAFT_ENVIRONMENT', getenv('ENVIRONMENT') ?: 'production');
        $app = require CRAFT_VENDOR_PATH . '/craftcms/cms/bootstrap/web.php';
        $app->init();
        $app->bootstrap();
        return $app;
    }
}