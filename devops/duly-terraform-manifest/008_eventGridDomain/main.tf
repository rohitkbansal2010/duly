resource "azurerm_eventgrid_domain" "egd" {
  count               = length(var.egds)
  name                = var.egds[count.index].name
  location            = var.egds[count.index].location
  resource_group_name = var.egds[count.index].rg_name
  input_schema        = "CloudEventSchemaV1_0"

  identity {
    type = "SystemAssigned"
  }
  # tags                = var.egds[count.index].tags
}

locals {
  egds_roles = flatten([
    for i in var.egds : [
      for r in i.roles : {
        egd_name                       = i.name
        egd_rg_name                    = i.rg_name
        user_assigned_identity_name    = r.user_assigned_identity_name
        user_assigned_identity_rg_name = r.user_assigned_identity_rg_name
        role_definition_name           = r.role_definition_name
      }
    ]
  ])
}

data "azurerm_eventgrid_domain" "egd" {
  count               = length(local.egds_roles)
  name                = local.egds_roles[count.index].egd_name
  resource_group_name = local.egds_roles[count.index].egd_rg_name

  depends_on = [
    azurerm_eventgrid_domain.egd
  ]
}

data "azurerm_user_assigned_identity" "uai" {
  count               = length(local.egds_roles)
  name                = local.egds_roles[count.index].user_assigned_identity_name
  resource_group_name = local.egds_roles[count.index].user_assigned_identity_rg_name
}

resource "azurerm_role_assignment" "role" {
  count                = length(local.egds_roles)
  scope                = data.azurerm_eventgrid_domain.egd[count.index].id
  role_definition_name = local.egds_roles[count.index].role_definition_name
  principal_id         = data.azurerm_user_assigned_identity.uai[count.index].principal_id
}