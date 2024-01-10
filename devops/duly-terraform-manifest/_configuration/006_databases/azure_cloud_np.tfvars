dbs = [
  # following NPD resources has been created manually

  # {
  #   server_name         = "duly-d-db"
  #   resource_group_name = "duly-d-data-rg"
  #   db_name             = "duly-d-collaboration-view-db"
  #   sku_name            = "S0"
  #   zone_redundant      = false
  #   lock_level          = "ReadOnly"
  # },
  # {
  #   server_name         = "duly-d-db"
  #   resource_group_name = "duly-d-data-rg"
  #   db_name             = "duly-q-collaboration-view-db"
  #   sku_name            = "S0"
  #   zone_redundant      = false
  #   lock_level          = "ReadOnly"
  # },
  # {
  #   server_name         = "duly-d-db"
  #   resource_group_name = "duly-d-data-rg"
  #   db_name             = "duly-a-collaboration-view-db"
  #   sku_name            = "S0"
  #   zone_redundant      = false
  #   lock_level          = "ReadOnly"
  # },
  # {
  #   server_name         = "duly-d-db"
  #   resource_group_name = "duly-d-data-rg"
  #   db_name             = "duly-u-collaboration-view-db"
  #   sku_name            = "S0"
  #   zone_redundant      = false
  #   lock_level          = "ReadOnly"
  # },
  # {
  #   server_name         = "duly-d-db"
  #   resource_group_name = "duly-d-data-rg"
  #   db_name             = "duly-d-communication-hub-db"
  #   sku_name            = "S0"
  #   zone_redundant      = false
  #   lock_level          = "ReadOnly"
  # },
  {
    server_name         = "duly-d-db"
    resource_group_name = "duly-d-data-rg"
    db_name             = "duly-q-communication-hub-db"
    sku_name            = "S0"
    zone_redundant      = false
    lock_level          = "ReadOnly"
  },
  {
    server_name         = "duly-d-db"
    resource_group_name = "duly-d-data-rg"
    db_name             = "duly-a-communication-hub-db"
    sku_name            = "S0"
    zone_redundant      = false
    lock_level          = "ReadOnly"
  },
  {
    server_name         = "duly-d-db"
    resource_group_name = "duly-d-data-rg"
    db_name             = "duly-u-communication-hub-db"
    sku_name            = "S0"
    zone_redundant      = false
    lock_level          = "ReadOnly"
  }
]