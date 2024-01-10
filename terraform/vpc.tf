module "vnet" {
  source  = "kumarvna/vnet/azurerm"
  version = "2.2.0"

  create_resource_group          = false
  vnetwork_name                  = "duly-${var.env}-vnet-wrapper"
  resource_group_name            = var.resource_group_name
  location                       = var.location
  vnet_address_space             = ["10.1.0.0/16"]
  firewall_subnet_address_prefix = ["10.1.0.0/26"]
  gateway_subnet_address_prefix  = ["10.1.1.0/27"]
  create_network_watcher         = false

  # Adding Standard DDoS Plan, and custom DNS servers (Optional)
  create_ddos_plan = false

  subnets = {
    public_subnet = {
      subnet_name           = "duly-${var.env}-snet-wrapper-public"
      subnet_address_prefix = ["10.1.2.0/24"]
      delegation = {}

      nsg_inbound_rules = [
        # [name, priority, direction, access, protocol, destination_port_range, source_address_prefix, destination_address_prefix]
        # To use defaults, use "" without adding any values.
        ["weballow", "100", "Inbound", "Allow", "Tcp", "80", "*", "0.0.0.0/0"],
        ["weballow1", "101", "Inbound", "Allow", "", "443", "*", ""],
      ]

      nsg_outbound_rules = [
        # [name, priority, direction, access, protocol, destination_port_range, source_address_prefix, destination_address_prefix]
        # To use defaults, use "" without adding any values.
        ["ntp_out", "103", "Outbound", "Allow", "Udp", "123", "", "0.0.0.0/0"],
      ]
    }

    private_subnet = {
      subnet_name           = "duly-${var.env}-snet-wrapper-private"
      subnet_address_prefix = ["10.1.3.0/24"]
      service_endpoints     = ["Microsoft.Storage"]

      nsg_inbound_rules = [
        ["weballow", "200", "Inbound", "Allow", "Tcp", "80", "*", ""],
        ["weballow1", "201", "Inbound", "Allow", "Tcp", "443", "AzureLoadBalancer", ""],
        ["weballow2", "202", "Inbound", "Allow", "Tcp", "9090", "VirtualNetwork", ""],
      ]

      nsg_outbound_rules = []
    }
  }
  #private_subnets = ["duly-test-snet-wrapper-private"]

  tags = {}
}


resource "azurerm_network_interface" "main" {
  name                = "duly-${var.env}-wrapper-nic"
  location            = var.location
  resource_group_name = var.resource_group_name

  ip_configuration {
    name                          = "duly-${var.env}-wrapper-ip"
    subnet_id                     = module.vnet.subnet_ids[0]
    private_ip_address_allocation = "Dynamic"
  }
}
