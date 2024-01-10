data "terraform_remote_state" "log_workspace" {
  count   = length(var.aks)
  backend = "azurerm"
  config = {
    container_name       = var.backend_container_name
    key                  = var.aks[count.index].log_analytics_workspace_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

data "terraform_remote_state" "acr" {
  count   = length(var.aks)
  backend = "azurerm"
  config = {
    container_name       = var.prod_backend_container_name
    key                  = var.aks[count.index].acr_state_path
    resource_group_name  = var.prod_backend_resource_group_name
    storage_account_name = var.prod_backend_storage_account_name
    subscription_id      = var.prod_backend_subscription_id
    client_secret        = var.prod_backend_client_secret
    client_id            = var.prod_backend_client_id
    tenant_id            = var.prod_backend_tenant_id
  }
}

data "azurerm_user_assigned_identity" "uai" {
  count               = length(var.aks)
  name                = var.aks[count.index].uai_name
  resource_group_name = var.aks[count.index].rg_name
}

module "aks" {
  source                  = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.aks?ref=dev" # =dev # ?ref=v0.1.2
  count                   = length(var.aks)
  location                = var.aks[count.index].location
  rg_name                 = var.aks[count.index].rg_name
  cluster_name            = var.aks[count.index].cluster_name
  dns_prefix              = var.aks[count.index].dns_prefix
  la_workspace_id         = data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_id[index(data.terraform_remote_state.log_workspace[count.index].outputs.log_analytics_workspace_name, var.aks[count.index].log_analytics_workspace_name)]
  private_cluster_enabled = var.aks[count.index].private_cluster_enabled
  uai_id                  = data.azurerm_user_assigned_identity.uai[count.index].id
  outbound_type           = var.aks[count.index].outbound_type
  private_dns_zone_name   = var.aks[count.index].private_dns_zone_name

  vnet_name    = var.aks[count.index].vnet_name
  vnet_rg_name = var.aks[count.index].vnet_rg_name
  # kube_dashboard                   = var.aks_kube_dashboard

  ### Authorized ip ranges for AKS
  #   api_server_authorized_ip_ranges = var.aks[count.index].api_server_authorized_ip_ranges

  ### Default node pool
  agentpool_name            = var.aks[count.index].agentpool_name
  agentpool_node_count      = var.aks[count.index].agentpool_node_count
  agentpool_vm_size         = var.aks[count.index].agentpool_vm_size
  agentpool_os_disk_size_gb = var.aks[count.index].agentpool_os_disk_size_gb
  #   agentpool_availability_zones    = var.aks[count.index].agentpool_availability_zones
  #   agentpool_enable_node_public_ip = var.aks[count.index].agentpool_enable_node_public_ip
  # agentpool_max_pods               = var.aks[count.index].    agentpool_max_pods
  #   agentpool_enable_auto_scaling = var.aks[count.index].agentpool_enable_auto_scaling
  #   agentpool_max_count           = var.aks[count.index].agentpool_max_count
  # agentpool_min_count           = var.aks[count.index].agentpool_min_count
  # agentpool_vnet_subnet_id         = var.aks[count.index].    agentpool_vnet_subnet_id
  subnet_name = var.aks[count.index].subnet_name


  ### Additional node pools
  agentpools = var.aks[count.index].agentpools

  ### Role base access control
  #   role_based_access_control   = var.aks[count.index].role_based_access_control
  #   RBAC_managed                = var.aks[count.index].RBAC_managed
  #   RBAC_admin_group_object_ids = var.aks[count.index].RBAC_admin_group_object_ids
  #   RBAC_client_app_id          = var.aks[count.index].RBAC_client_app_id
  #   RBAC_server_app_id          = var.aks[count.index].RBAC_server_app_id
  #   RBAC_server_app_secret      = var.aks[count.index].RBAC_server_app_secret
  #   RBAC_tenant_id              = var.aks[count.index].RBAC_tenant_id

  ### Auto scaler profile
  #   balance_similar_node_groups  = var.aks[count.index].balance_similar_node_groups
  #   max_graceful_termination_sec = var.aks[count.index].max_graceful_termination_sec
  #   scale_down_delay_after_add   = var.aks[count.index].scale_down_delay_after_add
  #   # scale_down_delay_after_delete    = var.scale_down_delay_after_delete
  #   scale_down_delay_after_failure   = var.aks[count.index].scale_down_delay_after_failure
  #   scan_interval                    = var.aks[count.index].scan_interval
  #   scale_down_unneeded              = var.aks[count.index].scale_down_unneeded
  #   scale_down_unready               = var.aks[count.index].scale_down_unready
  #   scale_down_utilization_threshold = var.aks[count.index].scale_down_utilization_threshold

  #   ### Network configuration
  #   network_plugin     = var.aks[count.index].network_plugin
  #   network_policy     = var.aks[count.index].network_policy
  #   pod_cidr           = var.aks[count.index].pod_cidr
  #   service_cidr       = var.aks[count.index].service_cidr
  #   dns_service_ip     = var.aks[count.index].dns_service_ip
  #   docker_bridge_cidr = var.aks[count.index].docker_bridge_cidr
  # vnet_address_space               = var.vnet_address_space

  # lock_level = var.aks[count.index].lock_level
  tags = var.aks[count.index].tags
}

data "azurerm_kubernetes_cluster" "aks" {
  count               = length(var.aks)
  name                = var.aks[count.index].cluster_name
  resource_group_name = var.aks[count.index].rg_name

  depends_on = [
    module.aks
  ]
}

resource "azurerm_role_assignment" "aks-acr" {
  count                = length(var.aks)
  scope                = data.terraform_remote_state.acr[0].outputs["acr_id"]
  role_definition_name = "AcrPull"
  principal_id         = data.azurerm_kubernetes_cluster.aks[count.index].kubelet_identity[0].object_id

  depends_on = [
    module.aks
  ]
}

# enabling aad_pod_identity
data "azurerm_resource_group" "aks_rg" {
  count = length(var.aks)
  name  = var.aks[count.index].rg_name
  depends_on = [
    module.aks
  ]
}

data "azurerm_resource_group" "aks_node_rg" {
  count = length(var.aks)
  name  = "${var.aks[count.index].rg_name}-node"
  depends_on = [
    module.aks
  ]
}

resource "azurerm_role_assignment" "kubelet_virtual_machine_contributor" {
  count                = length(var.aks)
  scope                = data.azurerm_resource_group.aks_node_rg[count.index].id
  role_definition_name = "Virtual Machine Contributor"
  principal_id         = data.azurerm_kubernetes_cluster.aks[count.index].kubelet_identity[0].object_id
  depends_on = [
    module.aks
  ]
}

resource "azurerm_role_assignment" "kubelet_managed_identity_operator" {
  count                = length(var.aks)
  scope                = data.azurerm_resource_group.aks_rg[count.index].id
  role_definition_name = "Managed Identity Operator"
  principal_id         = data.azurerm_kubernetes_cluster.aks[count.index].kubelet_identity[0].object_id
  depends_on = [
    module.aks
  ]
}