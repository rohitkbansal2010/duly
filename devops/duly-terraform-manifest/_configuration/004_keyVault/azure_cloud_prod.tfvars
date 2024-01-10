keyvaults = [
  {
    name                            = "duly-p-certs-kv"
    rg_name                         = "duly-p-sharedsvc-rg"
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
          "duly-p-ado-agent"
        ]
        secret_permissions = [
          "get",
          "list",
          "set",
          "delete",
          "purge"
        ]
        certificate_permissions = [
          "List",
          "Get"
        ]
      },
      {
        user_principal_names = [
          "Kevin.Davisson@boncura.com",
          "Evgeny.Polyarush@boncura.com"
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
        certificate_permissions = [
          "Create",
          "Delete",
          "List",
          "Import",
          "Get"
        ]
      },
      {
        group_names = [
        ]
        secret_permissions = [
        ]
      }
    ]
    secrets                   = {}
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