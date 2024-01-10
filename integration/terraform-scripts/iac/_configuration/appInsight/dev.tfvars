ais = [
  {
    ai_name        = "duly-d-integration-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-d-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-d-int-api-clinic-kv" = "duly-d-collaboration-view-rg"
      "duly-d-int-api-ngdp-kv"   = "duly-d-collaboration-view-rg"
    }
  }
]
