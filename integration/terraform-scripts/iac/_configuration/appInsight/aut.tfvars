ais = [
  {
    ai_name        = "duly-a-integration-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-a-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-a-int-api-clinic-kv" = "duly-a-collaboration-view-rg"
      "duly-a-int-api-ngdp-kv"   = "duly-a-collaboration-view-rg"
    }
  }
]
