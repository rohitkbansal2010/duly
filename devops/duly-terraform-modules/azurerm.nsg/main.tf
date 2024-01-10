# Create NSG
resource "azurerm_network_security_group" "nsg" {
  name                      = var.nsg_name
  location                  = var.location
  resource_group_name       = var.resource_group_name
  tags                      = var.tags
}

# Get subnet to associate with NSG
data "azurerm_subnet" "subnet_associate" {
  count                     = length(var.subnet_associate)

  name                      = var.subnet_associate[count.index].subnet_name
  virtual_network_name      = var.subnet_associate[count.index].vnet_name
  resource_group_name       = lookup(var.subnet_associate[count.index], "rg_name", var.resource_group_name)
}

# Associate subnet with NSG.
# NSG will be associated with subnet only after all rules are created, it significant for subnets such as AzureBastion or ApplicationGatewaySubnet.
resource "azurerm_subnet_network_security_group_association" "subnet" {
  count                     = length(var.subnet_associate)
  depends_on                = [ azurerm_network_security_rule.inbound_rules, azurerm_network_security_rule.outbound_rules ]

  subnet_id                 = data.azurerm_subnet.subnet_associate[count.index].id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

# Collect all ASG names to single array to get ids
# locals {
#   inbound_destination_asg_list = [
#     for rule in var.inbound_rules :
#       lookup(rule, "destination_asg", [])
#   ]
#   outbound_destination_asg_list = [
#     for rule in var.outbound_rules :
#       lookup(rule, "destination_asg", [])
#   ]
#   inbound_source_asg_list = [
#     for rule in var.inbound_rules :
#       lookup(rule, "source_asg", [])
#   ]
#   outbound_source_asg_list = [
#     for rule in var.outbound_rules :
#       lookup(rule, "source_asg", [])
#   ]

#   asg_list = flatten(setunion(flatten(local.inbound_destination_asg_list), flatten(local.outbound_destination_asg_list), flatten(local.inbound_source_asg_list), flatten(local.outbound_source_asg_list)))
# }

# # Get ASG data to associate with NSG
# data "azurerm_application_security_group" "asg" {
#   count                     = length(local.asg_list)

#   name                      = local.asg_list[count.index].name
#   resource_group_name       = local.asg_list[count.index].rg_name
# }

# locals {
#   # Create ASG map '<name> = <id>'
#   asg_names = [
#     for asg in data.azurerm_application_security_group.asg :
#     asg.name
#   ]
#   asg_ids = [
#     for asg in data.azurerm_application_security_group.asg :
#     asg.id
#   ]

#   asg_names_ids_map = zipmap(local.asg_names, local.asg_ids)

#   # Replace names with ids in ASG_list locals
#   inbound_destination_asg_ids = [
#     for rule in local.inbound_destination_asg_list : [
#       for asg in rule :
#       lookup(local.asg_names_ids_map, asg.name)
#     ]
#   ]
#   outbound_destination_asg_ids = [
#     for rule in local.outbound_destination_asg_list : [
#       for asg in rule :
#       lookup(local.asg_names_ids_map, asg.name)
#     ]
#   ]
#   inbound_source_asg_ids = [
#     for rule in local.inbound_source_asg_list : [
#       for asg in rule :
#       lookup(local.asg_names_ids_map, asg.name)
#     ]
#   ]
#   outbound_source_asg_ids = [
#     for rule in local.outbound_source_asg_list : [
#       for asg in rule :
#       lookup(local.asg_names_ids_map, asg.name)
#     ]
#   ]
# }

# Create NSG inbound rules
resource "azurerm_network_security_rule" "inbound_rules" {
  count                                      = length(var.inbound_rules)
  name                                       = lookup(var.inbound_rules[count.index], "name")
  priority                                   = lookup(var.inbound_rules[count.index], "priority", count.index == 0 ? 100 : 100 + 10 * count.index)
  direction                                  = lookup(var.inbound_rules[count.index], "direction", "Inbound")
  access                                     = lookup(var.inbound_rules[count.index], "access", "Allow")
  protocol                                   = lookup(var.inbound_rules[count.index], "protocol")
  source_port_range                          = lookup(var.inbound_rules[count.index], "source_port_range", null)
  source_port_ranges                         = lookup(var.inbound_rules[count.index], "source_port_ranges", null)
  destination_port_range                     = lookup(var.inbound_rules[count.index], "destination_port_range", null)
  destination_port_ranges                    = lookup(var.inbound_rules[count.index], "destination_port_ranges", null)
  source_address_prefix                      = lookup(var.inbound_rules[count.index], "source_address_prefix", null)
  source_address_prefixes                    = lookup(var.inbound_rules[count.index], "source_address_prefixes", null)
  destination_address_prefix                 = lookup(var.inbound_rules[count.index], "destination_address_prefix", null)
  destination_address_prefixes               = lookup(var.inbound_rules[count.index], "destination_address_prefixes", null)
  # source_application_security_group_ids      = local.inbound_source_asg_ids[count.index]
  # destination_application_security_group_ids = local.inbound_destination_asg_ids[count.index]
  resource_group_name                        = var.resource_group_name
  network_security_group_name                = azurerm_network_security_group.nsg.name
}

# Create NSG outbound rules
resource "azurerm_network_security_rule" "outbound_rules" {
  count                                      = length(var.outbound_rules)
  name                                       = lookup(var.outbound_rules[count.index], "name")
  priority                                   = lookup(var.outbound_rules[count.index], "priority", count.index == 0 ? 100 : 100 + 10 * count.index)
  direction                                  = lookup(var.outbound_rules[count.index], "direction", "Oubound")
  access                                     = lookup(var.outbound_rules[count.index], "access", "Allow")
  protocol                                   = lookup(var.outbound_rules[count.index], "protocol")
  source_port_range                          = lookup(var.outbound_rules[count.index], "source_port_range", null)
  source_port_ranges                         = lookup(var.outbound_rules[count.index], "source_port_ranges", null)
  destination_port_range                     = lookup(var.outbound_rules[count.index], "destination_port_range", null)
  destination_port_ranges                    = lookup(var.outbound_rules[count.index], "destination_port_ranges", null)
  source_address_prefix                      = lookup(var.outbound_rules[count.index], "source_address_prefix", null)
  source_address_prefixes                    = lookup(var.outbound_rules[count.index], "source_address_prefixes", null)
  destination_address_prefix                 = lookup(var.outbound_rules[count.index], "destination_address_prefix", null)
  destination_address_prefixes               = lookup(var.outbound_rules[count.index], "destination_address_prefixes", null)
  # source_application_security_group_ids      = local.outbound_source_asg_ids[count.index]
  # destination_application_security_group_ids = local.outbound_destination_asg_ids[count.index]
  resource_group_name                        = var.resource_group_name
  network_security_group_name                = azurerm_network_security_group.nsg.name
}

# Create logging for NSG
resource "azurerm_network_watcher_flow_log" "nsg01flow" {
  network_watcher_name      = var.network_watcher_name
  resource_group_name       = var.resource_group_name
  network_security_group_id = azurerm_network_security_group.nsg.id
  storage_account_id        = var.nsg_flow_sa_id
  version                   = 2
  enabled                   = true

  retention_policy {
    enabled = true
    days    = 7
  }
  
  traffic_analytics {
    enabled               = true
    workspace_id          = var.log_analytics_workspace_id
    workspace_region      = var.location
    workspace_resource_id = var.log_analytics_workspace_resource_id
  }
}


# Manages a diagnostic setting for created nsg
resource "azurerm_monitor_diagnostic_setting" "nsg" {
  name                       = var.nsg_diagnostic_setting_name
  target_resource_id         = azurerm_network_security_group.nsg.id
  log_analytics_workspace_id = var.log_analytics_workspace_resource_id

  log {
    category = "NetworkSecurityGroupEvent"
    enabled  = true

    retention_policy {
      enabled = true
    }
  }

  # log {
  #   category = "NetworkSecurityGroupRuleCounter"

  #   retention_policy {
  #     enabled = false
  #   }
  # }
}
