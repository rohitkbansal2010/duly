keyvaults = [
  {
    name                            = "duly-p-int-api-clinic-kv"
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
      "Audit--ServiceBusNamespace"                   = "duly-p-app-ns.servicebus.windows.net"
      "Audit--ServiceBusQueue"                       = "p-integration-clinic-audit-sbq"
      "Authentication--Instance"                     = "https://login.microsoftonline.com/"
      "Authentication--TenantId"                     = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "DulyRestApiClient--VaultKeyIdUrl"             = "https://duly-p-certs-kv.vault.azure.net/keys/digital-signing-privkey/ebe4439eb8e1454bb31f5db2baf0b1c5"
      "Fhir--ClientId"                               = "b8cf1cf5-164a-4db8-96d2-3d87dccc45ba"
      "Fhir--TokenUrl"                               = "https://fhir.eehealth.org/fhirtst/oauth2/token"
      "FhirServerUrls--BaseUrlR4"                    = "https://fhir.eehealth.org/fhirtst/api/FHIR/R4"
      "FhirServerUrls--BaseUrlSTU3"                  = "https://fhir.eehealth.org/fhirtst/api/FHIR/STU3"
      "PrivateApiOptions--ApiBaseAddress"            = ""
      "PrivateApiOptions--ApiClientId"               = ""
      "PrivateApiOptions--ApiPassword"               = ""
      "PrivateApiOptions--ApiUsername"               = ""
      "PrivateApiOptions--BypassAppointmentCreation" = "false"
      "PrivateApiOptions--CertificateKeyVaultUrl"    = "https://duly-p-certs-kv.vault.azure.net/"
      "PrivateApiOptions--CertificateName"           = "digital-p-epic-bridge-v1-cert"
      "ServiceBusOptions--FullyQualifiedNamespace"   = "duly-p-app-ns.servicebus.windows.net"
      "ServiceBusOptions--QueueName"                 = "p-integration-clinic-audit-sbq"
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
      environment = "PRD"
    }
  },
  {
    name                            = "duly-p-int-api-ngdp-kv"
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
      "Authentication--Instance"          = "https://login.microsoftonline.com/"
      "Authentication--TenantId"          = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "ConnectionStrings--NgdpConnection" = ""
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
      environment = "PRD"
    }
  },
  {
    name                            = "duly-p-clinic-audit-kv"
    rg_name                         = "duly-p-clinic-audit-rg"
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
      "LogAnalyticsClientOptions--SharedKey" = ""
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
      environment = "PRD"
    }
  }
]