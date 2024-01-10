module "public_ip" {
  source            = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.publicip?ref=dev"
  count             = length(var.public_ips)
  name              = var.public_ips[count.index].name
  rg_name           = var.public_ips[count.index].rg_name
  allocation_method = var.public_ips[count.index].allocation_method
  sku               = var.public_ips[count.index].sku
  ip_version        = var.public_ips[count.index].ip_version
  domain_name_label = var.public_ips[count.index].domain_name_label
  tags              = var.public_ips[count.index].tags
}