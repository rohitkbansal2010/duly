keyvaults = [
  {
    name                            = "duly-u-cv-api-enc-kv"
    rg_name                         = "duly-u-collaboration-view-rg"
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
          "duly-u-component-pod-id",
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
          "Aleksandr.Trutnev@boncura.com",
          "sravan.golla@boncura.com"
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
          "EPAM-DEV-GRP",
          "VT-ADM-TEAM",
          "VT-DEV-GRP"
        ]
        secret_permissions = [
          "get",
          "list",
          "set"
        ]
        certificate_permissions = [
          "get",
          "list",
        ]
      }
    ]
    secrets = {
      "ApplicationSettings--ADScopes"          = "Encounter.All"
      "Authentication--Instance"               = "https://login.microsoftonline.com/"
      "Authentication--TenantId"               = "d8b84fb8-d456-4b71-a75a-d15966423cdb"
      "ClinicApiClientOptions--ApiBaseAddress" = "https://api-uat.duly-np.digital/integration/clinic/v1"
      "NgdpApiClientOptions--ApiBaseAddress"   = "https://api-uat.duly-np.digital/integration/ngdp/v1/"
      "SiteDataOptions--Sites"                 = <<EOT
      [{"Id":"62889","Address":{ "Line":"1121 South Blvd","City":"Oak Park","State":"Illinois","PostalCode":"60302"}},{"Id":"62789","Address":{"Line":"9233 W. 159th St.","City":"Orland Hills","State":"Illinois","PostalCode":"60487"}},{"Id":"62989","Address":{"Line":"1034 N. Rohlwing Rd.","City":"Addison","State":"Illinois","PostalCode":"60101"}},{"id":"125034","address":{"line":"150 E. Willow Ave.","city":"Wheaton","state":"Illinois","postalCode":"60187"}},{"id":"12234","address":{"line":"220 Springfield Dr.","city":"Bloomingdale","state":"Illinois","postalCode":"60108"}},{"id":"17969","address":{"line":"640 S. Washington St.","city":"Naperville","state":"Illinois","postalCode":"60540"}}]
      EOT
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
    kv_diagnostic_setting_name         = "duly-u-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    tags = {
      environment         = "UAT"
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
    name                            = "duly-u-cv-api-res-kv"
    rg_name                         = "duly-u-collaboration-view-rg"
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
          "duly-u-component-pod-id",
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
          "Aleksandr.Trutnev@boncura.com",
          "sravan.golla@boncura.com"
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
          "EPAM-DEV-GRP",
          "VT-ADM-TEAM",
          "VT-DEV-GRP"
        ]
        secret_permissions = [
          "get",
          "list",
          "set"
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
    kv_diagnostic_setting_name         = "duly-u-keyvault-diagnostic"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_np/terraform.tfstate"
    log_analytics_workspace_name       = "duly-d-shsvc-loganalytics"
    tags = {
      environment         = "UAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "12/28/2021"
    }
  }
]