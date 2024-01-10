data "azurerm_client_config" "current" {}

data "azurerm_key_vault" "kv" {
  name                         = var.kv_name
  resource_group_name          = var.kv_rg_name
}
data "azurerm_key_vault_secret" "kv_secret" {
  name                         = var.administrator_login
  key_vault_id                 = data.azurerm_key_vault.kv.id
}
data "azurerm_virtual_network" "vnet" {
    name                       = var.vnet_name
    resource_group_name        = var.vnet_rg_name
}

data "azurerm_subnet" "subnet" {
  name                              = var.subnet_name
  virtual_network_name              = data.azurerm_virtual_network.vnet.name
  resource_group_name               = data.azurerm_virtual_network.vnet.resource_group_name
}

resource "azurerm_postgresql_server" "server" {
  name                              = var.server_name
  resource_group_name               = var.resource_group_name
  location                          = var.location
  version                           = var.server_version
  administrator_login               = try(var.administrator_login, "sqladmin")
  administrator_login_password      = data.azurerm_key_vault_secret.kv_secret.value == null ? "EP@Mp@$$w0rd12" : data.azurerm_key_vault_secret.kv_secret.value
  sku_name                          = var.sku_name
  storage_mb                        = var.storage_mb
  auto_grow_enabled                 = var.auto_grow_enabled
  backup_retention_days             = var.backup_retention_days
  geo_redundant_backup_enabled      = var.geo_redundant_backup_enabled
  infrastructure_encryption_enabled = var.infrastructure_encryption_enabled
  public_network_access_enabled     = var.public_network_access_enabled
  ssl_enforcement_enabled           = var.ssl_enforcement_enabled
  ssl_minimal_tls_version_enforced  = var.ssl_minimal_tls_version_enforced
  

  dynamic "identity" {
    for_each = var.create_custom_identity == true ? [1] : [0]
    content {
      type = "SystemAssigned"
    }
  }

  dynamic "threat_detection_policy" {
    for_each = var.threat_detection_policy == {} ? [] : [var.threat_detection_policy]
    iterator = policy
    content {
      enabled                      = policy.value.enable_threat_detection_policy
      disabled_alerts              = try(policy.value.disabled_alerts, [])
      email_account_admins         = try(policy.value.email_account_admins, false)
      email_addresses              = try(policy.value.email_addresses, [])
      retention_days               = try(policy.value.log_retention_days, 0)
      storage_account_access_key   = var.storage_account_access_key
      storage_endpoint             = var.storage_endpoint 
    }
  }
}

resource "azurerm_postgresql_configuration" "options" {
  count                            = length(var.server_options)
  name                             = var.server_options[count.index].name
  resource_group_name              = var.resource_group_name
  server_name                      = azurerm_postgresql_server.server.name
  value                            = var.server_options[count.index].value
}

resource "azurerm_postgresql_virtual_network_rule" "net_rule" {
  name                             = "${var.server_name}-vnet-rule"
  resource_group_name              = var.resource_group_name
  server_name                      = azurerm_postgresql_server.server.name
  subnet_id                        = data.azurerm_subnet.subnet.id
}

resource "azurerm_postgresql_firewall_rule" "rule" {
  count                            =  length(var.firewall_rules)
  name                             = var.firewall_rules[count.index].name
  resource_group_name              = var.resource_group_name
  server_name                      = azurerm_postgresql_server.server.name
  start_ip_address                 = var.firewall_rules[count.index].start_ip_address
  end_ip_address                   = var.firewall_rules[count.index].end_ip_address
}

resource "azurerm_key_vault_access_policy" "server" {
  count                            = var.create_custom_managed_key == true ? 1 : 0
  key_vault_id                     = data.azurerm_key_vault.kv.id
  tenant_id                        = data.azurerm_client_config.current.tenant_id
  object_id                        = azurerm_postgresql_server.server.identity[count.index].principal_id
  key_permissions                  = ["get", "unwrapkey", "wrapkey", "delete", "purge"]
  secret_permissions               = ["get"]
}

resource "azurerm_key_vault_key" "srv_key" {
  count                            = var.create_custom_managed_key == true ? 1 : 0
  depends_on                       = [ azurerm_key_vault_access_policy.server ]
  name                             = "${var.server_name}-tfex-key"
  key_vault_id                     = data.azurerm_key_vault.kv.id
  key_type                         = "RSA"
  key_size                         = 2048
  key_opts                         = ["decrypt", "encrypt", "sign", "unwrapKey", "verify", "wrapKey"]
}

resource "azurerm_postgresql_server_key" "key" {
  count                            = var.create_custom_managed_key == true ? 1 : 0
  server_id                        = azurerm_postgresql_server.server.id
  key_vault_key_id                 = azurerm_key_vault_key.srv_key[count.index].id
}
