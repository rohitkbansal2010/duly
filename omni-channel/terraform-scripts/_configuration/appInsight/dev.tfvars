ais = [
  {
    ai_name        = "duly-d-appointment-management-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-d-appointment-management-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-d-oc-apmg-orc-kv"     = "duly-d-appointment-management-rg"
    }
  }
]
