<?php

namespace craft\contentmigrations;

use Craft;
use craft\db\Migration;

use verbb\navigation\elements\Node;
use verbb\navigation\records\Nav;

/**
 * m210510_205605_need_care_navigation migration.
 */
class m210510_205605_need_care_navigation extends Migration
{
    private $_defaultNavigationSetup = [
        'needCareNavigation' => [
            [
                'title' => 'Need Care?',
                'newWindow' => false,
                'type' => null,
                'url' => '/',
                'classes' => 'navigation-icons navigation-icon-care',
                'children' => [
                    [
                        'title' => 'Check Symptoms | Answer a few questions to get the best care options for you.',
                        'newWindow' => true,
                        'type' => null,
                        'url' => 'https://dupage-staging.clearstep.health/',
                        'classes' => null
                    ],
                    [
                        'title' => 'Schedule An Appointment | Schedule an appointment with a physician now.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/schedule',
                        'classes' => null
                    ]
                ]
            ]
        ],
    ];
    /**
     * @inheritdoc
     */
    public function safeUp()
    {
        $nav = Nav::find()
            ->where(['handle' => 'needCareNavigation'])
            ->one();

        // This plugin isn't correctly adding data from the project config to the database.
        // If it doesn't, we need to manually add everything

        $elements = $this->_defaultNavigationSetup['needCareNavigation'];
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

    /**
     * @inheritdoc
     */
    public function safeDown()
    {
        echo "m210510_205605_need_care_navigation cannot be reverted.\n";
        return false;
    }
}
