secrets = [
  {
    resource_name    = "duly-q-integration-clinic-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-q-int-api-clinic-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-q-integration-ngdp-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-q-int-api-ngdp-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
      }
    ]
  }
]