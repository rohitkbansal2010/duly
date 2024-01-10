cache = [
  {
    name         = "duly-d-shsvc-cache"
    rg_name      = "duly-d-sharedsvc-rg"
    location     = "northcentralus"
    redis_family = "C"
    redis_sku    = "Basic"
  },
  {
    name         = "duly-u-shsvc-cache"
    rg_name      = "duly-d-sharedsvc-rg"
    location     = "northcentralus"
    redis_family = "C"
    redis_sku    = "Basic"
  }
]
