ais = [
  {
    ai_name        = "duly-q-collaboration-view-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-q-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv             = {
      "duly-d-cv-api-enc-kv"     = "duly-d-collaboration-view-rg"
      "duly-d-cv-api-res-kv"     = "duly-d-collaboration-view-rg"
    }
  }
]
