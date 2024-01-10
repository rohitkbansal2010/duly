ais = [
  {
    ai_name        = "duly-u-integration-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-u-collaboration-view-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-u-int-api-clinic-kv" = "duly-u-collaboration-view-rg"
      "duly-u-int-api-ngdp-kv"   = "duly-u-collaboration-view-rg"
    }
  }
]
