module "appInsight" {
  source      = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.appInsight?ref=dev"
  count       = length(var.ais)
  ai_name     = var.ais[count.index].ai_name
  ai_location = var.ais[count.index].ai_location
  ai_rg_name  = var.ais[count.index].ai_rg_name
}

data "azurerm_application_insights" "ai" {
  count               = length(var.ais)
  name                = var.ais[count.index].ai_name
  resource_group_name = var.ais[count.index].ai_rg_name
  depends_on = [
    module.appInsight
  ]
}

data "azurerm_key_vault" "kv" {
  for_each            = var.ais[0].kv
  name                = each.key
  resource_group_name = each.value
}

resource "azurerm_key_vault_secret" "instrumentationKey" {
  for_each     = var.ais[0].kv
  name         = var.ais[0].kv_secret_name
  value        = data.azurerm_application_insights.ai[0].instrumentation_key
  key_vault_id = data.azurerm_key_vault.kv[each.key].id

  depends_on = [
    module.appInsight
  ]
}