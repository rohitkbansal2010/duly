zones = [
  {
    rg_name               = "duly-p-app-aks-rg"
    private_dns_zone_name = "prd.privatelink.northcentralus.azmk8s.io"
    network_link = [
      {
        vnet_name = "duly-p-vnet"
        rg_name   = "duly-p-vnet-rg"
      }
    ]
    records = [
      # collaboration view & integration
      {
        a_record_name = "integration-clinic-system-api-prd"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "integration-ngdp-system-api-prd"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "cv-resource-process-api-prd"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "cv-encounter-process-api-prd"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      # no "-prd" suffix versions of the records
      {
        a_record_name = "integration-clinic-system-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "integration-ngdp-system-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "cv-resource-process-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "cv-encounter-process-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      # communication hub & omni channel records
      {
        a_record_name = "ch-adapter-microsite-svc"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-adapter-sendgrid"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-adapter-twilio-flow"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-adapter-twilio-function"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-channel-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-ingestion-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-listener-sendgrid-fnc"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ch-listener-twilio-fnc"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "chat-bot-connector"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ms-channel-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      },
      {
        a_record_name = "ms-payload-api"
        a_record_ip_addresses = [
          "172.26.41.200"
        ]
      }
    ]
  },
  {
    rg_name               = "duly-p-data-rg"
    private_dns_zone_name = "privatelink.database.windows.net"
    network_link = [
      {
        vnet_name = "duly-p-vnet"
        rg_name   = "duly-p-vnet-rg"
      }
    ]
    records = [
      {
        a_record_name = "duly-p-sql"
        a_record_ip_addresses = [
          "172.26.42.4"
        ]
      },
    ]
  }
]