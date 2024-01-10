products = [
  {
    api_management_name    = "duly-q-apim"
    api_management_rg_name = "duly-q-apim-rg"
    product_id             = "collaboration-view-apis-sdk"
    product_display_name   = "Collaboration View APIs SDK"
    product_description    = "The collection of APIs for Collaboration View application."
    subscriptions = {
      "collaboration-view-apis-aut" = []
      "collaboration-view-web-aut"  = []
    }
  },
  {
    api_management_name    = "duly-q-apim"
    api_management_rg_name = "duly-q-apim-rg"
    product_id             = "system-apis-sdk"
    product_display_name   = "System APIs SDK"
    product_description    = "Perform set of Epic related operations requested from process API."
    subscriptions = {
      "collaboration-view-ngdp-aut" = [
        {
          secret_name = "NgdpApiClientOptions--SubscriptionKey"
          kv_name     = "duly-a-cv-api-enc-kv"
          kv_rg_name  = "duly-a-collaboration-view-rg"
        }
      ]
      "collaboration-view-clinic-aut" = [
        {
          secret_name = "ClinicApiClientOptions--SubscriptionKey"
          kv_name     = "duly-a-cv-api-enc-kv"
          kv_rg_name  = "duly-a-collaboration-view-rg"
        }
      ]
    }
  }
]
