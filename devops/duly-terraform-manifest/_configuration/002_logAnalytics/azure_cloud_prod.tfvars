logAnalytics = [
  {
    name              = "duly-p-shsvc-loganalytics"
    rg_name           = "duly-p-sharedsvc-rg"
    pricing_tier      = "PerGB2018"
    retention_in_days = 30
  },
  {
    name              = "duly-p-clinic-audit-loganalytics"
    rg_name           = "duly-p-clinic-audit-rg"
    pricing_tier      = "PerGB2018"
    retention_in_days = 30
  }
]
