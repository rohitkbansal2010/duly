# Set provider properties
provider "azurerm" {
  subscription_id = "#{ENV_AZURE_SUBSCRIPTION_ID}#"
  client_secret   = "#{ENV_AZURE_CLIENT_SECRET}#"
  client_id       = "#{ENV_AZURE_CLIENT_ID}#"
  tenant_id       = "#{ENV_AZURE_TENANT_ID}#"
  features {
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
}

provider "azuread" {
  client_secret = "#{ENV_AZUREAD_CLIENT_SECRET}#"
  client_id     = "#{ENV_AZUREAD_CLIENT_ID}#"
  tenant_id     = "#{ENV_AZUREAD_TENANT_ID}#"
}

terraform {
  required_providers {
    azurerm = {
      version = "2.99.0"
    }
  }
}