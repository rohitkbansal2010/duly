ais = [
  {
    ai_name        = "duly-q-collaboration-view-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-q-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv             = {
      "duly-q-cv-api-enc-kv"     = "duly-q-collaboration-view-rg"
      "duly-q-cv-api-res-kv"     = "duly-q-collaboration-view-rg"
    }
  }
]
