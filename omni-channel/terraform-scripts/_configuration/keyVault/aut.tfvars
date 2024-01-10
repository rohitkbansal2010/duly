keyvaults = [
  {
    name                            = "duly-a-oc-apmg-orc-kv"
    rg_name                         = "duly-a-appointment-management-rg"
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
      "Authentication--Instance"              = "https://login.microsoftonline.com/"
      "Authentication--TenantId"              = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "IngestionSettings--IngestionApiUrl"    = "https://api-qat.duly-np.digital/communication/aut/ingestion/v1/request"
      "ConnectionStrings--DatabaseConnection" = ""
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
      reviewedOn          = "03/30/2022"
    }
  }
]