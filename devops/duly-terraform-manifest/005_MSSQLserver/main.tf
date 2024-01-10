module "sql-server" {
  source                         = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.sql-server?ref=dev"
  count                          = length(var.servers)
  server_name                    = var.servers[count.index].server_name
  resource_group_name            = var.servers[count.index].resource_group_name
  location                       = var.servers[count.index].location
  server_version                 = var.servers[count.index].server_version
  administrator_login            = var.servers[count.index].administrator_login
  azuread_administrator          = var.servers[count.index].azuread_administrator
  azuread_administrator_username = var.servers[count.index].azuread_administrator_username
  azuread_authentication_only    = var.servers[count.index].azuread_authentication_only
  vnet_name                      = var.servers[count.index].vnet_name
  vnet_rg_name                   = var.servers[count.index].vnet_rg_name
  subnet_name                    = var.servers[count.index].subnet_name
  firewall_rules                 = var.servers[count.index].firewall_rules
  kv_name                        = var.servers[count.index].kv_name
  kv_rg_name                     = var.servers[count.index].kv_rg_name
}

data "azurerm_mssql_server" "server" {
  count               = length(var.servers)
  name                = var.servers[count.index].server_name
  resource_group_name = var.servers[count.index].resource_group_name
}

data "azurerm_subnet" "subnet" {
  count                = length(var.servers)
  name                 = var.servers[count.index].subnet_name
  virtual_network_name = var.servers[count.index].vnet_name
  resource_group_name  = var.servers[count.index].vnet_rg_name
}

resource "azurerm_private_endpoint" "pe" {
  count               = length(var.servers)
  name                = "${var.servers[count.index].server_name}-pe"
  location            = var.servers[count.index].location
  resource_group_name = var.servers[count.index].resource_group_name
  subnet_id           = data.azurerm_subnet.subnet[count.index].id

  private_service_connection {
    name                           = "${var.servers[count.index].server_name}-pl"
    private_connection_resource_id = data.azurerm_mssql_server.server[count.index].id
    is_manual_connection           = false
    subresource_names = [
      "sqlServer"
    ]
  }
}