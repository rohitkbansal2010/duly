ais = [
  {
    ai_name        = "duly-a-collaboration-view-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-a-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-a-cv-api-enc-kv"     = "duly-a-collaboration-view-rg"
      "duly-a-cv-api-res-kv"     = "duly-a-collaboration-view-rg"
    }
  }
]
