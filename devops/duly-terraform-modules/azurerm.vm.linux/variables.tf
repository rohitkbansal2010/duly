variable "name" {
  default = "aks-injector"
}

variable "rg_name" {}

variable "location" {}

variable "vnet_name" {
  default     = ""
}
variable "vnet_rg_name" {
  default     = ""
}
variable "subnet_name" {
  default     = ""
}

variable "size" {
  default = "Standard_A1_v2"
}

variable "admin_username" {
  default = "adminuser"
}

variable "private_ip_allocation_method" { 
  default     = "Static" 
}

variable "private_ip_address" {
  default     = ""
}

variable "create_pip" {
  default     = false
}

variable "image_publisher" {
  default = "Canonical"
}

variable "image_offer" {
  default = "UbuntuServer"
}

variable "image_sku" {
  default = "18.04-LTS"
}

variable "image_version" {
  default = "latest"
}