servers = [
  {
    server_name                    = "duly-p-sql"
    resource_group_name            = "duly-p-data-rg"
    location                       = "northcentralus"
    server_version                 = "12.0"
    administrator_login            = "duly-p-sql--dba"
    azuread_administrator          = "17917b81-bb8e-49ff-9918-3bb1a73b917b" # object id
    azuread_administrator_username = "EPAM-ADM-TEAM"
    azuread_authentication_only    = false
    vnet_name                      = "duly-p-vnet"
    vnet_rg_name                   = "duly-p-vnet-rg"
    subnet_name                    = "duly-p-vnet-data-snet"
    firewall_rules = [
      {
        name             = "Azure services"
        start_ip_address = "0.0.0.0"
        end_ip_address   = "0.0.0.0"
      }
    ]

    kv_name    = "duly-p-shsvc-kv"
    kv_rg_name = "duly-p-sharedsvc-rg"
  }
]


