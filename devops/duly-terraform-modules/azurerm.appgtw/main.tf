# Get current subscription data
data "azurerm_subscription" "current" {}

# Create user assigned identity
resource "azurerm_user_assigned_identity" "identity" {
  count               = var.user_assigned_identity == true ? 1 : 0
  resource_group_name = var.resource_group_name
  location            = var.location
  name                = "id-${var.name}"
}

# Get key vault data
data "azurerm_key_vault" "kv" {
  count               = length(var.ssl_certificates)
  name                = var.ssl_certificates[count.index].kv_name
  resource_group_name = lookup(var.ssl_certificates[count.index], "kv_rg_name", var.resource_group_name)
}

# Set key vault access policy for previusly created user assigned identity
resource "azurerm_key_vault_access_policy" "cert_access" {
  count                   = length(var.ssl_certificates)
  key_vault_id            = data.azurerm_key_vault.kv[count.index].id
  tenant_id               = data.azurerm_subscription.current.tenant_id
  object_id               = azurerm_user_assigned_identity.identity[0].principal_id
  certificate_permissions = ["Get"]
  secret_permissions      = ["Get"]
}

# Get certificate data from key vault
data "azurerm_key_vault_certificate" "ssl_certificate" {
  count        = length(var.ssl_certificates)
  depends_on   = [azurerm_key_vault_access_policy.cert_access]
  name         = var.ssl_certificates[count.index].kv_cert_name
  key_vault_id = data.azurerm_key_vault.kv[count.index].id
}

# Get subnet to associate with Azure firewall
data "azurerm_subnet" "subnet_associate" {
  count                = length(var.gateway_ip_configurations)
  name                 = lookup(var.gateway_ip_configurations[count.index], "subnet_name", "AzureFirewallSubnet")
  virtual_network_name = var.gateway_ip_configurations[count.index].vnet_name
  resource_group_name  = lookup(var.gateway_ip_configurations[count.index], "vnet_rg_name", var.resource_group_name)
}

# Get public IP data
data "azurerm_public_ip" "public_ip" {
  count               = length(var.frontend_ip_configurations)
  name                = var.frontend_ip_configurations[count.index].public_ip_name
  resource_group_name = lookup(var.frontend_ip_configurations[count.index], "public_ip_rg_name", var.resource_group_name)
}

