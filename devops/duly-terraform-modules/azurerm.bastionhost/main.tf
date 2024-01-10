# Get data from vnet resource group
data "azurerm_resource_group" "vnet_rg" {
  name = var.vnet_rg_name
}

# Get data from vnet
data "azurerm_virtual_network" "vnet" {
  name                   = var.vnet_name
  resource_group_name    = var.vnet_rg_name
}

# Get data from AzureBastionSubnet subnet
data "azurerm_subnet" "bastion_subnet" {
  name                   = "AzureBastionSubnet"
  resource_group_name    = var.vnet_rg_name
  virtual_network_name   = var.vnet_name
}

# Create PIP for bastion host
resource "azurerm_public_ip" "bastion_pip" {
  name                   = var.bastion_pip_name
  location               = data.azurerm_resource_group.vnet_rg.location
  resource_group_name    = var.vnet_rg_name
  allocation_method      = "Static"
  sku                    = "Standard"
}
# Create PIP for bastion host
resource "azurerm_bastion_host" "bastion" {
  name                   = var.bastion_host_name
  location               = data.azurerm_resource_group.vnet_rg.location
  resource_group_name    = var.vnet_rg_name

  ip_configuration {
    name                 = "configuration"
    subnet_id            = data.azurerm_subnet.bastion_subnet.id
    public_ip_address_id = azurerm_public_ip.bastion_pip.id
  }
}
