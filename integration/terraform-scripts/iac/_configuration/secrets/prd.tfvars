secrets = [
  {
    resource_name    = "duly-p-integration-clinic-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-p-int-api-clinic-kv"
        kv_rg_name  = "duly-p-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-p-integration-ngdp-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-p-int-api-ngdp-kv"
        kv_rg_name  = "duly-p-collaboration-view-rg"
      }
    ]
  }
]