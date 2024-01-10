data "azurerm_resource_group" "rg" {
  name = var.rg_name
}

# Create a Log Analytics (formally Operational Insights) Workspace
resource "azurerm_log_analytics_workspace" "laworkspace" {
  name                = var.name
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = var.rg_name
  sku                 = var.pricing_tier
  retention_in_days   = var.retention_in_days
  tags                = var.tags
}

# It seems that Solutions deployment should be reviewver as it is deprecated approach. MS proposes using Insights for services.
# resource "azurerm_log_analytics_solution" "lasolution" {
#   for_each              = toset(var.solutions)
#   solution_name         = each.value
#   location              = azurerm_log_analytics_workspace.laworkspace.location
#   resource_group_name   = azurerm_log_analytics_workspace.laworkspace.resource_group_name
#   workspace_resource_id = azurerm_log_analytics_workspace.laworkspace.id
#   workspace_name        = azurerm_log_analytics_workspace.laworkspace.name

#   plan {
#     publisher = "Microsoft"
#     product   = "OMSGallery/ContainerInsights"
#   }
# }
