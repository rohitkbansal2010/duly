keyvaults = [
  {
    name                            = "duly-p-cv-api-enc-kv"
    rg_name                         = "duly-p-collaboration-view-rg"
    sku                             = "standard"
    crossRegionRestore              = "disabled"
    replication                     = "LRS"
    enabled_for_deployment          = true
    enabled_for_disk_encryption     = true
    enabled_for_template_deployment = true
    purge_protection_enabled        = false
    access_policies = [
      {
        application_names = [
          "duly-p-ado-agent",
        ]
        secret_permissions = [
          "get",
          "list",
          "set",
          "delete",
          "purge"
        ]
      },
      {
        application_names = [
          "duly-p-component-pod-id",
        ]
        secret_permissions = [
          "get",
          "list",
        ]
        certificate_permissions = [
          "get",
          "list",
        ]
        key_permissions = [
          "get",
          "list",
        ]
      }
    ]
    secrets = {
      "ApplicationSettings--ADScopes"          = "Encounter.All"
      "Authentication--Instance"               = "https://login.microsoftonline.com/"
      "Authentication--TenantId"               = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "ClinicApiClientOptions--ApiBaseAddress" = "https://api.duly.digital/integration/clinic/v1"
      "NgdpApiClientOptions--ApiBaseAddress"   = "https://api.duly.digital/integration/ngdp/v1/"
      "SiteDataOptions--Sites"                 = <<EOT
      [{"Id":"62889","Address":{ "Line":"1121 South Blvd","City":"Oak Park","State":"Illinois","PostalCode":"60302"}},{"Id":"62789","Address":{"Line":"9233 W. 159th St.","City":"Orland Hills","State":"Illinois","PostalCode":"60487"}},{"Id":"62989","Address":{"Line":"1034 N. Rohlwing Rd.","City":"Addison","State":"Illinois","PostalCode":"60101"}}]
      EOT
    }
    enable_rbac_authorization = false
    network_acls = {
      bypass         = "AzureServices"
      default_action = "Allow"
      ip_rules = [
        "174.128.60.160/32",
        "174.128.60.162/32"
      ]

      subnet_associations = [
        {
          subnet_name = "duly-p-vnet-app-snet"
          vnet_name   = "duly-p-vnet"
          rg_name     = "duly-p-vnet-rg"
        }
      ]
    }
    kv_diagnostic_setting_name         = "duly-p-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name       = "duly-p-shsvc-loganalytics"
    tags = {
      environment         = "PRD"
    }
  },
  {
    name                            = "duly-p-cv-api-res-kv"
    rg_name                         = "duly-p-collaboration-view-rg"
    sku                             = "standard"
    crossRegionRestore              = "disabled"
    replication                     = "LRS"
    enabled_for_deployment          = true
    enabled_for_disk_encryption     = true
    enabled_for_template_deployment = true
    purge_protection_enabled        = false
    access_policies = [
      {
        application_names = [
          "duly-p-ado-agent",
        ]
        secret_permissions = [
          "get",
          "list",
          "set",
          "delete",
          "purge"
        ]
      },
      {
        application_names = [
          "duly-p-component-pod-id",
        ]
        secret_permissions = [
          "get",
          "list",
        ]
        certificate_permissions = [
          "get",
          "list",
        ]
        key_permissions = [
          "get",
          "list",
        ]
      }
    ]
    secrets = {
      "ApplicationSettings--ADScopes"                          = "Resources.All"
      "Authentication--Instance"                               = "https://login.microsoftonline.com/"
      "Authentication--TenantId"                               = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "ConnectionStrings--CollaborationViewMetadataConnection" = ""
    }
    enable_rbac_authorization = false
    network_acls = {
      bypass         = "AzureServices"
      default_action = "Allow"
      ip_rules = [
        "174.128.60.160/32",
        "174.128.60.162/32"
      ]

      subnet_associations = [
        {
          subnet_name = "duly-p-vnet-app-snet"
          vnet_name   = "duly-p-vnet"
          rg_name     = "duly-p-vnet-rg"
        }
      ]
    }
    kv_diagnostic_setting_name         = "duly-p-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name       = "duly-p-shsvc-loganalytics"
    tags = {
      environment         = "PRD"
    }
  }
]