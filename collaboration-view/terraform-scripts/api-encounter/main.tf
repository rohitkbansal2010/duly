# the module requires swagger.json and policy.xml
# to be in the current directory to provision an API

resource "azurerm_api_management_api_version_set" "api-vs" {
  name                = var.version_set_name
  resource_group_name = var.apim_rg_name
  api_management_name = var.apim_name
  display_name        = var.version_set_display_name
  versioning_scheme   = "Segment"
}

resource "azurerm_api_management_api" "api" {
  name                  = var.api_name
  display_name          = var.api_display_name
  resource_group_name   = var.apim_rg_name
  api_management_name   = var.apim_name
  revision              = "1" # changing this forces the version set destroying, recreation fails
  subscription_required = var.subscription_required
  service_url           = var.api_web_service_url
  version               = "v1"
  version_set_id        = azurerm_api_management_api_version_set.api-vs.id
  path                  = var.api_url_suffix
  protocols             = ["https"]
  description           = var.api_description

  subscription_key_parameter_names {
    header = "subscription-key"
    query  = "subscription-key"
  }

  dynamic "oauth2_authorization" {
    for_each = var.oauth2_server_name == null ? [] : [1]
    content {
      authorization_server_name = var.oauth2_server_name
      scope                     = var.oauth2_override_scope
    }
  }
  import {
    content_format = "openapi+json"
    content_value  = file("./swagger.json")
  }

  depends_on = [
    azurerm_api_management_api_version_set.api-vs
  ]
}

resource "azurerm_api_management_api_policy" "api_policy" {
  api_name            = azurerm_api_management_api.api.name
  api_management_name = var.apim_name
  resource_group_name = var.apim_rg_name

  xml_content = file("./policy-#{ENV_TF_STATE_FOLDER}#.xml")

  depends_on = [
    azurerm_api_management_api.api
  ]
}

data "azurerm_api_management_product" "cv-apis" {
  count               = length(var.api_product_ids)
  product_id          = var.api_product_ids[count.index]
  api_management_name = var.apim_name
  resource_group_name = var.apim_rg_name
}

resource "azurerm_api_management_product_api" "cv-apis-product" {
  count               = length(var.api_product_ids)
  api_name            = azurerm_api_management_api.api.name
  product_id          = data.azurerm_api_management_product.cv-apis[count.index].product_id
  api_management_name = var.apim_name
  resource_group_name = var.apim_rg_name
}
