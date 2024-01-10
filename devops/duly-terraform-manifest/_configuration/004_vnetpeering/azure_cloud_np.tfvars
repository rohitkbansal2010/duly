vnet_peerings = [
  {
    name                          = "devops-to-apps"
    vnet_state_path               = "non-prod-tier/002_vnet/terraform.tfstate"
    source_vnet_name              = "duly-d-vnetdops"
    source_vnet_rg_name           = "duly-d-vnet-rg"
    destination_vnet_name         = "duly-d-vnet"
    destination_vnet_rg_name      = "duly-d-vnet-rg"
    traffic_from_remote_vnet      = "allow"
    traffic_forwarded_from_remote = "allow"
    allow_gateway_transit         = "no"
    use_virtual_network_gateway   = "no"
  },
  {
    name                          = "apps-to-devops"
    vnet_state_path               = "non-prod-tier/002_vnet/terraform.tfstate"
    source_vnet_name              = "duly-d-vnet"
    source_vnet_rg_name           = "duly-d-vnet-rg"
    destination_vnet_name         = "duly-d-vnetdops"
    destination_vnet_rg_name      = "duly-d-vnet-rg"
    traffic_from_remote_vnet      = "allow"
    traffic_forwarded_from_remote = "allow"
    allow_gateway_transit         = "no"
    use_virtual_network_gateway   = "no"
  }
]