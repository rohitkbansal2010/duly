<?php

namespace modules\physiciansmodule\controllers\tntsearch;

use PDO;
use TeamTNT\TNTSearch\TNTSearch;
use modules\physiciansmodule\controllers\tntsearch\Indexer\TNTIndexerDMG;

class TNTSearchDMG extends TNTSearch
{
    public $asYouType = true;
    public $maxDocs = PHP_INT_MAX;
    public $fuzziness = true;
    public $fuzzy_distance = 2;

    /**
     * @inheritdoc
     * 
     * Overwritten to use TNTIndexerDMG and not TNTIndexer
     */
    public function createIndex($indexName, $disableOutput = false)
    {
        $indexer = new TNTIndexerDMG;
        $indexer->loadConfig($this->config);
        $indexer->disableOutput = $disableOutput;

        if ($this->dbh) {
            $indexer->setDatabaseHandle($this->dbh);
        }
        return $indexer->createIndex($indexName, $this->dbh);
    }

    /**
     * @inheritdoc
     * 
     * Overwritten to use tntinfo and not info
     */
    public function getValueFromInfoTable($value)
    {
        $query = "SELECT * FROM tntinfo WHERE tntkey = '$value'";
        $docs  = $this->index->query($query);

        if ($ret = $docs->fetch(PDO::FETCH_ASSOC)) {
            return $ret['value'];
        }

        return null;
    }

    /**
     * @inheritdoc
     * 
     * Overwritten to add MYSQL_ATTR_USE_BUFFERED_QUERY (required by mysql)
     */
    public function selectIndex($indexName, PDO $index = null)
    {
        $this->index = $index;
        $this->index->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $this->index->setAttribute(PDO::MYSQL_ATTR_USE_BUFFERED_QUERY, true);
        $this->setStemmer();
        $this->setTokenizer();
    }

    /**
     * @inheritdoc
     * 
     * Overwritten to use TNTIndexerDMG and not TNTIndexer
     */
    public function getIndex()
    {
        $indexer = new TNTIndexerDMG;
        $indexer->inMemory = false;
        $indexer->setIndex($this->index);
        return $indexer;
    }
}
