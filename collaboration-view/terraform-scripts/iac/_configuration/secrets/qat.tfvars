secrets = [
  {
    resource_name    = "duly-d-shsvc-cache"
    resource_rg_name = "duly-d-sharedsvc-rg"
    resource_type    = "redis_cache"
    secret_attribute = "secondary_connection_string"
    keyvaults = [
      {
        secret_name = "Redis--ConnectionString"
        kv_name     = "duly-q-cv-api-enc-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-q-collaboration-view-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-q-cv-api-enc-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
      },
      {
        secret_name = "Authentication--ClientId"
        kv_name     = "duly-q-cv-api-res-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-q-collaboration-view-api"
    resource_type    = "azuread_application"
    secret_attribute = "client_secret"
    keyvaults = [
      {
        secret_name = "Authentication--ClientSecret"
        kv_name     = "duly-q-cv-api-enc-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
      }
    ]
  },
  {
    resource_name    = "duly-q-integration-clinic-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "ClinicApiClientOptions--Scopes--0"
        kv_name     = "duly-q-cv-api-enc-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
        prefix      = "api://"
        suffix      = "/Default.All"
      }
    ]
  },
  {
    resource_name    = "duly-q-integration-ngdp-api"
    resource_type    = "azuread_application"
    secret_attribute = "application_id"
    keyvaults = [
      {
        secret_name = "NgdpApiClientOptions--Scopes--0"
        kv_name     = "duly-q-cv-api-enc-kv"
        kv_rg_name  = "duly-q-collaboration-view-rg"
        prefix      = "api://"
        suffix      = "/Default.All"
      }
    ]
  }
]