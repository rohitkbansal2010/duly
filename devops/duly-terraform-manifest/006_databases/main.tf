data "azurerm_mssql_server" "server" {
  count               = length(var.dbs)
  name                = var.dbs[count.index].server_name
  resource_group_name = var.dbs[count.index].resource_group_name
}

module "sql-db" {
  source         = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.sql-db?ref=dev"
  count          = length(var.dbs)
  db_name        = var.dbs[count.index].db_name
  server_id      = data.azurerm_mssql_server.server[count.index].id
  sku_name       = var.dbs[count.index].sku_name
  zone_redundant = var.dbs[count.index].zone_redundant
}