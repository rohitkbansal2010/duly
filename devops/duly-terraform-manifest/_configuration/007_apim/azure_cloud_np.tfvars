# products on application level
# subscriptions on application level
# named values only two
# custom domains prod: dev portal, mgmt, gateway; qat: gateway
# CORS! Default json policy

apim = [
  {
    apim_name            = "duly-q-apim"
    rg_name              = "duly-q-apim-rg"
    location             = "northcentralus"
    publisher_name       = "Duly Health and Care"
    publisher_email      = "kevin.davisson@boncura.com"
    sku_name             = "Developer_1"
    subnet_name          = "duly-d-vnet-apim-snet"
    vnet_name            = "duly-d-vnet"
    vnet_rg_name         = "duly-d-vnet-rg"
    kv_name              = "duly-d-certs-kv"
    kv_rg_name           = "duly-d-sharedsvc-rg"
    app_insights_name    = "duly-d-shsvc-operations-ai"
    app_insights_rg_name = "duly-d-sharedsvc-rg"
    policy_configuration = [
      {
        xml_content = "./apim-api-config-policy.xml"
      }
    ]
    management_hostname_configuration = [
      {
        host_name    = "api-mgmt-qat.duly-np.digital"
        key_vault_id = "https://duly-d-certs-kv.vault.azure.net/secrets/duly-np-wildcard"
      }
    ]
    developer_portal_hostname_configuration = []
    proxy_hostname_configuration = [
      {
        host_name           = "api-qat.duly-np.digital"
        key_vault_id        = "https://duly-d-certs-kv.vault.azure.net/secrets/duly-np-wildcard"
        default_ssl_binding = true
      }
    ]
    named_values = {
      aad-tenant-id                         = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      ApplicationInsightsInstrumentationKey = "363fca40-80da-4a0f-910d-38bb35c577dd"
    }
  },
  {
    apim_name            = "duly-u-apim"
    rg_name              = "duly-u-apim-rg"
    location             = "northcentralus"
    publisher_name       = "Duly Health and Care"
    publisher_email      = "kevin.davisson@boncura.com"
    sku_name             = "Developer_1"
    subnet_name          = "duly-d-vnet-apim-snet"
    vnet_name            = "duly-d-vnet"
    vnet_rg_name         = "duly-d-vnet-rg"
    kv_name              = "duly-d-certs-kv"
    kv_rg_name           = "duly-d-sharedsvc-rg"
    app_insights_name    = "duly-d-shsvc-operations-ai"
    app_insights_rg_name = "duly-d-sharedsvc-rg"
    policy_configuration = [
      {
        xml_content = "./apim-api-config-policy.xml"
      }
    ]
    management_hostname_configuration = [
      {
        host_name    = "api-mgmt-uat.duly-np.digital"
        key_vault_id = "https://duly-d-certs-kv.vault.azure.net/secrets/duly-np-wildcard"
      }
    ]
    developer_portal_hostname_configuration = []
    proxy_hostname_configuration = [
      {
        host_name           = "api-uat.duly-np.digital"
        key_vault_id        = "https://duly-d-certs-kv.vault.azure.net/secrets/duly-np-wildcard"
        default_ssl_binding = true
      }
    ]
    named_values = {
      aad-tenant-id = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
    }
  }
]