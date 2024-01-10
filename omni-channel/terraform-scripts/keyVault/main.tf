# Retrieving all the loganalytics data from the remote tfstate file
data "terraform_remote_state" "log_workspace" {
  count   = length(var.keyvaults)
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.keyvaults[count.index].log_analytics_workspace_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

# Create recovery vaults
module "keyvault" {
  source                          = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.keyvault?ref=dev"
  count                           = length(var.keyvaults)
  name                            = var.keyvaults[count.index].name
  resource_group_name             = var.keyvaults[count.index].rg_name
  sku                             = var.keyvaults[count.index].sku
  enabled_for_deployment          = var.keyvaults[count.index].enabled_for_deployment
  enabled_for_disk_encryption     = var.keyvaults[count.index].enabled_for_disk_encryption
  enabled_for_template_deployment = var.keyvaults[count.index].enabled_for_template_deployment
  access_policies                 = var.keyvaults[count.index].access_policies
  secrets                         = var.keyvaults[count.index].secrets
  network_acls                    = var.keyvaults[count.index].network_acls
  purge_protection_enabled        = var.keyvaults[count.index].purge_protection_enabled
  enable_rbac_authorization       = var.keyvaults[count.index].enable_rbac_authorization
  kv_diagnostic_setting_name      = var.keyvaults[count.index].kv_diagnostic_setting_name
  log_analytics_workspace_id      = data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_id[index(data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_workspace_name, var.keyvaults[count.index].log_analytics_workspace_name)]
  tags                            = var.keyvaults[count.index].tags
}
