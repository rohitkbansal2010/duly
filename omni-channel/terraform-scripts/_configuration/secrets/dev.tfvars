secrets = [
  {
    resource_name    = "duly-d-appointment-management"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-d-oc-apmg-orc-kv"
        kv_rg_name  = "duly-d-appointment-management-rg"
      }
    ]
  },
  {
    resource_name    = "duly-d-appointment-management"
    resource_type    = "azuread_application"
    secret_attribute = "client_secret"
    keyvaults = [
      {
        secret_name = "Authentication--ClientSecret"
        kv_name     = "duly-d-oc-apmg-orc-kv"
        kv_rg_name  = "duly-d-appointment-management-rg"
      }
    ]
  },
  {
    resource_name    = "duly-d-communication-hub-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "IngestionSettings--IngestionScope"
        kv_name     = "duly-d-oc-apmg-orc-kv"
        kv_rg_name  = "duly-d-appointment-management-rg"
        prefix      = "api://"
        suffix      = "/.default"
      }
    ]
  }
]