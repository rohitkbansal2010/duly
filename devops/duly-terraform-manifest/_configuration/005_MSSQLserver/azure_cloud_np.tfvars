servers = [
  {
    server_name                    = "duly-d-db"
    resource_group_name            = "duly-d-data-rg"
    location                       = "northcentralus"
    server_version                 = "12.0"
    administrator_login            = "dba"
    azuread_administrator          = "17917b81-bb8e-49ff-9918-3bb1a73b917b"
    azuread_administrator_username = "EPAM-ADM-TEAM"
    azuread_authentication_only    = true
    vnet_name                      = "duly-d-vnet"
    vnet_rg_name                   = "duly-d-vnet-rg"
    subnet_name                    = "duly-d-vnet-data-snet"

    firewall_rules = [
      {
        name             = "us1"
        start_ip_address = "174.128.60.160"
        end_ip_address   = "174.128.60.162"
      },
      {
        name             = "Azure services"
        start_ip_address = "0.0.0.0"
        end_ip_address   = "0.0.0.0"
      }
    ]

    kv_name    = "duly-d-shsvc-kv"
    kv_rg_name = "duly-d-sharedsvc-rg"
  }
]


