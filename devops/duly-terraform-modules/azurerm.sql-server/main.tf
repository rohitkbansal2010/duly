data "azurerm_key_vault" "kv" {
  name                = var.kv_name
  resource_group_name = var.kv_rg_name
}
data "azurerm_key_vault_secret" "kv_secret" {
  name         = var.administrator_login
  key_vault_id = data.azurerm_key_vault.kv.id
}
data "azurerm_virtual_network" "vnet" {
  name                = var.vnet_name
  resource_group_name = var.vnet_rg_name
}

data "azurerm_subnet" "subnet" {
  name                 = var.subnet_name
  virtual_network_name = data.azurerm_virtual_network.vnet.name
  resource_group_name  = data.azurerm_virtual_network.vnet.resource_group_name
}

resource "azurerm_mssql_server" "sql_server" {
  name                         = var.server_name
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = var.server_version
  administrator_login          = var.administrator_login
  administrator_login_password = data.azurerm_key_vault_secret.kv_secret.value
  connection_policy            = var.connection_policy
  minimum_tls_version          = "1.2"

  dynamic "azuread_administrator" {
    for_each = var.azuread_authentication_only == true && var.azuread_administrator != null ? [1] : []
    content {
      login_username              = var.azuread_administrator_username
      azuread_authentication_only = var.azuread_authentication_only
      object_id                   = var.azuread_administrator
    }
  }

  # dynamic "azurerm_mssql_server_extended_auditing_policy" {
  #   for_each = var.storage_endpoint != null && var.storage_account_access_key != null ? [1] : []
  #   content {
  #     storage_endpoint           = var.storage_endpoint
  #     storage_account_access_key = var.storage_account_access_key
  #     retention_in_days          = var.retention_in_days
  #   }
  # }

}

resource "azurerm_mssql_virtual_network_rule" "net_rule" {
  name                = "${var.server_name}-vnet-rule"
  server_id           = azurerm_mssql_server.sql_server.id
  subnet_id           = data.azurerm_subnet.subnet.id
}

resource "azurerm_mssql_firewall_rule" "sql_server" {
  count               = length(var.firewall_rules)
  name                = var.firewall_rules[count.index].name
  server_id           = azurerm_mssql_server.sql_server.id
  start_ip_address    = var.firewall_rules[count.index].start_ip_address
  end_ip_address      = var.firewall_rules[count.index].end_ip_address
}

resource "azurerm_management_lock" "sql_server_lock" {
  count      = var.lock_level == "CanNotDelete" || var.lock_level == "ReadOnly" ? 1 : 0
  name       = "ADO pipeline lock"
  scope      = azurerm_mssql_server.sql_server.id
  lock_level = var.lock_level
}