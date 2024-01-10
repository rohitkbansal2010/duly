# Retrieving all the loganalytics data from the remote tfstate file
data "terraform_remote_state" "nsg_flow_log_workspace" {
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.nsg_flow_log_parameters["nsg_flow_log_workspace_state_path"]
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

locals {
  # Retrieving index of nsg_flow_log_workspace_name from outputs to get corresponding log_analytics_workspace_id and  log_analytics_workspace_resource_id                                           
  index = index(data.terraform_remote_state.nsg_flow_log_workspace.outputs.log_analytics_workspace_name, var.nsg_flow_log_parameters["nsg_flow_log_workspace_name"])
}

# Creating storage account for Nsg log analytics
module "nsg_flow_storage_account" {
  source                              = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.storageaccount?ref=dev"
  storage_name                        = var.nsg_flow_log_parameters["nsg_flow_sa_name"]
  rg_name                             = var.nsg_flow_log_parameters["nsg_flow_sa_rg_name"]
  storage_acc_diagnostic_setting_name = var.nsg_flow_log_parameters["storage_acc_diagnostic_setting_name"]
  log_analytics_workspace_id          = data.terraform_remote_state.nsg_flow_log_workspace.outputs.log_analytics_id[local.index]
  network_rules                       = lookup(var.nsg_flow_log_parameters, "network_rules", {})
}

# Creating Nsg
module "nsg" {
  source                              = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.nsg?ref=dev"
  depends_on                          = [module.nsg_flow_storage_account]
  for_each                            = var.nsg
  inbound_rules                       = each.value["inbound_rules"]
  outbound_rules                      = each.value["outbound_rules"]
  resource_group_name                 = each.value["rg_name"]
  nsg_name                            = each.key
  location                            = each.value["location"]
  subnet_associate                    = each.value["subnet_associate"]
  nsg_flow_sa_id                      = module.nsg_flow_storage_account.storage_account_id
  log_analytics_workspace_id          = data.terraform_remote_state.nsg_flow_log_workspace.outputs.log_analytics_workspace_id[local.index]
  log_analytics_workspace_resource_id = data.terraform_remote_state.nsg_flow_log_workspace.outputs.log_analytics_id[local.index]
  network_watcher_name                = var.nsg_flow_log_parameters["network_watcher_name"]
  nsg_diagnostic_setting_name         = each.value["nsg_diagnostic_setting_name"]
}