# todo: 
#   - per environment namespace
#   - nginx ingress controller (internal) pre-installation
#   - AAD Pod Identity controller pre-installation
#   - service principal for ADO
aks = [
  {
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    acr_state_path                     = "006_ACR/azure_cloud_prod/terraform.tfstate"
    location                           = "northcentralus"
    rg_name                            = "duly-d-app-aks-rg"
    cluster_name                       = "duly-d-app-aks"
    container_registry_name            = "dulydigitalcr"
    container_registry_rg              = "duly-p-sharedsvc-rg"
    dns_prefix                         = "duly-apps-aks"
    agentpool_node_count               = "3"
    agentpool_name                     = "appdefault"     # forces cluster replacement
    agentpool_vm_size                  = "Standard_D2_v3" # forces cluster replacement
    agentpool_os_disk_size_gb          = "128"            # forces cluster replacement
    subnet_name                        = "duly-d-vnet-app-snet"
    vnet_name                          = "duly-d-vnet"
    vnet_rg_name                       = "duly-d-vnet-rg"
    private_cluster_enabled            = "true"                                 # changing this without destroying causes cluster failure
    outbound_type                      = "userDefinedRouting"                   # forces cluster replacement
    private_dns_zone_name              = "privatelink.northcentralus.azmk8s.io" #"privatelink.northcentralus.azmk8s.io" # one private zone per cluster is accepted privatelink.<region>.azmk8s.io or <subzone>.privatelink.<region>.azmk8s.io
    uai_name                           = "duly-d-app-aks-id"
    # lock_level                         = "CanNotDelete" #"ReadOnly" # disabled in layer config
    agentpools = {
      "dulyapps01" = {
        name                  = "dulyapps01"
        node_count            = 0
        vm_size               = "Standard_D2_v2"
        os_disk_size_gb       = "128"
        availability_zones    = null
        enable_node_public_ip = false
        max_pods              = 110
        enable_auto_scaling   = false
      }
    }
    tags = {
      environment         = "Dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "12/27/2021"
    }

  },
  {
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    acr_state_path                     = "006_ACR/azure_cloud_prod/terraform.tfstate"
    location                           = "northcentralus"
    rg_name                            = "duly-d-devops-aks-rg"
    cluster_name                       = "duly-devops-aks"
    container_registry_name            = "dulydigitalcr"
    container_registry_rg              = "duly-d-sharedsvc-rg"
    dns_prefix                         = "duly-devops-aks"
    agentpool_node_count               = "3"
    agentpool_name                     = "dulydevops01" # forces cluster replacement
    agentpool_vm_size                  = "Standard_D3_v2"
    agentpool_os_disk_size_gb          = "256"
    subnet_name                        = "duly-d-vnetdops-cicd-snet"
    vnet_name                          = "duly-d-vnetdops"
    vnet_rg_name                       = "duly-d-vnet-rg"
    private_cluster_enabled            = "true"                                            # changing this without destroying causes cluster failure
    outbound_type                      = "loadBalancer"                                    # forces cluster replacement
    private_dns_zone_name              = "dulydevops.privatelink.northcentralus.azmk8s.io" # one private zone per cluster is accepted
    uai_name                           = "duly-d-devops-aks-id"
    # lock_level                         = "ReadOnly" # disabled in layer config
    agentpools = {
      "dulydevops02" = {
        name                  = "dulydevops02"
        node_count            = 0
        vm_size               = "Standard_D2_v2"
        os_disk_size_gb       = "128"
        availability_zones    = null
        enable_node_public_ip = false
        max_pods              = 110
        enable_auto_scaling   = false
      }
    }
    tags = {
      environment         = "Dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "01/12/2022"
    }
  }
]
