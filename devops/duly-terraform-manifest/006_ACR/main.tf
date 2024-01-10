data "azurerm_storage_account" "sa" {
  name                = var.acr[0].storage_account_name
  resource_group_name = var.acr[0].storage_account_rg
}

data "terraform_remote_state" "log_workspace" {
  count   = length(var.acr)
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.acr[count.index].log_analytics_workspace_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

resource "azurerm_container_registry" "acr" {
  name                = var.acr[0].name
  resource_group_name = var.acr[0].rg_name
  location            = var.acr[0].location
  sku                 = var.acr[0].sku
  admin_enabled       = false
}

resource "azurerm_monitor_diagnostic_setting" "acr_diag" {
  name                       = var.acr[0].diagnostic_setting
  target_resource_id         = azurerm_container_registry.acr.id
  storage_account_id         = data.azurerm_storage_account.sa.id
  log_analytics_workspace_id = data.terraform_remote_state.log_workspace[0].outputs.log_analytics_id[index(data.terraform_remote_state.log_workspace[0].outputs.log_analytics_workspace_name, var.acr[0].log_analytics_workspace_name)]

  log {
    category = "ContainerRegistryLoginEvents"
    enabled  = true

    retention_policy {
      enabled = false
    }
  }

  log {
    category = "ContainerRegistryRepositoryEvents"
    enabled  = true

    retention_policy {
      enabled = false
    }
  }

  metric {
    category = "AllMetrics"

    retention_policy {
      enabled = false
    }
  }
}
