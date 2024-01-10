locals {
  countindex      = range(0, length(var.secrets), 1)
  indexed_secrets = zipmap(local.countindex, var.secrets)

  # define a map - consumer for redis connections string
  redis_secrets = {
    for i, v in local.indexed_secrets : i => {
      resource_name    = v.resource_name
      resource_rg_name = v.resource_rg_name
      secret_attribute = v.secret_attribute
      keyvaults        = v.keyvaults
    } if v.resource_type == "redis_cache"
  }

  # define a map - sonsumer for app reg client secret
  client_secrets = {
    for i, v in local.indexed_secrets : i => {
      resource_name    = v.resource_name
      secret_attribute = v.secret_attribute
      keyvaults        = v.keyvaults
    } if v.secret_attribute == "client_secret" && v.resource_type == "azuread_application"
  }

  # define a map - consumer for app reg application id
  application_ids = {
    for i, v in local.indexed_secrets : i => {
      resource_name    = v.resource_name
      secret_attribute = v.secret_attribute
      keyvaults        = v.keyvaults
    } if v.secret_attribute == "application_id" && v.resource_type == "azuread_application"
  }
}

# begin populate of redis connection string
data "azurerm_redis_cache" "rc" {
  for_each            = local.redis_secrets
  name                = each.value.resource_name
  resource_group_name = each.value.resource_rg_name
}

module "redis_secrets" {
  for_each     = local.redis_secrets
  source       = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.secret-to-many-kv?ref=feature/one_secret_to_many_keyvaults"
  secret_value = each.value.secret_attribute == "secondary_connection_string" ? data.azurerm_redis_cache.rc[each.key].secondary_connection_string : ""
  secrets      = each.value.keyvaults
}
# end populate of redis connection string

# begin populate of app reg client secret
data "azuread_application" "app" {
  for_each     = local.client_secrets
  display_name = each.value.resource_name
}

resource "azuread_application_password" "client_secret" {
  for_each              = local.client_secrets
  display_name          = each.value.resource_name
  application_object_id = data.azuread_application.app[each.key].object_id
}

module "client_secrets" {
  for_each     = local.client_secrets
  source       = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.secret-to-many-kv?ref=feature/one_secret_to_many_keyvaults"
  secret_value = azuread_application_password.client_secret[each.key].value
  secrets      = each.value.keyvaults
}
# end populate of redis reg client secret

# begin populate of app reg application id
data "azuread_application" "app_id" {
  for_each     = local.application_ids
  display_name = each.value.resource_name
}

module "application_ids" {
  for_each     = local.application_ids
  source       = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.secret-to-many-kv?ref=feature/one_secret_to_many_keyvaults"
  secret_value = data.azuread_application.app_id[each.key].application_id
  secrets      = each.value.keyvaults
}
# begin populate of app reg application id