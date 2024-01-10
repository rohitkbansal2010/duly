# Retrieving group data from AAD
data "azuread_group" "main" {
  count        = length(local.group_names)
  display_name = local.group_names[count.index]
}

# Retrieving user data from AAD
data "azuread_user" "main" {
  count               = length(local.user_principal_names)
  user_principal_name = local.user_principal_names[count.index]
}

# Retrieving service_principal data from AAD
data "azuread_service_principal" "main" {
  count        = length(local.application_names)
  display_name = local.application_names[count.index]
}

# Retrieving resuorce group data
data "azurerm_resource_group" "main" {
  name = var.resource_group_name
}

# Retrieving client config data on runtime
data "azurerm_client_config" "main" {}

# Retrieving subnet data
data "azurerm_subnet" "main" {
  count                = length(var.network_acls.subnet_associations)
  name                 = var.network_acls.subnet_associations[count.index].subnet_name
  virtual_network_name = var.network_acls.subnet_associations[count.index].vnet_name
  resource_group_name  = var.network_acls.subnet_associations[count.index].rg_name
}

# Manages a diagnostic setting for created key vault
resource "azurerm_monitor_diagnostic_setting" "main" {
  name                       = var.kv_diagnostic_setting_name
  target_resource_id         = azurerm_key_vault.main.id
  log_analytics_workspace_id = var.log_analytics_workspace_id

  log {
    category = "AuditEvent"
    enabled  = true

    retention_policy {
      days = 0
      enabled = true
    }
  }

  log {
    category = "AzurePolicyEvaluationDetails"
    enabled  = false

    retention_policy {
      days = 0
      enabled = false
    }
  }

  metric {
    category = "AllMetrics"
    enabled  = false

    retention_policy {
      enabled = false
    }
  }
}

# Creating key vault
resource "azurerm_key_vault" "main" {
  name                            = var.name
  location                        = data.azurerm_resource_group.main.location
  resource_group_name             = data.azurerm_resource_group.main.name
  tenant_id                       = data.azurerm_client_config.main.tenant_id
  enabled_for_deployment          = var.enabled_for_deployment
  enabled_for_disk_encryption     = var.enabled_for_disk_encryption
  enabled_for_template_deployment = var.enabled_for_template_deployment
  sku_name                        = var.sku
  enable_rbac_authorization       = var.enable_rbac_authorization
  purge_protection_enabled        = var.purge_protection_enabled

# Creating access policies to key vault resources for each user, group, application
  dynamic "access_policy" {
    for_each = local.combined_access_policies

    content {
      tenant_id = data.azurerm_client_config.main.tenant_id
      object_id = access_policy.value.object_id

      certificate_permissions = access_policy.value.certificate_permissions
      key_permissions         = access_policy.value.key_permissions
      secret_permissions      = access_policy.value.secret_permissions
      storage_permissions     = access_policy.value.storage_permissions
    }
  }

# Creating network access control rules for key vault
  dynamic "network_acls" {
    for_each = var.network_acls == {} ? [] : [var.network_acls]
    iterator = acl
    content {
      bypass                     = coalesce(acl.value.bypass, "None")
      default_action             = acl.value.default_action
      ip_rules                   = acl.value.ip_rules
      virtual_network_subnet_ids = flatten(data.azurerm_subnet.main.*.id)
    }
  }

  tags = var.tags
}

# Creating secret
resource "azurerm_key_vault_secret" "main" {
  for_each     = var.secrets
  name         = each.key
  value        = each.value
  key_vault_id = azurerm_key_vault.main.id
  
  lifecycle {
    ignore_changes = all
  } 
}
