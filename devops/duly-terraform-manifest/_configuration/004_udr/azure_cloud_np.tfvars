route_tables = [
  {
    name              = "duly-d-app-udr"
    location          = "northcentralus"
    rg_name           = "duly-d-app-aks-rg"
    route_propogation = "no"
    routes = [
      {
        name                   = "default"
        address_prefix         = "0.0.0.0/0"
        next_hop_type          = "VirtualAppliance"
        next_hop_in_ip_address = "172.26.3.4" # duly Azure firewall
      }
    ]
    subnet_associate = [
      {
        subnet_name = "duly-d-vnet-app-snet"
        vnet_name   = "duly-d-vnet"
        rg_name     = "duly-d-vnet-rg"
      }
    ]
  }
]
