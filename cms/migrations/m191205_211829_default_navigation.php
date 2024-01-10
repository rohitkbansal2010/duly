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
 * m191205_211829_default_navigation migration.
 */
class m191205_211829_default_navigation extends Migration
{
    private $_defaultNavigationSetup = [
        'secondaryNavigation' => [
            [
                'title' => 'Login to MyChart',
                'newWindow' => true,
                'type' => null,
                'url' => 'https://mychart.dupagemedicalgroup.com/mychart/',
                'classes' => null
            ],
            [
                'title' => 'Pay My Bill',
                'newWindow' => true,
                'type' => null,
                'url' => 'https://mychart.dupagemd.net/mychart/billing/guestpay',
                'classes' => null
            ],
            [
                'title' => 'Immediate Care',
                'newWindow' => false,
                'type' => null,
                'url' => '/immediate-care',
                'classes' => null
            ],
        ],
        'primaryNavigation' => [
            [
                'title' => 'Find A Physician',
                'newWindow' => false,
                'type' => null,
                'url' => '/physicians',
                'classes' => 'navigation-icons navigation-icon-physician'
            ],
            [
                'title' => 'Services',
                'newWindow' => false,
                'type' => null,
                'url' => '/services',
                'classes' => 'navigation-icons navigation-icon-services'
            ],
            [
                'title' => 'Resources',
                'newWindow' => false,
                'type' => null,
                'url' => '/physicians',
                'classes' => 'navigation-icons navigation-icon-resources',
                'children' => [
                    [
                        'title' => 'Patient Resources | What to expect, patient forms, pre- and post-op checklists, and more.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/patient-resources',
                        'classes' => null
                    ],
                    [
                        'title' => 'Insurance | See if your insurance is accepted.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/insurance-carriers',
                        'classes' => null
                    ],
                    [
                        'title' => 'Request Medical Records | Request your medical resources online.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/request-medical-records',
                        'classes' => null
                    ],
                    [
                        'title' => 'Patient Survey | Share your DMG experience with us.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/patient-experience',
                        'classes' => null
                    ],
                    [
                        'title' => 'Health Topics | Expert health care insights & quick reads.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/health-topics',
                        'classes' => null
                    ],
                    [
                        'title' => 'Classes & Events | Educational classes & events lead by experts.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/events',
                        'classes' => null
                    ],
                    [
                        'title' => 'Clinical Research | Help benefit future patients.',
                        'newWindow' => false,
                        'type' => null,
                        'url' => '/clinical-research',
                        'classes' => null
                    ],
                ]
            ],
            [
                'title' => 'Immediate Care',
                'newWindow' => false,
                'type' => null,
                'url' => '/immediate-care',
                'classes' => 'navigation-icons navigation-icon-immediate-care'
            ]
        ],
        'footerNavigation' => [
            [
                'title' => 'About DMG',
                'newWindow' => false,
                'type' => null,
                'url' => '/about-dmg',
                'classes' => null
            ],
            [
                'title' => 'Locations',
                'newWindow' => false,
                'type' => null,
                'url' => '/locations',
                'classes' => null
            ],
            [
                'title' => 'FAQs',
                'newWindow' => false,
                'type' => null,
                'url' => '/faq',
                'classes' => null
            ],
            [
                'title' => 'Careers',
                'newWindow' => false,
                'type' => null,
                'url' => '/employment-opportunities',
                'classes' => null
            ],
            [
                'title' => 'Media Center',
                'newWindow' => false,
                'type' => null,
                'url' => '/media-center',
                'classes' => null
            ],
        ],
        'socialLinks' => [
            [
                'title' => 'Twitter',
                'newWindow' => true,
                'type' => null,
                'url' => 'https://www.twitter.com/dupagemedgroup',
                'classes' => 'footer-social-icons footer-social-icons-twitter'
            ],
            [
                'title' => 'Instagram',
                'newWindow' => true,
                'type' => null,
                'url' => 'https://www.instagram.com/dupagemedgroup/',
                'classes' => 'footer-social-icons footer-social-icons-instagram'
            ],
            [
                'title' => 'Facebook',
                'newWindow' => true,
                'type' => null,
                'url' => 'https://www.facebook.com/dupagemedicalgroup',
                'classes' => 'footer-social-icons footer-social-icons-facebook'
            ],
            [
                'title' => 'YouTube',
                'newWindow' => true,
                'type' => null,
                'url' => 'https://www.youtube.com/user/DuPageMedGroup?feature=mhee',
                'classes' => 'footer-social-icons footer-social-icons-youtube'
            ]
        ],
        'footerLinks' => [
            [
                'title' => 'Terms of Use',
                'newWindow' => false,
                'type' => null,
                'url' => '/terms-of-use',
                'classes' => null
            ],
            [
                'title' => 'Privacy Policy',
                'newWindow' => false,
                'type' => null,
                'url' => '/privacy-policy',
                'classes' => null
            ],
            [
                'title' => 'Billing & Financial Policy',
                'newWindow' => false,
                'type' => null,
                'url' => '/billing',
                'classes' => null
            ],
            [
                'title' => 'ACO',
                'newWindow' => false,
                'type' => null,
                'url' => '/aco',
                'classes' => null
            ],
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
        Craft::$app->getDb()->createCommand()
            ->delete('{{%navigation_nodes}}')
            ->execute();
    }
}
