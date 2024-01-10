vnets = [
  {
    vnet_name                          = "duly-d-vnetdops"
    rg_name                            = "duly-d-vnet-rg"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    vnet_diagnostic_setting_name       = "duly-d-vnetdops-diagnostic"
    address_space = [
      "172.26.230.0/24"
    ]
    dns_servers = [
      # "172.26.1.4",
      # "172.17.46.111",
      # "172.16.46.140"
    ]
    subnet_names = [
      "duly-d-vnetdops-cicd-snet",
      "duly-d-vnetdops-data-snet"
    ]
    subnet_prefixes = [
      "172.26.230.0/26",
      "172.26.230.64/26"
    ]

    subnet_enforce_private_link_endpoint_network_policies = {
      duly-d-vnetdops-cicd-snet = true
    }

    subnet_service_endpoints = {
      duly-d-vnetdops-cicd-snet = [

      ],
      duly-d-vnetdops-data-snet = [
        "Microsoft.Sql",
        "Microsoft.DBforPostgreSQL/flexibleServers"
      ]
    }

    tags = {
      environment = "dev"
      provision   = "Terraform"
    }

  },
  {
    vnet_name                          = "duly-d-vnet"
    rg_name                            = "duly-d-vnet-rg"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    vnet_diagnostic_setting_name       = "duly-d-vnet-diagnostic"
    address_space = [
      "172.26.32.0/21"
    ]
    dns_servers = [
      # "172.26.1.4",
      # "172.17.46.111",
      # "172.16.46.140"
    ]
    subnet_names = [
      "duly-d-vnet-agw-snet",
      "duly-d-vnet-apim-snet",
      "duly-d-vnet-app-snet",
      "duly-d-vnet-data-snet",
      "duly-q-vnet-data-snet",
      "duly-u-vnet-data-snet",
      "duly-q-vnet-test-snet",
      "AzureBastionSubnet",
      "GatewaySubnet"
    ]
    subnet_prefixes = [
      "172.26.32.0/26",
      "172.26.32.64/26",
      "172.26.33.0/24",
      "172.26.34.0/26",
      "172.26.34.64/26",
      "172.26.34.128/26",
      "172.26.35.0/27",
      "172.26.35.32/27",
      "172.26.32.192/26"
    ]

    subnet_enforce_private_link_endpoint_network_policies = {
      duly-d-vnet-app-snet = true
    }

    subnet_service_endpoints = {
      duly-d-vnet-agw-snet = [
        "Microsoft.KeyVault"
      ],
      duly-d-vnet-apim-snet = [
        "Microsoft.KeyVault"
      ],
      duly-d-vnet-app-snet = [
        "Microsoft.KeyVault"
      ],
      duly-d-vnet-data-snet = [
        "Microsoft.Sql"
      ],
      duly-q-vnet-data-snet = [
        "Microsoft.Sql"
      ],
      duly-u-vnet-data-snet = [
        "Microsoft.Sql"
      ],
      duly-q-vnet-test-snet = [

      ],
      AzureBastionSubnet = [

      ],
      GatewaySubnet = [

      ]

    }
    tags = {
      environment = "dev"
      provision   = "Terraform"
    }
  }
]
