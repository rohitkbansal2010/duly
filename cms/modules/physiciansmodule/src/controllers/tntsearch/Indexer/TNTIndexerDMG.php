<?php

namespace modules\physiciansmodule\controllers\tntsearch\Indexer;

use PDO;
use TeamTNT\TNTSearch\Indexer\TNTIndexer;

/**
 * TNTIndexerDMG extends the vendor TNTIndexer class.
 * This version modifies the extends the original class to support storing the indexes in a proper MySQL database,
 * whereas the original class forces saving indexes on the filesystem as SQLite files.
 */
class TNTIndexerDMG extends TNTIndexer
{
    /**
     * Creates an index. It uses the proovided PDO connection to a MySQL database to create the index inside the chosen MySQL database.
     *
     * Two main diffferences between this version of createIndex and the vendor's (sqlite) version:
     * 1. tntinfo table was renamed from info to tntinfo; info table is a Craft-specific table, so it can't be used by TNT
     * 2. key column inside the tntinfo table is a valid column name in sqlite, but is a reserved keyword in mysql; key was renamed to tntkey
     */
    public function createIndex($indexName, PDO $connection = null)
    {
        $this->indexName = $indexName;

        $this->index = $connection;

        $this->index->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        // required attribute for MySQL connections
        $this->index->setAttribute(PDO::MYSQL_ATTR_USE_BUFFERED_QUERY, true);

        $this->index->exec("DROP TABLE IF EXISTS wordlist;");
        $this->index->exec("DROP TABLE IF EXISTS doclist;");
        $this->index->exec("DROP TABLE IF EXISTS hitlist;");
        $this->index->exec("DROP TABLE IF EXISTS tntinfo;");

        // create indexes and tables as needed
        $this->index->exec("CREATE TABLE IF NOT EXISTS wordlist (
                    id INTEGER PRIMARY KEY NOT NULL AUTO_INCREMENT,
                    term VARCHAR(512) UNIQUE,
                    num_hits INTEGER,
                    num_docs INTEGER)");

        $this->index->exec("CREATE UNIQUE INDEX mainindex ON wordlist (term);");

        $this->index->exec("CREATE TABLE IF NOT EXISTS doclist (
                    term_id INTEGER,
                    doc_id INTEGER,
                    hit_count INTEGER)");

        $this->index->exec("CREATE TABLE IF NOT EXISTS fields (
                    id INTEGER PRIMARY KEY,
                    name TEXT)");

        $this->index->exec("CREATE TABLE IF NOT EXISTS hitlist (
                    term_id INTEGER,
                    doc_id INTEGER,
                    field_id INTEGER,
                    position INTEGER,
                    hit_count INTEGER)");

        $this->index->exec("CREATE TABLE IF NOT EXISTS tntinfo (
                    tntkey TEXT,
                    value INTEGER)");

        $this->index->exec("INSERT INTO tntinfo ( tntkey, value) values ( 'total_documents', 0)");

        $this->index->exec("CREATE INDEX IF NOT EXISTS mainterm_id_index ON doclist (term_id);");
        $this->index->exec("CREATE INDEX IF NOT EXISTS maindoc_id_index ON doclist (doc_id);");

        if (!$this->dbh) {
            $connector = $this->createConnector($this->config);
            $this->dbh = $connector->connect($this->config);
        }
        return $this;
    }

    /**
     * @inheritdoc
     */
    public function updateInfoTable($key, $value)
    {
        $this->index->exec("UPDATE tntinfo SET value = $value WHERE tntkey = '$key'");
    }

    /**
     * @inheritdoc
     */
    public function totalDocumentsInCollection()
    {
        $query = "SELECT * FROM tntinfo WHERE tntkey = 'total_documents'";
        $docs  = $this->index->query($query);

        return $docs->fetch(PDO::FETCH_ASSOC)['value'];
    }

    /**
     * @inheritdoc
     */
    public function setStemmer($stemmer)
    {
        $this->stemmer = $stemmer;
        $class = get_class($stemmer);
        $this->index->exec("INSERT INTO tntinfo ( tntkey, value) values ( 'stemmer', '$class')");
    }
}
