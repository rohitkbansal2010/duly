# resource "azurerm_servicebus_namespace" "sb_ns" {
#   name                = var.servicebus_namespace
#   location            = var.location
#   resource_group_name = var.servicebus_namespace_rg_name
#   sku                 = "Standard"

#   tags = {
#     source = "terraform"
#   }
# }

data "azurerm_servicebus_namespace" "sb_ns" {
  name                = var.servicebus_namespace
  resource_group_name = var.servicebus_namespace_rg_name
}

resource "azurerm_servicebus_queue" "sb_q" {
  count        = length(var.queues)
  name         = var.queues[count.index]
  namespace_id = data.azurerm_servicebus_namespace.sb_ns.id

  enable_partitioning = true
}

data "azurerm_user_assigned_identity" "uai" {
  count               = length(var.roles)
  name                = var.roles[count.index].user_assigned_identity_name
  resource_group_name = var.roles[count.index].user_assigned_identity_rg_name
}

resource "azurerm_role_assignment" "role" {
  count                = length(var.roles)
  scope                = data.azurerm_servicebus_namespace.sb_ns.id
  role_definition_name = var.roles[count.index].role_definition_name
  principal_id         = data.azurerm_user_assigned_identity.uai[count.index].principal_id
}