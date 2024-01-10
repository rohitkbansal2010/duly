app_gateways = [
  {
    name         = "duly-p-net-agw"
    location     = "northcentralus"
    rg_name      = "duly-p-agw-rg"
    autoscaling  = "Yes"
    enable_http2 = "disabled"
    sku = {
      name = "WAF_v2"
      tier = "WAF_v2"
    }

    waf = {
      enabled          = true
      firewall_mode    = "Prevention"
      rule_set_type    = "OWASP"
      rule_set_version = 3.2
    }

    waf_disabled_rule_groups = [
      {
        rule_group_name = "REQUEST-920-PROTOCOL-ENFORCEMENT"
        rules = [
          920320,
        ]
      },
      {
        rule_group_name = "REQUEST-942-APPLICATION-ATTACK-SQLI"
        rules = [
          942450,
        ]
      }
    ]

    appgtw_diagnostic_setting_name     = "duly-p-net-agw-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name       = "duly-p-shsvc-loganalytics"
    log_list = [
      "ApplicationGatewayAccessLog",
      "ApplicationGatewayPerformanceLog",
      "ApplicationGatewayFirewallLog"
    ]
    gateway_ip_configurations = [
      {
        name         = "gateway_ip_configuration_01"
        subnet_name  = "duly-p-vnet-agw-snet"
        vnet_name    = "duly-p-vnet"
        vnet_rg_name = "duly-p-vnet-rg"
      }
    ]
    frontend_ip_configurations = [
      {
        name              = "public"
        public_ip_name    = "duly-p-net-agw-pip01"
        public_ip_rg_name = "duly-p-agw-rg"
      }
    ]
    zones = []
    autoscale_configuration = {
      min_capacity = "0"
      max_capacity = "2"
    }
    frontend_ports = [
      {
        name = "Port_443"
        port = "443"
      },
      {
        name = "Port_80"
        port = "80"
      }
    ]
    ssl_certificates = [
      {
        name         = "duly-digital"
        kv_name      = "duly-p-certs-kv"
        kv_rg_name   = "duly-p-sharedsvc-rg"
        kv_cert_name = "duly-wildcard-prd"
      }
    ]
    app_definitions = [
      {
        app_suffix = "collaboration-view"
        backend_address_pool_fqdns = [
          "172.26.41.200"
        ]
        frontend_ip_configuration_name                            = "public"
        frontend_port_name                                        = "Port_443"
        http_listener_host_name                                   = "collaboration-view.duly.digital"
        http_listener_protocol                                    = "https"
        request_routing_rule_type                                 = "Basic"
        backend_http_settings_cookie_based_affinity               = "Disabled"
        backend_http_settings_port                                = "80"
        backend_http_settings_protocol                            = "http"
        backend_http_settings_request_timeout                     = "300"
        backend_http_settings_pick_host_name_from_backend_address = false
        probe_interval                                            = "30"
        probe_protocol                                            = "http"
        probe_timeout                                             = "30"
        probe_unhealthy_threshold                                 = "3"
        probe_path                                                = "/"
        probe_pick_host_name_from_backend_http_settings           = false
        probe_host                                                = "collaboration-view.duly.digital"
        probe_match = {
          "body" = ""
          "status_code" = [
            "200"
          ]
        }
        ssl_certificate_name = "duly-digital"
      },
      {
        app_suffix = "apim"
        backend_address_pool_fqdns = [
          "172.26.40.70"
        ]
        frontend_ip_configuration_name                            = "public"
        frontend_port_name                                        = "Port_443"
        http_listener_host_name                                   = "api.duly.digital"
        http_listener_protocol                                    = "https"
        request_routing_rule_type                                 = "Basic"
        backend_http_settings_cookie_based_affinity               = "Disabled"
        backend_http_settings_port                                = "443"
        backend_http_settings_protocol                            = "https"
        backend_http_settings_request_timeout                     = "300"
        backend_http_settings_pick_host_name_from_backend_address = false
        probe_interval                                            = "30"
        probe_protocol                                            = "https"
        probe_timeout                                             = "30"
        probe_unhealthy_threshold                                 = "3"
        probe_path                                                = "/status-0123456789abcdef"
        probe_pick_host_name_from_backend_http_settings           = false
        probe_host                                                = "api.duly.digital"
        probe_match = {
          "body" = ""
          "status_code" = [
            "200"
          ]
        }
        ssl_certificate_name = "duly-digital"
      },
      {
        app_suffix = "communication-hub"
        backend_address_pool_fqdns = [
          "172.26.41.200"
        ]
        frontend_ip_configuration_name                            = "public"
        frontend_port_name                                        = "Port_443"
        http_listener_host_name                                   = "communication-hub.duly.digital"
        http_listener_protocol                                    = "https"
        request_routing_rule_type                                 = "Basic"
        backend_http_settings_cookie_based_affinity               = "Disabled"
        backend_http_settings_port                                = "80"
        backend_http_settings_protocol                            = "http"
        backend_http_settings_request_timeout                     = "300"
        backend_http_settings_pick_host_name_from_backend_address = false
        probe_interval                                            = "30"
        probe_protocol                                            = "http"
        probe_timeout                                             = "30"
        probe_unhealthy_threshold                                 = "3"
        probe_path                                                = "/"
        probe_pick_host_name_from_backend_http_settings           = false
        probe_host                                                = "communication-hub.duly.digital"
        probe_match = {
          "body" = ""
          "status_code" = [
            "200"
          ]
        }
        ssl_certificate_name = "duly-digital"
      },
      {
        app_suffix = "microsite-services"
        backend_address_pool_fqdns = [
          "172.26.41.200"
        ]
        frontend_ip_configuration_name                            = "public"
        frontend_port_name                                        = "Port_443"
        http_listener_host_name                                   = "engage.duly.digital"
        http_listener_protocol                                    = "https"
        request_routing_rule_type                                 = "Basic"
        backend_http_settings_cookie_based_affinity               = "Disabled"
        backend_http_settings_port                                = "80"
        backend_http_settings_protocol                            = "http"
        backend_http_settings_request_timeout                     = "300"
        backend_http_settings_pick_host_name_from_backend_address = false
        probe_interval                                            = "30"
        probe_protocol                                            = "http"
        probe_timeout                                             = "30"
        probe_unhealthy_threshold                                 = "3"
        probe_path                                                = "/"
        probe_pick_host_name_from_backend_http_settings           = false
        probe_host                                                = "engage.duly.digital"
        probe_match = {
          "body" = ""
          "status_code" = [
            "200"
          ]
        }
        ssl_certificate_name = "duly-digital"
      }
    ]
  }
]

