# Deploy Resource Groups
module "rg" {
  source   = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.rg?ref=dev"
  count    = length(var.rg_list)
  name     = var.rg_list[count.index].name
  location = var.rg_list[count.index].location
  tags     = var.rg_list[count.index].tags
}
