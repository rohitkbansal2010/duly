ais = [
  {
    ai_name        = "duly-u-collaboration-view-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-u-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-u-cv-api-enc-kv"     = "duly-u-collaboration-view-rg"
      "duly-u-cv-api-res-kv"     = "duly-u-collaboration-view-rg"
    }
  }
]
