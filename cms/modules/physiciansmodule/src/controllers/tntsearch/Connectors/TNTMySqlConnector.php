<?php

namespace modules\physiciansmodule\controllers\tntsearch\Connectors;

use PDO;
use Craft;
use TeamTNT\TNTSearch\Connectors\MySqlConnector;
use TeamTNT\TNTSearch\Connectors\ConnectorInterface;

class TNTMySqlConnector extends MySqlConnector implements ConnectorInterface
{
    /**
     * @inheritdoc
     */
    public function createConnection($dsn, array $config, array $options)
    {
        return Craft::$app->db->pdo;
    }
}