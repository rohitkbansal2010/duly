locals {
  storage_names = [
    for storage in module.storage_account :
    storage.storage_account_name
  ]
  storage_ids = [
    for storage in module.storage_account :
    storage.storage_account_id
  ]
  primary_blob_endpoints = [
    for endpoint in module.storage_account :
    endpoint.primary_blob_endpoint
  ]

  primary_access_keys = [
    for storage in module.storage_account :
    storage.primary_access_key
  ]
}

output "storage_account_name" {
  value = local.storage_names
}

output "storage_account_id" {
  value = local.storage_ids
}

output "primary_blob_endpoint" {
  value = local.primary_blob_endpoints
}

# output "primary_access_key" {
#   value = local.primary_access_keys
# }

