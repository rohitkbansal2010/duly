locals {
  routes = [
    for table in var.route_tables : [
      for route in table.routes : {
        name                   = route.name
        address_prefix         = route.address_prefix
        next_hop_type          = route.next_hop_type
        next_hop_in_ip_address = route.next_hop_in_ip_address
      }
    ]
  ]
}

# Create route table
module "udr" {
  source                        = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.udr?ref=dev"
  count                         = length(var.route_tables)
  name                          = var.route_tables[count.index].name
  location                      = var.route_tables[count.index].location
  resource_group_name           = var.route_tables[count.index].rg_name
  routes                        = local.routes[count.index]
  subnet_associate              = var.route_tables[count.index].subnet_associate
  disable_bgp_route_propagation = var.route_tables[count.index].route_propogation == "yes" ? false : true
}
