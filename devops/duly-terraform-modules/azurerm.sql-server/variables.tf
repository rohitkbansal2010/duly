variable "server_name" {
  type        = string
  description = "The name of the SQL Server. This needs to be globally unique within Azure."
  default     = ""
}
variable "resource_group_name" {
  type        = string
  description = "The name of the resource group in which to create the SQL Server."
}
variable "location" {
  type        = string
  description = "Specifies the supported Azure location where the resource exists."
}
variable "server_version" {
  type        = string
  description = "The version for the new server. Valid values are: 2.0 (for v11 server) and 12.0 (for v12 server)."
  default     = "12.0"
}
variable "administrator_login" {
  type        = string
  description = "The administrator login name for the new server."
  default     = null
}

variable "azuread_authentication_only" {
  type        = bool
  description = "Specifies whether only AD Users and administrators can be used to login or also local database users"
  default     = true
}

variable "azuread_administrator" {
  type        = string
  description = "Admin Object/App ID of an AzureAD DBA group/user"
  default     = null
}

variable "azuread_administrator_username" {
  description = "A name corresponding to azuread_administrator ID"
}

variable "connection_policy" {
  type        = string
  description = "The connection policy the server will use. Possible values are Default, Proxy, and Redirect. Defaults to Default"
  default     = "Default"
}
variable "vnet_name" {
  type        = string
  description = "The name of the vnet where server takes place"
}
variable "vnet_rg_name" {
  type        = string
  description = "The name of vnet's rg"
}
variable "subnet_name" {
  type        = string
  description = "The name of the subnet where server takes place"
}

variable "firewall_rules" {

}

variable "storage_endpoint" {
  type        = string
  description = " Specifies the blob storage endpoint (e.g. https://MyAccount.blob.core.windows.net)."
  default     = null
}
variable "storage_account_access_key" {
  type        = string
  description = "Specifies the access key to use for the auditing storage account."
  default     = null
}
variable "retention_in_days" {
  type        = number
  description = "Specifies the number of days to retain logs for in the storage account."
  default     = 7
}
variable "storage_account_access_key_is_secondary" {
  type        = bool
  description = "Specifies whether storage_account_access_key value is the storage's secondary key."
  default     = false
}
variable "kv_name" {
  type        = string
  description = "The name of the key vault where the password stored"
}
variable "kv_rg_name" {
  type        = string
  description = "Key vault's resource group name"
}
variable "lock_level" {
  default = ""
}