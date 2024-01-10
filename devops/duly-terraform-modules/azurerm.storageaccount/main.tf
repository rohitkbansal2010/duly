# Get resource group data
data "azurerm_resource_group" "storage" {
  name = var.rg_name
}

# Retrieving subnet data
data "azurerm_subnet" "storage" {
  count                = var.network_rules == {} ? 0 : length(var.network_rules.subnet_associations)
  name                 = var.network_rules.subnet_associations[count.index].subnet_name
  virtual_network_name = var.network_rules.subnet_associations[count.index].vnet_name
  resource_group_name  = var.network_rules.subnet_associations[count.index].rg_name
}

# Create storage account
resource "azurerm_storage_account" "storage" {
  name                      = var.storage_name
  resource_group_name       = data.azurerm_resource_group.storage.name
  location                  = data.azurerm_resource_group.storage.location
  account_tier              = var.account_tier              # "Standard"
  account_kind              = var.account_kind              # "StorageV2"
  account_replication_type  = var.account_replication_type  # "LRS"
  min_tls_version           = var.min_tls_version           # "TLS1_2"
  access_tier               = var.access_tier               # "Hot"
  allow_blob_public_access  = var.allow_blob_public_access  # true
  large_file_share_enabled  = var.large_file_share_enabled  # false
  enable_https_traffic_only = var.enable_https_traffic_only # true
  is_hns_enabled            = var.is_hns_enabled            # false
  tags                      = var.tags
  
  blob_properties {
    delete_retention_policy {
      days = 7
    }
  }

  # Creating network access control rules for storage account
  dynamic "network_rules" {
    for_each = var.network_rules == {} ? [] : [var.network_rules]
    iterator = network_rule
    content {
      bypass                     = [coalesce(network_rule.value.bypass, "None")]
      default_action             = network_rule.value.default_action
      ip_rules                   = network_rule.value.ip_rules
      virtual_network_subnet_ids = flatten(data.azurerm_subnet.storage.*.id)
    }
  }
}

# Manages a diagnostic setting for created storage account
resource "azurerm_monitor_diagnostic_setting" "storage" {
  name                       = var.storage_acc_diagnostic_setting_name
  target_resource_id         = azurerm_storage_account.storage.id
  log_analytics_workspace_id = var.log_analytics_workspace_id

  metric {
    category = "Transaction"
    enabled  = true

    retention_policy {
      enabled = true
    }
  }
  
  depends_on = [
    azurerm_storage_account.storage
  ]
}
