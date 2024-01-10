
resource "azurerm_postgresql_database" "db" {
  name                = var.db_name
  resource_group_name = var.resource_group_name
  server_name         = var.server_name
  charset             = var.charset
  collation           = var.collation
}