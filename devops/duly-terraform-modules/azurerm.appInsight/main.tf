resource "azurerm_application_insights" "ai" {
  name                = var.ai_name
  location            = var.ai_location
  resource_group_name = var.ai_rg_name
  application_type    = "web"
}

output "instrumentation_key" {
  value = azurerm_application_insights.ai.instrumentation_key
}
