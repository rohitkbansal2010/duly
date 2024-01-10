secrets = [
  {
    resource_name    = "duly-u-integration-clinic-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-u-int-api-clinic-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-u-integration-ngdp-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-u-int-api-ngdp-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
      }
    ]
  }
]