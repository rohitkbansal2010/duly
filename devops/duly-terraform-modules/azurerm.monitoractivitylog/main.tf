data "azurerm_subscription" "subsciption" {}

# Manages a diagnostic setting for created storage account
resource "azurerm_monitor_diagnostic_setting" "activitylog" {
  name                       = var.diagnostic_setting_name
  target_resource_id         = data.azurerm_subscription.subsciption.id
  log_analytics_workspace_id = var.log_analytics_workspace_id

  dynamic "log" {
    for_each = {for log in var.log_list: "log-${log}" => log} 
    content {
      category = log.value
      enabled  = true

      retention_policy {
        enabled = true
      }
    }
  }
}