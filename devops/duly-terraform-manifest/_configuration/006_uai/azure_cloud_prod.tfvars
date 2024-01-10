uais = [
  {
    name                  = "duly-p-app-aks-id"
    rg_name               = "duly-p-app-aks-rg"
    location              = "northcentralus"
    subnet_name           = "duly-p-vnet-app-snet"
    vnet_name             = "duly-p-vnet"
    vnet_rg_name          = "duly-p-vnet-rg"
    udr_name              = "duly-p-app-udr"
    udr_rg_name           = "duly-p-app-aks-rg"
    private_dns_zone_name = "prd.privatelink.northcentralus.azmk8s.io"
    acr_state_path        = "006_ACR/azure_cloud_prod/terraform.tfstate"
  }
]