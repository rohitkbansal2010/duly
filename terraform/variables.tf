variable "env" {
  type = string
}

variable "location" {
  type        = string
  default     = "NorthCentralUS"
  description = "default resources location"
}
 
variable "resource_group_name" {
  type        = string
  description = "resource group name"
}

variable "storage_account_name" {
  type        = string
  description = "storage account name"
}

variable "os_disk_auto_delete" {
  type        = string
}

variable "data_disk_auto_delete" {
  type        = string
}

variable "managed_disk_type" {
  type        = string
}


