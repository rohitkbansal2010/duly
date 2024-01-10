data "terraform_remote_state" "acr" {
  count   = length(var.uais)
  backend = "azurerm"
  config = {
    container_name       = var.prod_backend_container_name
    key                  = var.uais[count.index].acr_state_path
    resource_group_name  = var.prod_backend_resource_group_name
    storage_account_name = var.prod_backend_storage_account_name
    subscription_id      = var.prod_backend_subscription_id
    client_secret        = var.prod_backend_client_secret
    client_id            = var.prod_backend_client_id
    tenant_id            = var.prod_backend_tenant_id
  }
}

data "azurerm_subnet" "subnet" {
  count                = length(var.uais)
  name                 = var.uais[count.index].subnet_name
  virtual_network_name = var.uais[count.index].vnet_name
  resource_group_name  = var.uais[count.index].vnet_rg_name
}

data "azurerm_private_dns_zone" "private_dns" {
  count               = length(var.uais)
  name                = var.uais[count.index].private_dns_zone_name
  resource_group_name = var.uais[count.index].rg_name
}

resource "azurerm_user_assigned_identity" "uai" {
  count               = length(var.uais)
  name                = var.uais[count.index].name
  resource_group_name = var.uais[count.index].rg_name
  location            = var.uais[count.index].location
}

resource "azurerm_role_assignment" "uai_dns_contributor" {
  count                = length(var.uais)
  scope                = data.azurerm_private_dns_zone.private_dns[count.index].id
  role_definition_name = "Private DNS Zone Contributor"
  principal_id         = azurerm_user_assigned_identity.uai[count.index].principal_id
  depends_on = [
    azurerm_user_assigned_identity.uai
  ]
}

resource "azurerm_role_assignment" "uai_network_contributor" {
  count                = length(var.uais)
  scope                = data.azurerm_subnet.subnet[count.index].id
  role_definition_name = "Network Contributor"
  principal_id         = azurerm_user_assigned_identity.uai[count.index].principal_id
  depends_on = [
    azurerm_user_assigned_identity.uai
  ]
}

###################################### make generic #################################
data "azurerm_route_table" "udr" {
  count               = length(var.uais)
  name                = var.uais[count.index].udr_name
  resource_group_name = var.uais[count.index].udr_rg_name
}

resource "azurerm_role_assignment" "uai_network_contributor_udr" {
  count                = length(var.uais)
  scope                = data.azurerm_route_table.udr[count.index].id
  role_definition_name = "Network Contributor"
  principal_id         = azurerm_user_assigned_identity.uai[0].principal_id
  depends_on = [
    azurerm_user_assigned_identity.uai
  ]
}

resource "azurerm_role_assignment" "uai_cr_acrpull" {
  count                = length(var.uais)
  scope                = data.terraform_remote_state.acr[0].outputs["acr_id"]
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.uai[count.index].principal_id
  depends_on = [
    azurerm_user_assigned_identity.uai
  ]
}