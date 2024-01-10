vnets = [
  {
    vnet_name                          = "duly-p-vnet"
    rg_name                            = "duly-p-vnet-rg"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name       = "duly-p-shsvc-loganalytics"
    vnet_diagnostic_setting_name       = "duly-p-vnet-diagnostic"
    address_space = [
      "172.26.40.0/21"
    ]
    dns_servers = [
      # "172.26.1.4",
      # "172.17.46.111",
      # "172.16.46.140"
    ]
    subnet_names = [
      "duly-p-vnet-agw-snet",
      "duly-p-vnet-apim-snet",
      "duly-p-vnet-app-snet",
      "duly-p-vnet-data-snet",
      "GatewaySubnet"
    ]
    subnet_prefixes = [
      # duly-p-vnet-agw-snet
      "172.26.40.0/26",
      # duly-p-vnet-apim-snet
      "172.26.40.64/26",
      # duly-p-vnet-app-snet
      "172.26.41.0/24",
      # duly-p-vnet-data-snet
      "172.26.42.0/24",
      # GatewaySubnet
      "172.26.40.192/26"
    ]

    subnet_enforce_private_link_endpoint_network_policies = {
      duly-p-vnet-app-snet  = true
      duly-p-vnet-data-snet = true
    }

    subnet_service_endpoints = {
      duly-p-vnet-agw-snet = [
        "Microsoft.KeyVault"
      ],
      duly-p-vnet-apim-snet = [
        "Microsoft.KeyVault"
      ],
      duly-p-vnet-app-snet = [
        "Microsoft.KeyVault"
      ],
      duly-p-vnet-data-snet = [
        "Microsoft.Sql"
      ],
      GatewaySubnet = [

      ]

    }
    tags = {
      environment = "PRD"
    }
  }
]
