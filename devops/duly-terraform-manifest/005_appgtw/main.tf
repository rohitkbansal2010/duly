# Retrieving all the loganalytics data from the remote tfstate file
data "terraform_remote_state" "log_workspace" {
  count   = length(var.app_gateways)
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.app_gateways[count.index].log_analytics_workspace_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

# Create application gateways
module "app_gateway" {
  source = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.appgtw?ref=dev"
  count  = length(var.app_gateways)

  name                = var.app_gateways[count.index].name
  location            = var.app_gateways[count.index].location
  resource_group_name = var.app_gateways[count.index].rg_name
  zones               = lookup(var.app_gateways[count.index], "zones", null)
  tags                = lookup(var.app_gateways[count.index], "tags", null)
  autoscaling         = lookup(var.app_gateways[count.index], "autoscaling", "no") == "yes" ? true : false
  enable_http2        = lookup(var.app_gateways[count.index], "enable_http2", false) == "enabled" ? true : false

  sku                        = var.app_gateways[count.index].sku
  waf                        = var.app_gateways[count.index].waf
  waf_disabled_rule_groups   = var.app_gateways[count.index].waf_disabled_rule_groups
  autoscale_configuration    = lookup(var.app_gateways[count.index], "autoscale_configuration", null)
  gateway_ip_configurations  = var.app_gateways[count.index].gateway_ip_configurations
  frontend_ip_configurations = var.app_gateways[count.index].frontend_ip_configurations
  app_definitions            = var.app_gateways[count.index].app_definitions
  frontend_ports             = var.app_gateways[count.index].frontend_ports
  ssl_certificates           = var.app_gateways[count.index].ssl_certificates

  log_list                       = var.app_gateways[count.index].log_list
  appgtw_diagnostic_setting_name = var.app_gateways[count.index].appgtw_diagnostic_setting_name
  log_analytics_workspace_id     = data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_id[index(data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_workspace_name, var.app_gateways[count.index].log_analytics_workspace_name)]
}

