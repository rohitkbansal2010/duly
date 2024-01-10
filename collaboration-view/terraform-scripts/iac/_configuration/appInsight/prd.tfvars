ais = [
  {
    ai_name        = "duly-p-collaboration-view-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-p-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-p-cv-api-enc-kv"     = "duly-p-collaboration-view-rg"
      "duly-p-cv-api-res-kv"     = "duly-p-collaboration-view-rg"
    }
  }
]
