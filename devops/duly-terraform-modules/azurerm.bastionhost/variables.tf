variable "vnet_rg_name" {
    type = string
    description = "The name of Vm's vnet resource group"  
}
variable "vnet_name" {
    type = string
    description = "The name of Vm's vnet"  
}
variable "bastion_pip_name" {
    type = string
    description = "The name of bastion PIP"  
}
variable "bastion_host_name" {
    type = string
    description = "The name of bastion host"  
}