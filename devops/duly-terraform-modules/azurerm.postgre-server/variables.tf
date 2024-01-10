variable "server_name" {
    type = string
    description = "Specifies the name of the PostgreSQL Server. "
    default = "epampsqlserver"
}
variable "resource_group_name" {
    type = string
    description = "The name of the resource group in which to create the PostgreSQL Server."
}
variable "location" {
    type = string
    description = "Specifies the supported Azure location where the resource exists."
}
variable "server_version" {
    type = string
    description = "Specifies the version of PostgreSQL to use. Valid values are 9.5, 9.6, 10, 10.0, and 11."
    default = "9.6"
}
variable "administrator_login" {
    type = string
    description = "The Administrator Login for the PostgreSQL Server."
    default = "epamadmin"
}
variable "administrator_login_password" {
    type = string
    description = "The Password associated with the administrator_login for the PostgreSQL Server."
    default = "EP@Mp@$$w0rd12"
}
variable "create_custom_managed_key" {
    type = bool
    description = "Manages a Customer Managed Key for a PostgreSQL Server."
    default = true
}
variable "server_options" {
    type = list
    description = "Specifies the value of the PostgreSQL Configuration"
    default = []
}
variable "threat_detection_policy" {
 #   type = map
    description = "Specifies threat detection policy?"
    default = {}
}
variable "create_custom_identity" {
    type = bool
    description = "The Type of Identity which should be used for this PostgreSQL Server. At this time the only possible value is SystemAssigned."
    default = true
}
variable "storage_account_access_key" {
    type = string
    description = "Specifies the identifier key of the Threat Detection audit storage account."
}
variable "storage_endpoint" {
    type = string
    description = "Specifies the blob storage endpoint (e.g. https://MyAccount.blob.core.windows.net). This blob storage will hold all Threat Detection audit logs."
}
variable "ssl_minimal_tls_version_enforced" {
    type = string
    description = "The minimum TLS version to support on the sever. Possible values are TLSEnforcementDisabled, TLS1_0, TLS1_1, and TLS1_2"
    default = "TLS1_2"
}
variable "sku_name" {
    type = string
    description = "Specifies the SKU Name for this PostgreSQL Server. The name of the SKU, follows the tier + family + cores pattern (e.g. B_Gen4_1, GP_Gen5_8)."
    default = "GP_Gen5_2"
}
variable "storage_mb" {
    type = number
    description = "Max storage allowed for a server. Possible values are between 5120 MB(5GB) and 1048576 MB(1TB) for the Basic SKU and between 5120 MB(5GB) and 16777216 MB(16TB) for General Purpose/Memory Optimized SKUs."
    default = 5120
}
variable "auto_grow_enabled" {
    type = bool
    description = "Enable/Disable auto-growing of the storage. Storage auto-grow prevents your server from running out of storage and becoming read-only. If storage auto grow is enabled, the storage automatically grows without impacting the workload."
    default = true
}
variable "backup_retention_days" {
    type = number
    description = "Backup retention days for the server, supported values are between 7 and 35 days."
    default = 7
}
variable "geo_redundant_backup_enabled" {
    type = bool
    description = "Turn Geo-redundant server backups on/off"
    default = false
}
variable "infrastructure_encryption_enabled" {
    type = bool
    description = "Whether or not infrastructure is encrypted for this server."
    default = false
}
variable "public_network_access_enabled" {
    type = bool
    description = "Whether or not public network access is allowed for this server"
    default = true
}
variable "ssl_enforcement_enabled" {
    type = bool
    description = "Specifies if SSL should be enforced on connections."
    default = true
}
variable "vnet_name" {
    type = string
    description = "The name of the vnet where server takes place"
}
variable "vnet_rg_name" {
    type = string
    description = "The name of vnet's rg"
}
variable "subnet_name" {
    type = string
    description = "The name of the subnet where server takes place"
}
variable "firewall_rules" {
    type = list
    description = "The Ip addresses allowed through the firewall"
    default = []
}
variable "kv_name" {
    type = string
    description = "The name of the key vault where the password stored"
}
variable "kv_rg_name" {
    type = string
    description = "Key vault's resource group name"
}