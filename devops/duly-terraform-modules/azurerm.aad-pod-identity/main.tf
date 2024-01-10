# the manifest creates a managed identity for aad_pod_identity 
resource "azurerm_user_assigned_identity" "uai" {
  name                = var.name
  resource_group_name = var.rg_name
  location            = var.location

  tags = var.tags
}

# create azure identity binding for var.aks_name
resource "helm_release" "azure_identity" {
  name  = "aad-pod-identity-${var.name}"
  chart = var.chart_path
  set {
    name  = "pod_identity.IDENTITY_RESOURCE_ID"
    value = azurerm_user_assigned_identity.uai.id
  }
  set {
    name  = "pod_identity.IDENTITY_CLIENT_ID"
    value = azurerm_user_assigned_identity.uai.client_id
  }
  set {
    name  = "pod_identity.name"
    value = var.name
  }
}

data "azurerm_client_config" "current" {}

data "azurerm_key_vault" "kv" {
  count               = length(var.baseline_keyvaults)
  name                = var.baseline_keyvaults[count.index].name
  resource_group_name = var.baseline_keyvaults[count.index].rg_name
}


resource "azurerm_key_vault_access_policy" "access_policy" {
  count                   = length(var.baseline_keyvaults)
  key_vault_id            = data.azurerm_key_vault.kv[count.index].id
  tenant_id               = azurerm_user_assigned_identity.uai.tenant_id
  object_id               = azurerm_user_assigned_identity.uai.principal_id
  key_permissions         = var.baseline_keyvaults[count.index].key_permissions
  secret_permissions      = var.baseline_keyvaults[count.index].secret_permissions
  certificate_permissions = var.baseline_keyvaults[count.index].certificate_permissions

  depends_on = [
    azurerm_user_assigned_identity.uai
  ]
}
