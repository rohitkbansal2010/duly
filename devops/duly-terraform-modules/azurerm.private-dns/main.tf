resource "azurerm_private_dns_zone" "private_dns" {
  name                = var.private_dns_zone_name
  resource_group_name = var.rg_name
}

data "azurerm_virtual_network" "vnet" {
  count               = length(var.links)
  name                = var.links[count.index].vnet_name
  resource_group_name = var.links[count.index].rg_name
}

resource "azurerm_private_dns_zone_virtual_network_link" "vnet_link" {
  count                 = length(var.links)
  name                  = "${var.links[count.index].vnet_name}-link"
  resource_group_name   = var.rg_name
  private_dns_zone_name = var.private_dns_zone_name
  virtual_network_id    = data.azurerm_virtual_network.vnet[count.index].id

  depends_on = [
    azurerm_private_dns_zone.private_dns
  ]
}

resource "azurerm_private_dns_a_record" "a_record" {
  count               = length(var.records)
  name                = var.records[count.index].a_record_name
  zone_name           = var.private_dns_zone_name
  resource_group_name = var.rg_name
  ttl                 = 300
  records             = var.records[count.index].a_record_ip_addresses

  depends_on = [
    azurerm_private_dns_zone.private_dns
  ]
}