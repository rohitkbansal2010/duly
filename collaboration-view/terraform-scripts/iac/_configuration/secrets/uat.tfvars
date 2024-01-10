secrets = [
  {
    resource_name    = "duly-u-shsvc-cache"
    resource_rg_name = "duly-d-sharedsvc-rg"
    resource_type    = "redis_cache"
    secret_attribute = "secondary_connection_string"
    keyvaults = [
      {
        secret_name = "Redis--ConnectionString"
        kv_name     = "duly-u-cv-api-enc-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-u-collaboration-view-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-u-cv-api-enc-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
      },
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-u-cv-api-res-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-u-collaboration-view-api"
    resource_type    = "azuread_application"
    secret_attribute = "client_secret"
    keyvaults = [
      {
        secret_name = "Authentication--ClientSecret"
        kv_name     = "duly-u-cv-api-enc-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-u-integration-clinic-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "ClinicApiClientOptions--Scopes--0"
        kv_name     = "duly-u-cv-api-enc-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
        prefix      = "api://"
        suffix      = "/Default.All"
      }
    ]
  },
  {
    resource_name    = "duly-u-integration-ngdp-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "NgdpApiClientOptions--Scopes--0"
        kv_name     = "duly-u-cv-api-enc-kv"
        kv_rg_name  = "duly-u-collaboration-view-rg"
        prefix      = "api://"
        suffix      = "/Default.All"
      }
    ]
  }
]