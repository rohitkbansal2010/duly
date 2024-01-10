# Get resource group data
data "azurerm_resource_group" "rg" {
  name                    = var.rg_name
}

# Create public IPs
resource "azurerm_public_ip" "public_ip" {
  name                    = var.name
  location                = data.azurerm_resource_group.rg.location
  resource_group_name     = data.azurerm_resource_group.rg.name
  allocation_method       = var.allocation_method
  tags                    = var.tags
  sku                     = var.sku
  ip_version              = var.ip_version
  domain_name_label       = var.domain_name_label
  availability_zone = var.availability_zone
}