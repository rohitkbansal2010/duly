ais = [
  {
    ai_name        = "duly-u-appointment-management-ai"
    ai_location    = "northcentralus"
    ai_rg_name     = "duly-u-appointment-management-rg"
    kv_secret_name = "Serilog--WriteTo--1--Args--instrumentationKey"
    kv = {
      "duly-u-oc-apmg-orc-kv"     = "duly-u-appointment-management-rg"
    }
  }
]