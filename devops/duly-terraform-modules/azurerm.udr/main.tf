# Create route table
resource "azurerm_route_table" "route_table" {
  name                          = var.name
  location                      = var.location
  resource_group_name           = var.resource_group_name
  disable_bgp_route_propagation = var.disable_bgp_route_propagation
  tags                          = var.tags
}

# Get subnet to associate with route table
data "azurerm_subnet" "subnet_associate" {
  count                     = length(var.subnet_associate)

  name                      = var.subnet_associate[count.index].subnet_name
  virtual_network_name      = var.subnet_associate[count.index].vnet_name
  resource_group_name       = lookup(var.subnet_associate[count.index], "rg_name", var.resource_group_name)
}

# Associate subnet with route table
# Route table will be associated with subnet only after all routes are created, it significant for subnets such as AzureBastion.
resource "azurerm_subnet_route_table_association" "subnet" {
  count                     = length(var.subnet_associate)
  depends_on                = [ azurerm_route.route ]

  subnet_id                 = data.azurerm_subnet.subnet_associate[count.index].id
  route_table_id            = azurerm_route_table.route_table.id
}

# Create routes
resource "azurerm_route" "route" {
  count                  = length(var.routes)

  name                   = lookup(var.routes[count.index], "name")
  address_prefix         = lookup(var.routes[count.index], "address_prefix")
  next_hop_type          = lookup(var.routes[count.index], "next_hop_type")
  next_hop_in_ip_address = lookup(var.routes[count.index], "next_hop_in_ip_address", null)
  resource_group_name    = var.resource_group_name
  route_table_name       = azurerm_route_table.route_table.name
}