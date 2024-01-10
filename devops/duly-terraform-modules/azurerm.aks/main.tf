
data "azurerm_virtual_network" "vnet" {
  name                = var.vnet_name
  resource_group_name = var.vnet_rg_name
}

data "azurerm_subnet" "subnet" {
  name                 = var.subnet_name
  virtual_network_name = data.azurerm_virtual_network.vnet.name
  resource_group_name  = data.azurerm_virtual_network.vnet.resource_group_name
}

data "azurerm_private_dns_zone" "private_dns" {
  count               = var.private_dns_zone_name != "" ? 1 : 0
  name                = var.private_dns_zone_name
  resource_group_name = var.rg_name
}


### Creates AKS
resource "azurerm_kubernetes_cluster" "k8s" {
  name                            = var.cluster_name
  location                        = var.location
  resource_group_name             = var.rg_name
  dns_prefix                      = var.dns_prefix
  private_cluster_enabled         = var.private_cluster_enabled
  private_dns_zone_id             = var.private_dns_zone_name != "" ? data.azurerm_private_dns_zone.private_dns[0].id : "System"
  api_server_authorized_ip_ranges = var.api_server_authorized_ip_ranges
  node_resource_group             = "${var.rg_name}-node"

  default_node_pool {
    name                  = var.agentpool_name
    node_count            = var.agentpool_node_count
    vm_size               = var.agentpool_vm_size
    os_disk_size_gb       = var.agentpool_os_disk_size_gb
    availability_zones    = var.agentpool_availability_zones
    enable_node_public_ip = var.agentpool_enable_node_public_ip
    enable_auto_scaling   = var.agentpool_enable_auto_scaling
    max_count             = var.agentpool_enable_auto_scaling ? var.agentpool_max_count : null
    min_count             = var.agentpool_enable_auto_scaling ? var.agentpool_min_count : null
    vnet_subnet_id        = data.azurerm_subnet.subnet.id
    os_sku                = var.os_sku
  }

  dynamic "role_based_access_control" {
    for_each = var.role_based_access_control ? [1] : []
    content {
      enabled = var.role_based_access_control
      dynamic "azure_active_directory" {
        for_each = var.RBAC_managed == null ? [] : [1]
        content {
          managed                = var.RBAC_managed
          admin_group_object_ids = var.RBAC_managed ? var.RBAC_admin_group_object_ids : null
          client_app_id          = var.RBAC_managed ? null : var.RBAC_client_app_id
          server_app_id          = var.RBAC_managed ? null : var.RBAC_server_app_id
          server_app_secret      = var.RBAC_managed ? null : var.RBAC_server_app_secret
          tenant_id              = var.RBAC_managed ? null : var.RBAC_tenant_id
        }
      }
    }


  }

  auto_scaler_profile {
    balance_similar_node_groups      = var.balance_similar_node_groups
    max_graceful_termination_sec     = var.max_graceful_termination_sec
    scale_down_delay_after_add       = var.scale_down_delay_after_add
    scale_down_delay_after_failure   = var.scale_down_delay_after_failure
    scan_interval                    = var.scan_interval
    scale_down_unneeded              = var.scale_down_unneeded
    scale_down_unready               = var.scale_down_unready
    scale_down_utilization_threshold = var.scale_down_utilization_threshold
  }

  network_profile {
    network_plugin     = var.network_plugin
    network_policy     = var.network_policy
    pod_cidr           = var.network_plugin == "azure" ? null : var.pod_cidr
    service_cidr       = var.service_cidr
    dns_service_ip     = var.dns_service_ip
    docker_bridge_cidr = var.docker_bridge_cidr
    outbound_type      = var.outbound_type
  }

  dynamic "service_principal" {
    for_each = var.client_id != null && var.client_secret != null ? [1] : []
    content {
      client_id     = var.client_id
      client_secret = var.client_secret
    }
  }

  dynamic "identity" {
    for_each = var.client_id == null && var.client_secret == null ? [1] : []
    content {
      type                      = "UserAssigned"
      user_assigned_identity_id = var.uai_id
    }
  }


  # dynamic "identity" {
  #   for_each = var.client_id == null && var.client_secret == null ? [1] : []
  #   content {
  #     type = "SystemAssigned"
  #   }
  # }

  addon_profile {
    oms_agent {
      enabled                    = true
      log_analytics_workspace_id = var.la_workspace_id
    }
  }

  # linux_profile {

  # }
  tags = var.tags
}

### Creates additional node pool(s) according to ${agentpools} variable (type = map)
resource "azurerm_kubernetes_cluster_node_pool" "additional" {
  for_each              = var.agentpools
  name                  = each.value.name
  kubernetes_cluster_id = azurerm_kubernetes_cluster.k8s.id
  node_count            = each.value.node_count
  os_sku                = var.os_sku
  vm_size               = each.value.vm_size
  os_disk_size_gb       = each.value.os_disk_size_gb
  availability_zones    = each.value.availability_zones
  enable_node_public_ip = each.value.enable_node_public_ip
  max_pods              = each.value.max_pods
  enable_auto_scaling   = each.value.enable_auto_scaling
  max_count             = each.value.enable_auto_scaling ? each.value.max_count : null
  min_count             = each.value.enable_auto_scaling ? each.value.min_count : null
  vnet_subnet_id        = data.azurerm_subnet.subnet.id
  tags = var.tags
}

### Lock Azure resource if ${lock_level} defined as "CanNotDelete" or "ReadOnly" (Vnet included)
resource "azurerm_management_lock" "k8s" {
  count      = var.lock_level == "CanNotDelete" || var.lock_level == "ReadOnly" ? 1 : 0
  name       = "ADO pipeline lock"
  scope      = azurerm_kubernetes_cluster.k8s.id
  lock_level = var.lock_level
}
