nsg_flow_log_parameters = {
  nsg_flow_sa_name                    = "dulypnsgflowsa"
  nsg_flow_sa_rg_name                 = "duly-p-vnet-rg"
  nsg_flow_log_workspace_state_path   = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
  nsg_flow_log_workspace_name         = "duly-p-shsvc-loganalytics"
  network_watcher_name                = "duly-p-networkwatcher"
  storage_acc_diagnostic_setting_name = "nsgflow-diagnostic"
  network_rules = {
    bypass         = "AzureServices"
    default_action = "Allow"
    ip_rules       = null
    subnet_associations = [
      #   {
      #     subnet_name = "duly-p-vnet-agw-snet"
      #     vnet_name   = "duly-p-vnet"
      #     rg_name     = "duly-p-vnet-rg"
      #   },
      #   {
      #     subnet_name = "duly-p-vnet-apim-snet"
      #     vnet_name   = "duly-p-vnet"
      #     rg_name     = "duly-p-vnet-rg"
      #   },
      #   {
      #     subnet_name = "duly-p-vnet-app-snet"
      #     vnet_name   = "duly-p-vnet"
      #     rg_name     = "duly-p-vnet-rg"
      #   },
      #   {
      #     subnet_name = "duly-p-vnet-data-snet"
      #     vnet_name   = "duly-p-vnet"
      #     rg_name     = "duly-p-vnet-rg"
      #   },
      #   {
      #     subnet_name = "GatewaySubnet"
      #     vnet_name   = "duly-p-vnet"
      #     rg_name     = "duly-p-vnet-rg"
      #   }
    ]
  }
}
nsg = {
  duly-p-agw-snet-nsg = {
    location                    = "northcentralus"
    rg_name                     = "duly-p-vnet-rg"
    nsg_diagnostic_setting_name = "duly-p-agw-snet-nsg-diagnostic"
    subnet_associate = [
      {
        subnet_name = "duly-p-vnet-agw-snet"
        vnet_name   = "duly-p-vnet"
        rg_name     = "duly-p-vnet-rg"
      }
    ]
    inbound_rules = [
      {
        name                       = "AllowGatewayManager"
        direction                  = "Inbound"
        priority                   = "100"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "GatewayManager"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "65200-65535"
      },
      {
        name                       = "AllowAzureLoadBalancer"
        direction                  = "Inbound"
        priority                   = "101"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "AzureLoadBalancer"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      },
      {
        name                       = "AllowInternet"
        direction                  = "Inbound"
        priority                   = "1000"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "Internet"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      }
    ]
    outbound_rules = []
  }
  duly-p-apim-snet-nsg = {
    location                    = "northcentralus"
    rg_name                     = "duly-p-vnet-rg"
    nsg_diagnostic_setting_name = "duly-p-apim-snet-nsg-diagnostic"
    subnet_associate = [
      {
        subnet_name = "duly-p-vnet-apim-snet"
        vnet_name   = "duly-p-vnet"
        rg_name     = "duly-p-vnet-rg"
      }
    ]
    inbound_rules = [
      {
        name                       = "AllowGatewayManager"
        direction                  = "Inbound"
        priority                   = "100"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "GatewayManager"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "65200-65535"
      },
      {
        name                       = "AllowAzureLoadBalancer"
        direction                  = "Inbound"
        priority                   = "101"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "AzureLoadBalancer"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      },
      {
        name                       = "AllowInternet"
        direction                  = "Inbound"
        priority                   = "1000"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "Internet"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      }
    ]
    outbound_rules = []
  }
  duly-p-app-snet-nsg = {
    location                    = "northcentralus"
    rg_name                     = "duly-p-vnet-rg"
    nsg_diagnostic_setting_name = "duly-p-app-snet-nsg-diagnostic"
    subnet_associate = [
      {
        subnet_name = "duly-p-vnet-app-snet"
        vnet_name   = "duly-p-vnet"
        rg_name     = "duly-p-vnet-rg"
      }
    ]
    inbound_rules = [
      {
        name                       = "AllowGatewayManager"
        direction                  = "Inbound"
        priority                   = "100"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "GatewayManager"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "65200-65535"
      },
      {
        name                       = "AllowAzureLoadBalancer"
        direction                  = "Inbound"
        priority                   = "101"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "AzureLoadBalancer"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      },
      {
        name                       = "AllowInternet"
        direction                  = "Inbound"
        priority                   = "1000"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "Internet"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      }
    ]
    outbound_rules = []
  }
  duly-p-data-snet-nsg = {
    location                    = "northcentralus"
    rg_name                     = "duly-p-vnet-rg"
    nsg_diagnostic_setting_name = "duly-p-data-snet-nsg-diagnostic"
    subnet_associate = [
      {
        subnet_name = "duly-p-vnet-data-snet"
        vnet_name   = "duly-p-vnet"
        rg_name     = "duly-p-vnet-rg"
      }
    ]
    inbound_rules = [
      {
        name                       = "AllowGatewayManager"
        direction                  = "Inbound"
        priority                   = "100"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "GatewayManager"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "65200-65535"
      },
      {
        name                       = "AllowAzureLoadBalancer"
        direction                  = "Inbound"
        priority                   = "101"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "AzureLoadBalancer"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      },
      {
        name                       = "AllowInternet"
        direction                  = "Inbound"
        priority                   = "1000"
        access                     = "allow"
        protocol                   = "*"
        source_address_prefix      = "Internet"
        source_port_range          = "*"
        destination_address_prefix = "*"
        destination_port_range     = "*"
      }
    ]
    outbound_rules = []
  }
}