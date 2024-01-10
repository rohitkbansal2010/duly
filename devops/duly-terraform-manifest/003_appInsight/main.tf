module "appInsight" {
  source      = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.appInsight?ref=dev"
  count       = length(var.ais)
  ai_name     = var.ais[count.index].ai_name
  ai_location = var.ais[count.index].ai_location
  ai_rg_name  = var.ais[count.index].ai_rg_name
}