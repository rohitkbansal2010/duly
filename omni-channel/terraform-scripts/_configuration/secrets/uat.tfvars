secrets = [
  {
    resource_name    = "duly-u-appointment-management"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-u-oc-apmg-orc-kv"
        kv_rg_name  = "duly-u-appointment-management-rg"
      }
    ]
  },
  {
    resource_name    = "duly-u-appointment-management"
    resource_type    = "azuread_application"
    secret_attribute = "client_secret"
    keyvaults = [
      {
        secret_name = "Authentication--ClientSecret"
        kv_name     = "duly-u-oc-apmg-orc-kv"
        kv_rg_name  = "duly-u-appointment-management-rg"
      }
    ]
  },
  {
    resource_name    = "duly-u-communication-hub-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "IngestionSettings--IngestionScope"
        kv_name     = "duly-u-oc-apmg-orc-kv"
        kv_rg_name  = "duly-u-appointment-management-rg"
        prefix      = "api://"
        suffix      = "/.default"
      }
    ]
  }
]