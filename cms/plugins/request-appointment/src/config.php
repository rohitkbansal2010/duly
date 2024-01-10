<?php

use gftp\FtpComponent;
use gftp\FtpProtocol;

/**
 * request-appointment plugin for Craft CMS 3.x
 *
 * A request appointment mode plugin for Craft 3 developed by Punchkick Interactive
 *
 * @link      https://www.punchkick.com
 * @copyright Copyright (c) 2019 Punchkick Interactive
 */

/**
 * request-appointment config.php
 *
 * This file exists only as a template for the request-appointment settings.
 * It does nothing on its own.
 *
 * Don't edit this file, instead copy it to 'craft/config' as 'request-appointment.php'
 * and make your changes there to override default settings.
 *
 * Once copied to 'craft/config', this file will be multi-environment aware as
 * well, so you can have different settings groups for each environment, just as
 * you do for 'general.php'
 */

return [	
    'components'=>[
        'ftp' => [
            'class' => FtpComponent::class,
            'driverOptions' => [
                'class' => FtpProtocol::valueOf('sftp')->driver,
                'user' => getenv('SFTP_USER'),
                'pass' => getenv('SFTP_PASSWORD'),
                'host' => getenv('SFTP_HOST')
            ]
        ]
    ],
];
