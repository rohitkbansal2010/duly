resource "azurerm_redis_cache" "cache" {
  count               = length(var.cache)
  name                = var.cache[count.index].name
  location            = var.cache[count.index].location
  resource_group_name = var.cache[count.index].rg_name
  capacity            = 0
  family              = var.cache[count.index].redis_family
  sku_name            = var.cache[count.index].redis_sku
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  redis_configuration {
  }
}
