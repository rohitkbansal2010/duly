zones = [
  {
    rg_name               = "duly-d-app-aks-rg"
    private_dns_zone_name = "privatelink.northcentralus.azmk8s.io"
    network_link = [
      {
        vnet_name = "duly-d-vnet"
        rg_name   = "duly-d-vnet-rg"
      },
      {
        vnet_name = "duly-d-vnetdops"
        rg_name   = "duly-d-vnet-rg"
      }
    ]
  },
  {
    rg_name               = "duly-d-devops-aks-rg"
    private_dns_zone_name = "dulydevops.privatelink.northcentralus.azmk8s.io"
    network_link = [
      {
        vnet_name = "duly-d-vnetdops"
        rg_name   = "duly-d-vnet-rg"
      }
    ]
  }
]