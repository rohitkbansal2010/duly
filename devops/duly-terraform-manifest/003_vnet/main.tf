data "terraform_remote_state" "log_workspace" {
  count   = length(var.vnets)
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.vnets[count.index].log_analytics_workspace_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

# Create virtual network
module "vnet" {
  source                                                = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.vnet?ref=dev"
  count                                                 = length(var.vnets)
  vnet_name                                             = var.vnets[count.index].vnet_name
  rg_name                                               = var.vnets[count.index].rg_name
  address_space                                         = var.vnets[count.index].address_space
  dns_servers                                           = lookup(var.vnets[count.index], "dns_servers", [])
  subnet_prefixes                                       = var.vnets[count.index].subnet_prefixes
  subnet_names                                          = var.vnets[count.index].subnet_names
  subnet_service_endpoints                              = var.vnets[count.index].subnet_service_endpoints
  subnet_enforce_private_link_endpoint_network_policies = var.vnets[count.index].subnet_enforce_private_link_endpoint_network_policies
  vnet_diagnostic_setting_name                          = var.vnets[count.index].vnet_diagnostic_setting_name
  log_analytics_workspace_id                            = data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_id[index(data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_workspace_name, var.vnets[count.index].log_analytics_workspace_name)]
}
