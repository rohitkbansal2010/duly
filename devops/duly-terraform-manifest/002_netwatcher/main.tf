module "netwatcher" {
  source              = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.netwatcher?ref=dev"
  count               = length(var.network_watchers)
  name                = var.network_watchers[count.index].name
  location            = var.network_watchers[count.index].location
  resource_group_name = var.network_watchers[count.index].rg_name
}
