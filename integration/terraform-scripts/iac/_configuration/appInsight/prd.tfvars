ais = [
  {
    ai_name        = "duly-p-integration-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-p-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-p-int-api-clinic-kv" = "duly-p-collaboration-view-rg"
      "duly-p-int-api-ngdp-kv"   = "duly-p-collaboration-view-rg"
    }
  }
]
