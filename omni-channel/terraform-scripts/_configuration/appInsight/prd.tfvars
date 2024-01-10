ais = [
  {
    ai_name        = "duly-p-appointment-management-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-p-appointment-management-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-p-oc-apmg-orc-kv"     = "duly-p-appointment-management-rg"
    }
  }
]