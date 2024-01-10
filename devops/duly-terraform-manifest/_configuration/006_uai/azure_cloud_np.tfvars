uais = [
  {
    name                  = "duly-d-app-aks-id"
    rg_name               = "duly-d-app-aks-rg"
    location              = "northcentralus"
    subnet_name           = "duly-d-vnet-app-snet"
    vnet_name             = "duly-d-vnet"
    vnet_rg_name          = "duly-d-vnet-rg"
    private_dns_zone_name = "privatelink.northcentralus.azmk8s.io"
    udr_name              = "duly-d-app-udr"
    udr_rg_name           = "duly-d-app-aks-rg"
    acr_state_path        = "006_ACR/azure_cloud_prod/terraform.tfstate"
  },
  {
    name                  = "duly-d-devops-aks-id"
    rg_name               = "duly-d-devops-aks-rg"
    location              = "northcentralus"
    subnet_name           = "duly-d-vnetdops-cicd-snet"
    vnet_name             = "duly-d-vnetdops"
    vnet_rg_name          = "duly-d-vnet-rg"
    private_dns_zone_name = "dulydevops.privatelink.northcentralus.azmk8s.io"
    udr_name              = "duly-d-app-udr"
    udr_rg_name           = "duly-d-app-aks-rg"
    acr_state_path        = "006_ACR/azure_cloud_prod/terraform.tfstate"
  },
]