secrets = [
  {
    resource_name    = "duly-d-integration-clinic-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-d-int-api-clinic-kv"
        kv_rg_name  = "duly-d-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-d-integration-ngdp-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-d-int-api-ngdp-kv"
        kv_rg_name  = "duly-d-collaboration-view-rg"
      }
    ]
  }
]