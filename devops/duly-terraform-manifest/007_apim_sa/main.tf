# Retrieving all the loganalytics data from the remote tfstate file
data "terraform_remote_state" "log_workspace" {
  count   = length(var.storage_accounts)
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.storage_accounts[count.index].log_analytics_workspace_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

# Create storage accounts

module "storage_account" {
  source                    = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.storageaccount?ref=dev"
  count                     = length(var.storage_accounts)
  storage_name              = var.storage_accounts[count.index].storage_name
  rg_name                   = var.storage_accounts[count.index].rg_name
  account_tier              = var.storage_accounts[count.index].account_tier
  account_kind              = var.storage_accounts[count.index].account_kind
  account_replication_type  = var.storage_accounts[count.index].account_replication_type
  min_tls_version           = var.storage_accounts[count.index].min_tls_version
  access_tier               = var.storage_accounts[count.index].access_tier
  allow_blob_public_access  = var.storage_accounts[count.index].allow_blob_public_access
  large_file_share_enabled  = var.storage_accounts[count.index].large_file_share_enabled
  enable_https_traffic_only = var.storage_accounts[count.index].enable_https_traffic_only
  is_hns_enabled            = var.storage_accounts[count.index].is_hns_enabled
  #  network_rules                       = var.storage_accounts[count.index].network_rules
  storage_acc_diagnostic_setting_name = var.storage_accounts[count.index].storage_acc_diagnostic_setting_name
  log_analytics_workspace_id          = data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_id[index(data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_workspace_name, var.storage_accounts[count.index].log_analytics_workspace_name)]
}
