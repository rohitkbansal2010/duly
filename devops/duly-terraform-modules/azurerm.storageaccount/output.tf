output "storage_account_name" {
  description = "Name of the storage account."
  value = azurerm_storage_account.storage.name
}

output "storage_account_id" {
  description = "Id of the storage account."
  value = azurerm_storage_account.storage.id 
}

output "primary_blob_endpoint" {
  description = "The endpoint URL for blob storage in the primary location."
  value = azurerm_storage_account.storage.primary_blob_endpoint
}

output "primary_access_key" {
  value = azurerm_storage_account.storage.primary_access_key
}