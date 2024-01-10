keyvaults = [
  {
    name                            = "duly-a-int-api-clinic-kv"
    rg_name                         = "duly-a-collaboration-view-rg"
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
          "duly-a-component-pod-id",
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
      "Audit--ServiceBusNamespace"                   = "duly-d-app-ns.servicebus.windows.net"
      "Audit--ServiceBusQueue"                       = "a-integration-clinic-audit-sbq"
      "Authentication--Instance"                     = "https://login.microsoftonline.com/"
      "Authentication--TenantId"                     = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "DulyRestApiClient--VaultKeyIdUrl"             = "https://duly-d-certs-kv.vault.azure.net/keys/digital-signing-privkey/8cb84211d8b6415c868599ff0380a5b1"
      "Fhir--ClientId"                               = "b8cf1cf5-164a-4db8-96d2-3d87dccc45ba"
      "Fhir--TokenUrl"                               = "https://fhir.eehealth.org/fhirtst/oauth2/token"
      "FhirServerUrls--BaseUrlR4"                    = "https://fhir.eehealth.org/fhirtst/api/FHIR/R4"
      "FhirServerUrls--BaseUrlSTU3"                  = "https://fhir.eehealth.org/fhirtst/api/FHIR/STU3"
      "PrivateApiOptions--ApiBaseAddress"            = "https://epicproxy-np.et1296.epichosted.com/"
      "PrivateApiOptions--ApiClientId"               = "ef6de80d-1de6-4e75-acbf-46abd721ce6b"
      "PrivateApiOptions--ApiPassword"               = ""
      "PrivateApiOptions--ApiUsername"               = ""
      "PrivateApiOptions--BypassAppointmentCreation" = "true"
      "PrivateApiOptions--CertificateKeyVaultUrl"    = "https://duly-d-certs-kv.vault.azure.net/"
      "PrivateApiOptions--CertificateName"           = "digital-d-epic-bridge-v1-cert"
      "ServiceBusOptions--FullyQualifiedNamespace"   = "duly-d-app-ns.servicebus.windows.net"
      "ServiceBusOptions--QueueName"                 = "a-integration-clinic-audit-sbq"
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
    kv_diagnostic_setting_name         = "duly-a-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    tags = {
      environment         = "AUT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "03/31/2022"
    }
  },
  {
    name                            = "duly-a-int-api-ngdp-kv"
    rg_name                         = "duly-a-collaboration-view-rg"
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
          "duly-a-component-pod-id",
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
      "Authentication--Instance" = "https://login.microsoftonline.com/"
      "Authentication--TenantId" = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
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
      environment         = "AUT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "02/10/2022"
    }
  }
]