<?php

require_once __DIR__ . '/../vendor/autoload.php';

use Symfony\Component\Yaml\Yaml;

$dir = __DIR__ . '/project';

// Construct the iterator
$it = new RecursiveDirectoryIterator($dir);

// Loop through files
foreach(new RecursiveIteratorIterator($it) as $file) {
    if ($file->isFile() && $file->getExtension() == 'yaml') {
        $yaml = Yaml::parseFile($file);
        $yaml = Yaml::dump($yaml);
        \file_put_contents($file->getPathname(), $yaml);
    }
}