# Define providers list
# terraform {
#   required_version = ">= 0.13.5"
#   required_providers {
#     azurerm = {
#       source  = "hashicorp/azurerm"
#       version = "> 2.4.0"
#     }
#   }
# }

# Get virtual networks data from tfstate files
data "terraform_remote_state" "vnet" {
  count                  = length(var.vnet_peerings)
  backend                = "azurerm"
  config  = {
    container_name       = var.backend_container_name
    # key                  = "011_vnet/${var.backend_tfvars_folder_list[count.index]}/terraform.tfstate"
    key = var.vnet_peerings[count.index].vnet_state_path
    resource_group_name  = var.backend_resource_group_name
    storage_account_name = var.backend_storage_account_name
    subscription_id      = var.backend_subscription_id
    client_secret        = var.backend_client_secret
    client_id            = var.backend_client_id
    tenant_id            = var.backend_tenant_id
  }
}

# Create map with virtual network data: '<name> = <id>'
locals {
  vnet_ids = flatten([
    for vnet in data.terraform_remote_state.vnet :
    vnet.outputs.vnet_ids
  ])
  vnet_names = flatten([
    for vnet in data.terraform_remote_state.vnet :
    vnet.outputs.vnet_names
  ])

  vnet_names_ids_map = zipmap(local.vnet_names, local.vnet_ids)
}

# Create virtual network peering
module "vnet_peering" {
  source                       = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.vnetpeering?ref=dev"
  count                        = length(var.vnet_peerings)
  name                         = var.vnet_peerings[count.index].name
  resource_group_name          = var.vnet_peerings[count.index].source_vnet_rg_name
  virtual_network_name         = var.vnet_peerings[count.index].source_vnet_name
  remote_virtual_network_id    = lookup(local.vnet_names_ids_map, var.vnet_peerings[count.index].destination_vnet_name)
  allow_virtual_network_access = var.vnet_peerings[count.index].traffic_from_remote_vnet == "allow" ? true : false
  allow_forwarded_traffic      = var.vnet_peerings[count.index].traffic_forwarded_from_remote == "allow" ? true : false
  allow_gateway_transit        = var.vnet_peerings[count.index].allow_gateway_transit == "allow" ? true : false
  use_remote_gateways          = var.vnet_peerings[count.index].use_virtual_network_gateway == "yes" ? true : false
}



####### For local development ########
# data "terraform_remote_state" "vnet" {
#   count   = length(var.backend_tfvars_folder_list)

#   backend = "azurerm"
#   config  = {
#     container_name       = "terraform-state-files"
#     key                  = "011_vnet/${var.backend_tfvars_folder_list[count.index]}/terraform.tfstate"
#     resource_group_name  = "epam-rg-noeu-h-tfstate-01"
#     storage_account_name = "noeuhtfstatesa"
#     subscription_id      = "8375be24-0868-44fa-85f6-67a8434b5b45"
#   }
# }