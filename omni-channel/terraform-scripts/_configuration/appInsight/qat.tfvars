ais = [
  {
    ai_name        = "duly-q-appointment-management-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-q-appointment-management-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-q-oc-apmg-orc-kv"     = "duly-q-appointment-management-rg"
    }
  }
]
