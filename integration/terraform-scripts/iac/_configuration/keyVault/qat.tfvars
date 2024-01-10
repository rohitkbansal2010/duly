keyvaults = [
  {
    name                            = "duly-q-int-api-clinic-kv"
    rg_name                         = "duly-q-collaboration-view-rg"
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
          "duly-d-ado-agent",
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
          "duly-q-component-pod-id",
          "duly-d-component-pod-id",
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
      },
      {
        user_principal_names = [
          "Syarhei.Prudnikaw@boncura.com",
          "Evgeny.Polyarush@boncura.com",
          "Mikhail.Zvinchuk@boncura.com",
          "Efim.Shats@boncura.com",
          "Aleksandr.Soloshenko@boncura.com",
          "Oleg.Kvashnin@boncura.com",
          "Aleksandr.Trutnev@boncura.com"
        ]
        secret_permissions = [
          "get",
          "list",
          "set",
          "delete",
          "recover",
          "backup",
          "restore",
          "purge"
        ]
        key_permissions = [
          "get",
          "list",
          "update",
          "create",
          "import",
          "delete",
          "recover",
          "backup",
          "restore",
          "decrypt",
          "encrypt",
          "wrapkey",
          "unwrapkey",
          "verify",
          "sign",
          "purge"
        ]
      },
      {
        group_names = [
          "EPAM-ADM-TEAM",
          "EPAM-DEV-GRP"
        ]
        secret_permissions = [
          "get",
          "list",
          "set"
        ]
      }
    ]
    secrets = {
      "Audit--ServiceBusNamespace"                    = "duly-d-app-ns.servicebus.windows.net"
      "Audit--ServiceBusQueue"                        = "q-integration-clinic-audit-sbq"
      "Authentication--ClientId"                      = ""
      "Authentication--Instance"                      = ""
      "Authentication--TenantId"                      = ""
      "DulyRestApiClient--VaultKeyIdUrl"              = ""
      "Fhir--ClientId"                                = ""
      "Fhir--TokenUrl"                                = ""
      "FhirServerUrls--BaseUrlR4"                     = ""
      "FhirServerUrls--BaseUrlSTU3"                   = ""
      "PrivateApiOptions--ApiBaseAddress"             = "https://epicproxy-np.et1296.epichosted.com/"
      "PrivateApiOptions--ApiClientId"                = "ef6de80d-1de6-4e75-acbf-46abd721ce6b"
      "PrivateApiOptions--ApiPassword"                = ""
      "PrivateApiOptions--ApiUsername"                = ""
      "PrivateApiOptions--BypassAppointmentCreation"  = "true"
      "PrivateApiOptions--CertificateKeyVaultUrl"     = "https://duly-d-certs-kv.vault.azure.net/"
      "PrivateApiOptions--CertificateName"            = "digital-d-epic-bridge-v1-cert"
      "Serilog--WriteTo--1--Args--instrumentationKey" = ""
      "ServiceBusOptions--FullyQualifiedNamespace"    = "duly-d-app-ns.servicebus.windows.net"
      "ServiceBusOptions--QueueName"                  = "q-integration-clinic-audit-sbq"
    }
    enable_rbac_authorization = false
    network_acls = {
      bypass         = "AzureServices"
      default_action = "Deny"
      ip_rules = [
        "174.128.60.160/32",
        "174.128.60.162/32"
      ]

      subnet_associations = [
        {
          subnet_name = "duly-d-vnet-app-snet"
          vnet_name   = "duly-d-vnet"
          rg_name     = "duly-d-vnet-rg"
        },
        {
          subnet_name = "duly-d-vnetdops-cicd-snet"
          vnet_name   = "duly-d-vnetdops"
          rg_name     = "duly-d-vnet-rg"
        },
      ]
    }
    kv_diagnostic_setting_name         = "duly-q-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    tags = {
      environment         = "QAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "12/28/2021"
    }
  },
  {
    name                            = "duly-q-int-api-ngdp-kv"
    rg_name                         = "duly-q-collaboration-view-rg"
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
          "duly-d-ado-agent",
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
          "duly-q-component-pod-id",
          "duly-d-component-pod-id",
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
      },
      {
        group_names = [
          "EPAM-ADM-TEAM",
          "EPAM-DEV-GRP"
        ]
        secret_permissions = [
          "get",
          "list",
          "set",
          "delete",
          "recover",
          "backup",
          "restore",
          "purge"
        ]
        key_permissions = [
          "get",
          "list",
          "update",
          "create",
          "import",
          "delete",
          "recover",
          "backup",
          "restore",
          "decrypt",
          "encrypt",
          "wrapkey",
          "unwrapkey",
          "verify",
          "sign",
          "purge"
        ]
      }
    ]
    secrets = {
      "Authentication--ClientId"                      = ""
      "Authentication--Instance"                      = ""
      "Authentication--TenantId"                      = ""
      "Serilog--WriteTo--1--Args--instrumentationKey" = ""
    }
    enable_rbac_authorization = false
    network_acls = {
      bypass         = "AzureServices"
      default_action = "Deny"
      ip_rules = [
        "174.128.60.160/32",
        "174.128.60.162/32"
      ]

      subnet_associations = [
        {
          subnet_name = "duly-d-vnet-app-snet"
          vnet_name   = "duly-d-vnet"
          rg_name     = "duly-d-vnet-rg"
        },
        {
          subnet_name = "duly-d-vnetdops-cicd-snet"
          vnet_name   = "duly-d-vnetdops"
          rg_name     = "duly-d-vnet-rg"
        },
      ]
    }
    kv_diagnostic_setting_name         = "duly-d-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    tags = {
      environment         = "QAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "03/31/2022"
    }
  }
]