# Cretate application gateway
resource "azurerm_application_gateway" "app_gtw" {
  name                = var.name
  resource_group_name = var.resource_group_name
  location            = var.location
  zones               = var.zones
  enable_http2        = var.enable_http2
  tags                = var.tags

  sku {
    name     = lookup(var.sku, "name")
    tier     = lookup(var.sku, "tier")
    capacity = lookup(var.sku, "capacity", null)
  }

  waf_configuration {
    enabled          = lookup(var.waf, "enabled")
    firewall_mode    = lookup(var.waf, "firewall_mode")
    rule_set_type    = lookup(var.waf, "rule_set_type")
    rule_set_version = lookup(var.waf, "rule_set_version")

    dynamic "disabled_rule_group" {
      for_each = var.waf_disabled_rule_groups
      content {
        rule_group_name = disabled_rule_group.value.rule_group_name
        rules           = disabled_rule_group.value.rules
      }
    }

  }

  ssl_policy {
    disabled_protocols = [
      "TLSv1_0", 
      "TLSv1_1"
    ]
  }

  dynamic "autoscale_configuration" {
    for_each = var.autoscaling != null ? [1] : []
    content {
      min_capacity = lookup(var.autoscale_configuration, "min_capacity")
      max_capacity = lookup(var.autoscale_configuration, "max_capacity")
    }
  }

  dynamic "identity" {
    for_each = var.user_assigned_identity == true ? [1] : []
    content {
      type         = "UserAssigned"
      identity_ids = [azurerm_user_assigned_identity.identity[0].id]
    }
  }

  dynamic "gateway_ip_configuration" {
    for_each = var.gateway_ip_configurations
    content {
      name      = gateway_ip_configuration.value["name"]
      subnet_id = data.azurerm_subnet.subnet_associate[index(var.gateway_ip_configurations, gateway_ip_configuration.value)].id
    }
  }

  dynamic "frontend_port" {
    for_each = var.frontend_ports
    content {
      name = frontend_port.value["name"]
      port = frontend_port.value["port"]
    }
  }

  dynamic "frontend_ip_configuration" {
    for_each = var.frontend_ip_configurations
    content {
      name                 = frontend_ip_configuration.value["name"]
      public_ip_address_id = data.azurerm_public_ip.public_ip[index(var.frontend_ip_configurations, frontend_ip_configuration.value)].id
    }
  }

  dynamic "ssl_certificate" {
    for_each = var.ssl_certificates
    content {
      name                = ssl_certificate.value["name"]
      data                = lookup(ssl_certificate.value, "data", null)
      password            = lookup(ssl_certificate.value, "password", null)
      key_vault_secret_id = lookup(data.azurerm_key_vault_certificate.ssl_certificate[index(var.ssl_certificates, ssl_certificate.value)], "secret_id", null)
    }
  }

  # The following dynamic blocks intended to use configurations from 'app_definitions' variable. It only supports request routing rules with defined backend.
  # To set request routing rules with different configuration type, create one more instance of the following blocks and configure new variable with appropriate configuration.
  # Investigation required
  dynamic "backend_address_pool" {
    for_each = var.app_definitions
    content {
      name         = "${backend_address_pool.value["app_suffix"]}-apbp"
      fqdns        = lookup(backend_address_pool.value, "backend_address_pool_fqdns", null)
      ip_addresses = lookup(backend_address_pool.value, "backend_address_pool_ip_addresses", [])
    }
  }

  dynamic "backend_http_settings" {
    for_each = var.app_definitions
    content {
      name                                = "${backend_http_settings.value["app_suffix"]}-apht"
      cookie_based_affinity               = backend_http_settings.value["backend_http_settings_cookie_based_affinity"]
      affinity_cookie_name                = lookup(backend_http_settings.value, "backend_http_settings_affinity_cookie_name", null)
      path                                = lookup(backend_http_settings.value, "backend_http_settings_path", null)
      port                                = backend_http_settings.value["backend_http_settings_port"]
      probe_name                          = "${backend_http_settings.value["app_suffix"]}-aphp"
      protocol                            = backend_http_settings.value["backend_http_settings_protocol"]
      request_timeout                     = backend_http_settings.value["backend_http_settings_request_timeout"]
      host_name                           = lookup(backend_http_settings.value, "backend_http_settings_host_name", null)
      pick_host_name_from_backend_address = tobool(lower(lookup(backend_http_settings.value, "backend_http_settings_pick_host_name_from_backend_address", "false")))
      trusted_root_certificate_names      = lookup(backend_http_settings.value, "backend_http_settings_trusted_root_certificate_names", [])

      dynamic "connection_draining" {
        for_each = lookup(backend_http_settings.value, "backend_http_settings_connection_draining", null) != null ? [1] : []
        content {
          enabled           = tobool(lower(lookup(backend_http_settings.value.backend_http_settings_connection_draining, "enabled")))
          drain_timeout_sec = lookup(backend_http_settings.value.backend_http_settings_connection_draining, "drain_timeout_sec")
        }
      }

      dynamic "authentication_certificate" {
        for_each = lookup(backend_http_settings.value, "backend_http_settings_authentication_certificate", [])
        content {
          name = authentication_certificate.value
        }
      }
    }
  }

  dynamic "http_listener" {
    for_each = var.app_definitions
    content {
      name                           = "${http_listener.value["app_suffix"]}-https-apls"
      frontend_ip_configuration_name = http_listener.value["frontend_ip_configuration_name"]
      frontend_port_name             = http_listener.value["frontend_port_name"]
      host_name                      = http_listener.value["http_listener_host_name"]
      host_names                     = lookup(http_listener.value, "http_listener_host_names", [])
      protocol                       = http_listener.value["http_listener_protocol"]
      require_sni                    = lookup(http_listener.value, "http_listener_require_sni", null)
      ssl_certificate_name           = lookup(http_listener.value, "ssl_certificate_name", null)
      firewall_policy_id             = lookup(http_listener.value, "firewall_policy_id", null)

      # # Uncommenting this dynamic block will fail all dynamic blocks in module!
      # dynamic "custom_error_configuration" {
      #   for_each                     = lookup(http_listener.value, "custom_error_configuration", [])
      #   content {
      #     status_code                = custom_error_configuration.value["status_code"]
      #     custom_error_page_url      = custom_error_configuration.value["custom_error_page_url"]
      #   }
      # }
    }
  }

  dynamic "request_routing_rule" {
    for_each = var.app_definitions
    content {
      name                       = "${request_routing_rule.value["app_suffix"]}-aprl"
      rule_type                  = lookup(request_routing_rule.value, "request_routing_rule_type", "Basic")
      http_listener_name         = "${request_routing_rule.value["app_suffix"]}-https-apls"
      backend_address_pool_name  = "${request_routing_rule.value["app_suffix"]}-apbp"
      backend_http_settings_name = "${request_routing_rule.value["app_suffix"]}-apht"
    }
  }

  dynamic "probe" {
    for_each = var.app_definitions
    content {
      name                                      = "${probe.value["app_suffix"]}-aphp"
      interval                                  = probe.value["probe_interval"]
      protocol                                  = probe.value["probe_protocol"]
      path                                      = probe.value["probe_path"]
      timeout                                   = probe.value["probe_timeout"]
      unhealthy_threshold                       = probe.value["probe_unhealthy_threshold"]
      host                                      = lookup(probe.value, "probe_host", null)
      port                                      = lookup(probe.value, "probe_port", null)
      pick_host_name_from_backend_http_settings = tobool(lower(lookup(probe.value, "probe_pick_host_name_from_backend_http_settings", "false")))
      minimum_servers                           = lookup(probe.value, "probe_backend_adminimum_serversdress_pool_name", null)

      dynamic "match" {
        for_each = lookup(probe.value, "probe_match", null) != null ? [1] : []
        content {
          body        = lookup(probe.value.probe_match, "body")
          status_code = lookup(probe.value.probe_match, "status_code")
        }
      }
    }
  }

  # Configure redirection from Port_80 to Port_443
  dynamic "http_listener" {
    for_each = var.app_definitions
    content {
      name                           = "${http_listener.value["app_suffix"]}-http-apls"
      frontend_ip_configuration_name = http_listener.value["frontend_ip_configuration_name"]
      frontend_port_name             = lookup(http_listener.value, "second_http_listener_frontend_port_name", "Port_80")
      protocol                       = lookup(http_listener.value, "second_http_listener_protocol", "http")
      host_name                      = http_listener.value["http_listener_host_name"]
    }
  }
  # Configure redirection from Port_80 to Port_443
  dynamic "redirect_configuration" {
    for_each = var.app_definitions
    content {
      name                 = "${redirect_configuration.value["app_suffix"]}-aprd"
      redirect_type        = lookup(redirect_configuration.value, "redirect_type", "Permanent")
      target_listener_name = "${redirect_configuration.value["app_suffix"]}-https-apls"
      include_path         = tobool(lower(lookup(redirect_configuration.value, "include_path", "true")))
      include_query_string = tobool(lower(lookup(redirect_configuration.value, "include_query_string", "true")))
    }
  }
  # Configure redirection from Port_80 to Port_443
  dynamic "request_routing_rule" {
    for_each = var.app_definitions
    content {
      name                        = "${request_routing_rule.value["app_suffix"]}-redirect-aprl"
      rule_type                   = lookup(request_routing_rule.value, "request_routing_rule_type", "Basic")
      http_listener_name          = "${request_routing_rule.value["app_suffix"]}-http-apls"
      redirect_configuration_name = "${request_routing_rule.value["app_suffix"]}-aprd"
    }
  }
}

# Manages a diagnostic setting for created recovery vault
resource "azurerm_monitor_diagnostic_setting" "app_gtw" {
  name                       = var.appgtw_diagnostic_setting_name
  target_resource_id         = azurerm_application_gateway.app_gtw.id
  log_analytics_workspace_id = var.log_analytics_workspace_id

  dynamic "log" {
    for_each = { for log in var.log_list : "log-${log}" => log }
    content {
      category = log.value
      enabled  = true

      retention_policy {
        enabled = true
      }
    }
  }

  # metric {
  #   category = "AllMetrics"

  #   retention_policy {
  #     enabled = false
  #   }
  # }
}
