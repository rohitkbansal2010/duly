aks = [
  {
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name       = "duly-p-shsvc-loganalytics"
    acr_state_path                     = "006_ACR/azure_cloud_prod/terraform.tfstate"
    location                           = "northcentralus"
    rg_name                            = "duly-p-app-aks-rg"
    cluster_name                       = "duly-p-app-aks"
    container_registry_name            = "dulydigitalcr"
    container_registry_rg              = "duly-p-sharedsvc-rg"
    dns_prefix                         = "duly-apps-aks"
    agentpool_node_count               = "1"
    agentpool_name                     = "appdefault"
    agentpool_vm_size                  = "Standard_D2_v3"
    agentpool_os_disk_size_gb          = "50"
    subnet_name                        = "duly-p-vnet-app-snet"
    vnet_name                          = "duly-p-vnet"
    vnet_rg_name                       = "duly-p-vnet-rg"
    private_cluster_enabled            = "true"
    outbound_type                      = "userDefinedRouting"
    private_dns_zone_name              = "prd.privatelink.northcentralus.azmk8s.io"
    uai_name                           = "duly-p-app-aks-id"
    agentpools = {
      "dulyapps01" = {
        name                  = "dulyapps01"
        node_count            = 1
        vm_size               = "Standard_D2_v3"
        os_disk_size_gb       = "50"
        availability_zones    = null
        enable_node_public_ip = false
        max_pods              = 110
        enable_auto_scaling   = true
        min_count             = 0
        max_count             = 4
      }
    }
    tags = {
      environment = "PRD"
    }

  }
]