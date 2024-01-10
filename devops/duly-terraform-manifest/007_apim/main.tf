data "azurerm_application_insights" "app_insights" {
  count               = length(var.apim)
  name                = var.apim[count.index].app_insights_name
  resource_group_name = var.apim[count.index].app_insights_rg_name
}

module "apim" {
  source                                  = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.api-management?ref=dev" # =dev # ?ref=v0.1.2"
  count                                   = length(var.apim)
  apim_name                               = var.apim[count.index].apim_name
  rg_name                                 = var.apim[count.index].rg_name
  location                                = var.apim[count.index].location
  sku_name                                = var.apim[count.index].sku_name
  vnet_name                               = var.apim[count.index].vnet_name
  vnet_rg_name                            = var.apim[count.index].vnet_rg_name
  subnet_name                             = var.apim[count.index].subnet_name
  publisher_name                          = var.apim[count.index].publisher_name
  publisher_email                         = var.apim[count.index].publisher_email
  kv_name                                 = var.apim[count.index].kv_name
  kv_rg_name                              = var.apim[count.index].kv_rg_name
  management_hostname_configuration       = var.apim[count.index].management_hostname_configuration
  developer_portal_hostname_configuration = var.apim[count.index].developer_portal_hostname_configuration
  proxy_hostname_configuration            = var.apim[count.index].proxy_hostname_configuration
  app_insights_name                       = var.apim[count.index].app_insights_name
  app_insights_rg_name                    = var.apim[count.index].app_insights_rg_name

  named_values = merge(var.apim[count.index].named_values, {
    ApplicationInsightsInstrumentationKey = data.azurerm_application_insights.app_insights[count.index].instrumentation_key
  })

  policy_configuration = var.apim[count.index].policy_configuration
}

data "azurerm_api_management" "apim" {
  count               = length(var.apim)
  name                = var.apim[count.index].apim_name
  resource_group_name = var.apim[count.index].rg_name
}

data "azurerm_redis_cache" "redis" {
  count               = length(var.apim)
  name                = var.apim[count.index].redis_cache_name
  resource_group_name = var.apim[count.index].rg_name
}

resource "azurerm_api_management_redis_cache" "apim_cache" {
  count             = length(var.apim)
  name              = "${var.apim[count.index].apim_name}-Redis-Cache"
  api_management_id = data.azurerm_api_management.apim[count.index].id
  connection_string = data.azurerm_redis_cache.redis[count.index].secondary_connection_string
  description       = "Redis cache instances"
  redis_cache_id    = data.azurerm_redis_cache.redis[count.index].id
  cache_location    = var.apim[count.index].location

  depends_on = [
    module.apim
  ]
}
 