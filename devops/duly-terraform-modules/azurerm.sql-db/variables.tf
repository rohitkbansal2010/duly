variable "db_name" {
  type        = string
  description = "The name of the SQL Database"
  default     = null
}

variable "server_id" {

}

variable "sku_name" {

}

variable "collation" {
  type        = string
  description = "The name of the collation. Applies only if create_mode is Default. Azure default is SQL_LATIN1_GENERAL_CP1_CI_AS. "
  default     = "SQL_Latin1_General_CP1_CI_AS"
}
variable "max_size_gb" {
  type        = number
  description = "The maximum size that the database can grow to. Applies only if create_mode is Default"
  default     = 2
}
variable "retention_in_days" {
  type        = number
  description = "Specifies the number of days to retain logs for in the storage account."
  default     = 7
}
variable "zone_redundant" {
  type        = bool
  description = "Whether or not this database is zone redundant, which means the replicas of this database will be spread across multiple availability zones."
  default     = false
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
variable "lock_level" {
  default = ""
}

variable "storage_account_type" {
  description = "Possible values are GRS, LRS and ZRS. The default value is GRS."
  default     = "LRS"
}