locals {
  vnet_ids = [
    for vnet in module.vnet :
    vnet.vnet_id
  ]
  vnet_names = [
    for vnet in module.vnet :
    vnet.vnet_name
  ]
}

output "vnet_ids" {
  value = local.vnet_ids
}

output "vnet_names" {
  value = local.vnet_names
}