locals {
  links = [
    for zone in var.zones : [
      for link in zone.network_link : {
        vnet_name = link.vnet_name
        rg_name   = link.rg_name
      }
    ]
  ]
}

module "private_dns_zone" {
  source                = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.private-dns?ref=dev"
  count                 = length(var.zones)
  private_dns_zone_name = var.zones[count.index].private_dns_zone_name
  rg_name               = var.zones[count.index].rg_name
  links                 = local.links[count.index]
  records               = var.zones[count.index].records
}