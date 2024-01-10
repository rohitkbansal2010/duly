module "logAnalytics" {
  source            = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.loganalytics?ref=dev"
  count             = length(var.logAnalytics)
  name              = var.logAnalytics[count.index].name
  rg_name           = var.logAnalytics[count.index].rg_name
  pricing_tier      = var.logAnalytics[count.index].pricing_tier
  retention_in_days = var.logAnalytics[count.index].retention_in_days
  # solutions         = var.logAnalytics[count.index].solutions
}
