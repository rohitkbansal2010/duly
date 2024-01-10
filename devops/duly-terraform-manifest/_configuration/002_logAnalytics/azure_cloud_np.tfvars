logAnalytics = [
  {
    name              = "duly-d-shsvc-loganalytics"
    rg_name           = "duly-d-sharedsvc-rg"
    pricing_tier      = "PerGB2018"
    retention_in_days = 30
  },
  {
    name              = "duly-d-clinic-audit-loganalytics"
    rg_name           = "duly-d-clinic-audit-rg"
    pricing_tier      = "PerGB2018"
    retention_in_days = 30
  }
]
