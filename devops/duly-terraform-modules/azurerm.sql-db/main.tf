resource "azurerm_mssql_database" "sql_db" {
  name                 = var.db_name
  server_id            = var.server_id
  collation            = var.collation
  max_size_gb          = var.max_size_gb
  zone_redundant       = var.zone_redundant
  sku_name             = var.sku_name
  storage_account_type = var.storage_account_type

  # dynamic "extended_auditing_policy" {
  #   for_each = var.storage_endpoint != null && var.storage_account_access_key != null ? [1] : []
  #   content {
  #     storage_endpoint           = var.storage_endpoint
  #     storage_account_access_key = var.storage_account_access_key
  #     retention_in_days          = var.retention_in_days
  #   }
  # }
}

resource "azurerm_management_lock" "sql_db_lock" {
  count      = var.lock_level == "CanNotDelete" || var.lock_level == "ReadOnly" ? 1 : 0
  name       = "ADO pipeline lock"
  scope      = azurerm_mssql_database.sql_db.id
  lock_level = var.lock_level
}