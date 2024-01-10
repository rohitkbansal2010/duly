<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;
use craft\events\ConfigEvent;

use Symfony\Component\Yaml\Yaml;

use verbb\navigation\elements\Node;
use verbb\navigation\models\Nav as NavModel;
use verbb\navigation\Navigation;
use verbb\navigation\records\Nav;


/**
 * m200427_221631_iam_nav migration.
 */
class m200427_221631_iam_nav extends Migration
{
    private $_defaultNavigationSetup = [
        'IAMNavigation' => [
            [
                'title' => 'current patient',
                'newWindow' => false,
                'type' => null,
                'url' => '#',
                'classes' => null,
                'children' => [
                    [
                        'title' => 'to schedule an appointment',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/schedule',
                        'classes' => null
                    ],
                    [
                        'title' => 'for a primary care physician',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/physicians?service[]=Primary+Care&visittype=new-problem',
                        'classes' => null
                    ],
                    [
                        'title' => 'for a specialist',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/physicians?visittype=new-problem',
                        'classes' => null
                    ],
                    [
                        'title' => 'for urgent care',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/immediate-care',
                        'classes' => null
                    ],
                    [
                        'title' => 'for insurance / health plans',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/insurance-carriers',
                        'classes' => null
                    ],
                    [
                        'title' => 'to request a prescription refill',
                        'newWindow' => true,
                        'type' => null,
                        'url' => 'https://mychart.dupagemedicalgroup.com/mychart/',
                        'classes' => null
                    ],
                    [
                        'title' => 'to ask my doctor a question',
                        'newWindow' => true,
                        'type' => null,
                        'url' => 'https://mychart.dupagemedicalgroup.com/mychart/',
                        'classes' => null
                    ],
                    [
                        'title' => 'to pay my bill',
                        'newWindow' => true,
                        'type' => null,
                        'url' => 'https://mychart.dupagemedicalgroup.com/mychart/',
                        'classes' => null
                    ],
                ],
            ],
            [
                'title' => 'new patient',
                'newWindow' => false,
                'type' => null,
                'url' => '#',
                'classes' => null,
                'children' => [
                    [
                        'title' => 'to schedule an appointment',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/schedule',
                        'classes' => null
                    ],
                    [
                        'title' => 'for a primary care physician',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/physicians?service[]=Primary+Care&visittype=new-patient',
                        'classes' => null
                    ],
                    [
                        'title' => 'for a pediatrician near me',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/physicians?service[]=Pediatrics&visittype=new-patient',
                        'classes' => null
                    ],
                    [
                        'title' => 'for a specialist',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/physicians?visittype=new-patient',
                        'classes' => null
                    ],
                    [
                        'title' => 'for urgent care',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/immediate-care',
                        'classes' => null
                    ],
                    [
                        'title' => 'for insurance / health plans',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/insurance-carriers',
                        'classes' => null
                    ],
                ]
            ]
        ],
    ];

    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $projectConfig = Yaml::parseFile(__DIR__ . '/../config/project.yaml');
        $navigationConfig = $projectConfig['navigation']['navs'];

        foreach ($this->_defaultNavigationSetup as $navHandle => $elements) {
            $nav = Nav::find()
                ->where(['handle' => $navHandle])
                ->one();

            // This plugin isn't correctly adding data from the project config to the database.
            // If it doesn't, we need to manually add everything.
            if ($nav === null) {
                // Find the value from the project config
                foreach ($navigationConfig as $uid => $navElement) {
                    if ($navElement['handle'] === $navHandle) {
                        // Create a new ConfigEvent that aligns to what Craft _should_ create then initiate it
                        $e = new ConfigEvent;
                        $e->newValue = $navElement;
                        $e->tokenMatches[0] = $uid;

                        Navigation::$plugin->navs->handleChangedNav($e);
                        $nav = Nav::find()
                            ->where(['handle' => $navHandle])
                            ->one();
                        if ($nav === null) {
                            throw new \Exception('The project config was not appropriately applied.');
                        }
                        break;
                    }
                }
            }

            foreach ($elements as $n) {
                $node = new Node;

                $children = [];
                if (isset($n['children'])) {
                    $children = $n['children'];
                    unset($n['children']);
                }

                foreach ($n as $k => $v) {
                    $node->$k = $v;
                }
                $node->navId = $nav->id;
                $node->enabled = true;
                $node->enabledForSite = true;

                if (Craft::$app->getElements()->saveElement($node, true, true) !== true) {
                    return false;
                }

                foreach ($children as $n2) {
                    $childNode = new Node;
                    foreach ($n2 as $k => $v) {
                        $childNode->$k = $v;
                    }
                    $childNode->navId = $nav->id;
                    $childNode->enabled = true;
                    $childNode->enabledForSite = true;
                    $childNode->newParentId = $node->id;

                    if (Craft::$app->getElements()->saveElement($childNode, true, true) !== true) {
                        return false;
                    }
                }
            }
        }
    }

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m200427_221631_iam_nav cannot be reverted.\n";
        return false;
    }
}
