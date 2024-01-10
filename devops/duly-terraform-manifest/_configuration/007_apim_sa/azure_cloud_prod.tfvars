storage_accounts = [
  {
    storage_name                        = "dulypapimsa"
    rg_name                             = "duly-p-sharedsvc-rg"
    account_tier                        = "Standard"
    account_kind                        = "StorageV2"
    account_replication_type            = "LRS"
    min_tls_version                     = "TLS1_1"
    access_tier                         = "Hot"
    allow_blob_public_access            = true
    large_file_share_enabled            = false
    enable_https_traffic_only           = true
    is_hns_enabled                      = false
    storage_acc_diagnostic_setting_name = "dulypapimsa-diagnostic"
    log_analytics_workspace_state_path  = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name        = "duly-p-shsvc-loganalytics"
  }
]