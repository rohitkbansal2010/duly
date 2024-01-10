### General variable
variable "location" {}
variable "rg_name" {}
variable "cluster_name" {}
variable "dns_prefix" {}
variable "la_workspace_id" {}
variable "client_id" {
  default = null
}
variable "client_secret" {
  default = null
}

variable "uai_id" {
  default = null
}

variable "api_server_authorized_ip_ranges" {
  type    = list(any)
  default = null
}
variable "subnet_name" {
  type = string
}
variable "private_cluster_enabled" {
  type    = bool
  default = false
}

variable "private_dns_zone_name" {
  default = ""
}

variable "outbound_type" {
  default = "loadBalancer"
}

variable "lock_level" {
  default = ""
}

### Default node pool variables
variable "os_sku" {
  default = "Ubuntu"
}
variable "agentpool_vm_size" {}
variable "agentpool_os_disk_size_gb" {}
variable "agentpool_name" {
  default = "default"
}
variable "agentpool_enable_auto_scaling" {
  type    = bool
  default = false
}
variable "agentpool_max_count" {
  default = 1
}
variable "agentpool_min_count" {
  default = 1
}
variable "agentpool_node_count" {
  default = 1
}
variable "agentpool_availability_zones" {
  default = null
}
variable "agentpool_enable_node_public_ip" {
  type    = bool
  default = false
}
variable "agentpool_subnet_address_prefixes" {
  default = "10.240.0.0/16"
}
variable "vnet_name" {}
variable "vnet_rg_name" {}


### Additional node pool variables
variable "agentpools" {
  type    = map(any)
  default = {}
}

### Auto_scaler_profile variables
variable "balance_similar_node_groups" {
  default = false
}
variable "max_graceful_termination_sec" {
  default = "600"
}
variable "scale_down_delay_after_add" {
  default = "10m"
}
variable "scale_down_delay_after_failure" {
  default = "3m"
}
variable "scan_interval" {
  default = "10s"
}
variable "scale_down_unneeded" {
  default = "10m"
}
variable "scale_down_unready" {
  default = "20m"
}
variable "scale_down_utilization_threshold" {
  default = "0.5"
}

### Role base access control (RBAC) variables
variable "role_based_access_control" {
  default = false
}
variable "RBAC_managed" {
  default = null
}
variable "RBAC_tenant_id" {
  default = null
}
variable "RBAC_server_app_secret" {
  default = null
}
variable "RBAC_server_app_id" {
  default = null
}
variable "RBAC_client_app_id" {
  default = null
}
variable "RBAC_admin_group_object_ids" {
  type    = list(any)
  default = null
}

### Network configuration variables
variable "network_plugin" {
  default = "kubenet"
}
variable "network_policy" {
  default = null
}
variable "pod_cidr" {
  default = null
}
variable "service_cidr" {
  default = null
}
variable "dns_service_ip" {
  default = null
}
variable "docker_bridge_cidr" {
  default = null
}

variable "tags" {
  default = {}
}
