public_ips = [
  {
    name              = "duly-p-net-agw-pip01"
    rg_name           = "duly-p-agw-rg"
    allocation_method = "Static"
    sku               = "Standard"
    tier              = "Regional"
    availability_zone = "Zone-Redundant"
    ip_version        = "IPv4"
    domain_name_label = "duly-p-net-agw-pip01"
    tags = {
      environment = "PRD"
    }
  }
]