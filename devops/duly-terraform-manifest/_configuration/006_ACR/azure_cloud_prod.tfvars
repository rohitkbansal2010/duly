acr = [
  {
    name                               = "dulydigitalcr"
    log_analytics_workspace_state_path = "002_logAnalytics/azure_cloud_prod/terraform.tfstate"
    log_analytics_workspace_name       = "duly-p-shsvc-loganalytics"
    storage_account_name               = "dulyptfstatesa"
    storage_account_rg                 = "duly-p-sharedsvc-rg"
    diagnostic_setting                 = "duly-digital-cr-diagnostic"
    location                           = "northcentralus"
    rg_name                            = "duly-p-sharedsvc-rg"
    sku                                = "Premium"
  }
]