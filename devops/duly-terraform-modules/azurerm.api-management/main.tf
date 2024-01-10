data "azurerm_resource_group" "apim_rg" {
  name = var.rg_name
}

data "azurerm_subnet" "apim_subnet" {
  name                 = var.subnet_name
  virtual_network_name = var.vnet_name
  resource_group_name  = var.vnet_rg_name
}

data "azurerm_application_insights" "app_insights" {
  name                = var.app_insights_name
  resource_group_name = var.app_insights_rg_name
}

data "azurerm_key_vault" "kv" {
  name                = var.kv_name
  resource_group_name = var.kv_rg_name
}

data "azurerm_client_config" "current" {}

resource "azurerm_api_management" "apim" {
  name                 = var.apim_name
  location             = data.azurerm_resource_group.apim_rg.location
  resource_group_name  = data.azurerm_resource_group.apim_rg.name
  publisher_name       = var.publisher_name
  publisher_email      = var.publisher_email
  sku_name             = var.sku_name
  virtual_network_type = "Internal"

  virtual_network_configuration {
    subnet_id = data.azurerm_subnet.apim_subnet.id
  }
  #   lifecycle {
  #     ignore_changes = [virtual_network_configuration]
  #   }

  identity {
    type = "SystemAssigned"
  }
  protocols {
    enable_http2 = var.enable_http2
  }

  dynamic "policy" {
    for_each = var.policy_configuration
    content {
      xml_content = file(lookup(policy.value, "xml_content", null))
      xml_link    = lookup(policy.value, "xml_link", null)
    }
  }

  dynamic "security" {
    for_each = var.security_configuration
    content {
      enable_backend_ssl30      = lookup(security.value, "enable_backend_ssl30", false)
      enable_backend_tls10      = lookup(security.value, "enable_backend_tls10", false)
      enable_backend_tls11      = lookup(security.value, "enable_backend_tls11", false)
      enable_frontend_ssl30     = lookup(security.value, "enable_frontend_ssl30", false)
      enable_frontend_tls10     = lookup(security.value, "enable_frontend_tls10", false)
      enable_frontend_tls11     = lookup(security.value, "enable_frontend_tls11", false)
      enable_triple_des_ciphers = lookup(security.value, "enable_triple_des_ciphers", false)
    }
  }

  notification_sender_email = var.notification_sender_email
}
resource "azurerm_api_management_custom_domain" "hostname_configuration" {
  api_management_id = azurerm_api_management.apim.id

  # dynamic "hostname_configuration" {
  #   for_each = length(concat(
  #     var.management_hostname_configuration,
  #     var.portal_hostname_configuration,
  #     var.developer_portal_hostname_configuration,
  #     var.proxy_hostname_configuration,
  #   )) == 0 ? [] : [1]

  #   content {
  dynamic "management" {
    for_each = var.management_hostname_configuration
    content {
      host_name                    = lookup(management.value, "host_name")
      key_vault_id                 = lookup(management.value, "key_vault_id", null)
      certificate                  = lookup(management.value, "certificate", null)
      certificate_password         = lookup(management.value, "certificate_password", null)
      negotiate_client_certificate = lookup(management.value, "negotiate_client_certificate", false)
    }
  }

  dynamic "portal" {
    for_each = var.portal_hostname_configuration
    content {
      host_name                    = lookup(portal.value, "host_name")
      key_vault_id                 = lookup(portal.value, "key_vault_id", null)
      certificate                  = lookup(portal.value, "certificate", null)
      certificate_password         = lookup(portal.value, "certificate_password", null)
      negotiate_client_certificate = lookup(portal.value, "negotiate_client_certificate", false)
    }
  }

  dynamic "developer_portal" {
    for_each = var.developer_portal_hostname_configuration
    content {
      host_name                    = lookup(developer_portal.value, "host_name")
      key_vault_id                 = lookup(developer_portal.value, "key_vault_id", null)
      certificate                  = lookup(developer_portal.value, "certificate", null)
      certificate_password         = lookup(developer_portal.value, "certificate_password", null)
      negotiate_client_certificate = lookup(developer_portal.value, "negotiate_client_certificate", false)
    }
  }

  dynamic "proxy" {
    for_each = var.proxy_hostname_configuration
    content {
      host_name                    = lookup(proxy.value, "host_name")
      default_ssl_binding          = lookup(proxy.value, "default_ssl_binding", false)
      key_vault_id                 = lookup(proxy.value, "key_vault_id", null)
      certificate                  = lookup(proxy.value, "certificate", null)
      certificate_password         = lookup(proxy.value, "certificate_password", null)
      negotiate_client_certificate = lookup(proxy.value, "negotiate_client_certificate", false)
    }
  }

  dynamic "scm" {
    for_each = var.scm_hostname_configuration
    content {
      host_name                    = lookup(scm.value, "host_name")
      key_vault_id                 = lookup(scm.value, "key_vault_id", null)
      certificate                  = lookup(scm.value, "certificate", null)
      certificate_password         = lookup(scm.value, "certificate_password", null)
      negotiate_client_certificate = lookup(scm.value, "negotiate_client_certificate", false)
    }
  }

  depends_on = [
    azurerm_key_vault_access_policy.kv_apim_policy
  ]
}


resource "azurerm_key_vault_access_policy" "kv_apim_policy" {
  key_vault_id = data.azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_api_management.apim.identity[0].principal_id

  secret_permissions = [
    "get",
    "list"
  ]

  certificate_permissions = [
    "get",
    "list"
  ]
  depends_on = [
    azurerm_api_management.apim
  ]
}

resource "azurerm_api_management_named_value" "nv" {
  for_each            = var.named_values
  name                = each.key
  display_name        = each.key
  value               = each.value
  resource_group_name = var.rg_name
  api_management_name = var.apim_name
  
  depends_on = [
    azurerm_api_management.apim
  ]
}

resource "azurerm_api_management_logger" "log" {
  name                = "${var.apim_name}-log"
  api_management_name = var.apim_name
  resource_group_name = var.rg_name

  application_insights {
    instrumentation_key = data.azurerm_application_insights.app_insights.instrumentation_key
  }

  depends_on = [
    azurerm_api_management.apim
  ]
}