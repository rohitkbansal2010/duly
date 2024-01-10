ais = [
  {
    ai_name        = "duly-q-integration-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-q-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-q-int-api-clinic-kv" = "duly-q-collaboration-view-rg"
      "duly-q-int-api-ngdp-kv"   = "duly-q-collaboration-view-rg"
    }
  }
]
