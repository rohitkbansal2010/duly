<?php
/**
 * Site URL Rules
 *
 * You can define custom site URL rules here, which Craft will check in addition
 * to any routes you’ve defined in Settings → Routes.
 *
 * See http://www.yiiframework.com/doc-2.0/guide-runtime-routing.html for more
 * info about URL rules.
 *
 * In addition to Yii’s supported syntaxes, Craft supports a shortcut syntax for
 * defining template routes:
 *
 *     'blog/archive/<year:\d{4}>' => ['template' => 'blog/_archive'],
 *
 * That example would match URIs such as `/blog/archive/2012`, and pass the
 * request along to the `blog/_archive` template, providing it a `year` variable
 * set to the value `2012`.
 */

return [
    'offline' => ['template' => '_errors/offline'],
    'error' => ['template' => '_errors/error'],
    'event/<slug:{slug}>/<location:\d+>/<date:\d{4}-\d{2}-\d{2}>' => ['template' => '_events/events.twig'],
    'event/<slug:{slug}>/<location:\d+>' => ['template' => '_events/events.twig'],
    'event/<slug:{slug}>' => ['template' => '_events/events.twig'],
    'location/<slug:{slug}>/<suite:{slug}>' => ['template' => '_locations/single/_single_location.twig'],
    'services' => ['template' => '_services/_list.twig']
];
