# version set
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

  depends_on = [
    azurerm_api_management_api_version_set.api-vs
  ]
}

# common validate-jwt policy
resource "azurerm_api_management_api_policy" "api_policy" {
  api_name            = var.api_name
  api_management_name = var.apim_name
  resource_group_name = var.apim_rg_name

  xml_content = file("./policy-#{ENV_TF_STATE_FOLDER}#.xml")

  depends_on = [
    azurerm_api_management_api.api
  ]
}

# create operations from the list
resource "azurerm_api_management_api_operation" "operation" {
  count               = length(var.operations)
  api_name            = var.api_name
  api_management_name = var.apim_name
  resource_group_name = var.apim_rg_name
  operation_id        = var.operations[count.index].operation_id
  display_name        = var.operations[count.index].display_name
  method              = var.operations[count.index].method
  url_template        = var.operations[count.index].url_template
  description         = var.operations[count.index].description
  depends_on = [
    azurerm_api_management_api.api
  ]
}

# per operation policy
# looks for operation_id.xml file
resource "azurerm_api_management_api_operation_policy" "operation_policy" {
  count               = length(var.operations)
  api_name            = var.api_name
  api_management_name = var.apim_name
  resource_group_name = var.apim_rg_name
  operation_id        = var.operations[count.index].operation_id
  xml_content         = file("./${var.operations[count.index].operation_id}.xml")
  depends_on = [
    azurerm_api_management_api_operation.operation
  ]
}


