dbs = [
  {
    server_name         = "duly-p-sql"
    resource_group_name = "duly-p-data-rg"
    db_name             = "duly-p-collaboration-view-db"
    sku_name            = "S0"
    zone_redundant      = false
    lock_level          = "ReadOnly"
  },
  {
    server_name         = "duly-p-sql"
    resource_group_name = "duly-p-data-rg"
    db_name             = "duly-p-communication-hub-db"
    sku_name            = "S0"
    zone_redundant      = false
    lock_level          = "ReadOnly"
  }
]