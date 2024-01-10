cache = [
  {
    name         = "duly-p-shsvc-cache"
    rg_name      = "duly-p-sharedsvc-rg"
    location     = "northcentralus"
    redis_family = "C"
    redis_sku    = "Basic"
  },
  {
    name         = "duly-p-apim-cache"
    rg_name      = "duly-p-apim-rg"
    location     = "northcentralus"
    redis_family = "C"
    redis_sku    = "Basic"
  }
]