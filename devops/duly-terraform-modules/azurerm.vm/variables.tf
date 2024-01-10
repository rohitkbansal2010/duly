
variable "vm_vnet_name" {
  description = "The virtual network name"
  default     = ""
}
variable "vm_vnet_rg_name" {
  description = "The virtual network resource group"
  default     = ""
}
variable "vm_subnet_name" {
  description = "The subnet name"
  default     = ""
}
variable "vm_rg_name" {
  description = "The VM resource group"
  default     = ""
}
variable "vm_name" {
  description = "The VM name (must be unique for each VM to create)"
  default     = ""
}
variable "vm_size" {
  description = "The SKU which should be used for this Virtual Machine."
  default     = "Standard_D2s_v3"
}
variable "kv_name" {
  description = "The name of the keyvault which contains the VM password and where will be stored a disk encryption key after disk encryption."
  default     = ""
}
variable "kv_rg_name" {
  description = "The keyvault resource group"
  default     = ""
}
variable "vm_image_publisher" {
  description = "Specifies the publisher of the image used to create the virtual machines."
  default     = "MicrosoftWindowsServer"
}
variable "vm_image_offer" {
  description = "Specifies the offer of the image used to create the virtual machines."
  default     = "WindowsServer"
}
variable "vm_image_sku" {
  description = "Specifies the SKU of the image used to create the virtual machines."
  default     = "2019-Datacenter"
}
variable "vm_admin_username" {
  description = "The name of the secret which contains the VM password stored in the keyvault. The username of the local administrator used for the Virtual Machine. Changing this forces a new resource to be created."
  default     = ""
}
variable "vm_pip_allocation_method" {
  description = "Defines the allocation method for this IP address. Possible values are Static or Dynamic"
  default     = "Static"
}
variable "vm_private_ip_allocation_method" { 
  description = "The allocation method used for the Private IP Address. Possible values are Dynamic and Static."
  default     = "Static" 
}
variable "vm_private_ip_address" {
  description = "The private ip address of the VM"
  default     = ""
}
variable "vm_guest_os" { 
  description = "The name of the guest OS"
  default     = "windows"
}
variable "enable_data_disk" {
  description = "Data disk to enable"
  default     = true
}
variable "vm_create_pip" {
  description = "VMs public ip to create"
  default     = false
}
 variable "storage_account_uri" {
  description = "The endpoint URL for blob storage in the primary location (primary_blob_endpoint)."
  default     = ""
 }
variable "bootdiag_storage_id" {
  description = "The ID of the Storage Account."
  default     = ""
}
variable "bootdiag_storage_name" {
  description = "The name of the Storage Account."
  default     = ""
}
variable "bootdiag_storage_primary_access_key" {
  description = "The primary access key for the storage account."
  default     = ""
}

variable "vm_disable_password_authentication" {
  default = false
}

# os disk variables
variable "storage_account_type" {
  description = "The Type of Storage Account which should back this the Internal OS Disk. Possible values are Standard_LRS, StandardSSD_LRS and Premium_LRS"
  default     = "Standard_LRS"
}

# data disk variables
variable "data_disk_size_gb" {
  description = "The size of the data disk"
  default     = 10
}
variable "data_disk_lun" {
  description = "The logic volume number"
  default     = 10
}

# VM extension
variable "vm_dsc_management_install" {
  description = "The Desired state configuration management extention"
  default     = false
}
variable "vm_disk_encryption_install" {
  description = "Disk encryption to install"
  default = false
}
variable "vm_diagnostic_setting_install" {
  description = "Diagnostic setting to install"
  default     = false
}

# VM disks encryption config
variable "encryption_key_url" {
  description = "URL to encrypt Key. The URL of the key encryption key, in the format https://<keyvault-name>.vault.azure.net/key/<key-name>. If you do not wish to use a KEK, leave this field blank."
  default     = ""
}
variable "encryption_algorithm" {
  description = " Algo for encryption"
  default     = "RSA-OAEP"
}
variable "volume_type" {
  description = "Type of volume that the encryption operation is performed on. Valid values are OS, Data, and All. Encryption operations on data volume need encryption to be enabled OS volume first."
  default     = "All"
}
variable "encrypt_operation" {
  description = "The encryption operation"
  default     = "EnableEncryption"
}
variable "tags" {
  type = map(string)
  default = {}
}
variable "asg_name" {
  description = "The name of the application security group"
  type        = string
}
# variable "rec_vault_name" {
#   description = "The name of the recovery vault"
#   type        = string
# }
# variable "rec_vault_rg_name" {
#   description = "The name of the resource group of the recovery vault"
#   type        = string
# }
# variable "backup_policy_name" {
#   description = "The name of the vm backup_policy"
#   type        = string
# }
