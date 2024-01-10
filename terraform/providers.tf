provider "azurerm" {
    # The "feature" block is required for AzureRM provider 2.x.
    # If you're using version 1.x, the "features" block is not allowed.
    features {}
}

terraform {
  required_providers {
    azurerm = {
      version = "2.99.0"
    }
  }
}
