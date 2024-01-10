apim = [
  {
    apim_name            = "duly-p-apim"
    rg_name              = "duly-p-apim-rg"
    location             = "northcentralus"
    publisher_name       = "Duly Health and Care"
    publisher_email      = "kevin.davisson@boncura.com"
    sku_name             = "Premium_1"
    subnet_name          = "duly-p-vnet-apim-snet"
    vnet_name            = "duly-p-vnet"
    vnet_rg_name         = "duly-p-vnet-rg"
    kv_name              = "duly-p-certs-kv"
    kv_rg_name           = "duly-p-sharedsvc-rg"
    app_insights_name    = "duly-p-shsvc-operations-ai"
    app_insights_rg_name = "duly-p-sharedsvc-rg"
    redis_cache_name     = "duly-p-apim-cache"
    policy_configuration = [
      {
        xml_content = "./apim-api-config-policy.xml" # CORS
      }
    ]
    management_hostname_configuration = [
      {
        host_name    = "api-mgmt.duly.digital"
        key_vault_id = "https://duly-p-certs-kv.vault.azure.net/secrets/duly-wildcard-prd"
      }
    ]
    developer_portal_hostname_configuration = [
      {
        host_name    = "api-portal.duly.digital"
        key_vault_id = "https://duly-p-certs-kv.vault.azure.net/secrets/duly-wildcard-prd"
      }
    ]
    proxy_hostname_configuration = [
      {
        host_name           = "api.duly.digital"
        key_vault_id        = "https://duly-p-certs-kv.vault.azure.net/secrets/duly-wildcard-prd"
        default_ssl_binding = true
      }
    ]
    named_values = {
      aad-tenant-id = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
    }
  }
]