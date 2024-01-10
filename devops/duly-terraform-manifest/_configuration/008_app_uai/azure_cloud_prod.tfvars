aks_name    = "duly-p-app-aks"
aks_rg_name = "duly-p-app-aks-rg"
app_uai = [
  {
    name     = "duly-p-component-pod-id"
    rg_name  = "duly-p-app-aks-rg"
    location = "northcentralus"
    tags = {
      environment = "PRD"
    }

    baseline_keyvaults = [
      {
        name    = "duly-p-certs-kv"
        rg_name = "duly-p-sharedsvc-rg"

        key_permissions = [
          "Get",
          "List",
          "Decrypt",
          "Encrypt",
          "UnwrapKey",
          "WrapKey",
          "Verify",
          "Sign"
        ]

        secret_permissions = [
          "Get",
          "List"
        ]

        certificate_permissions = [
          "Get",
          "List"
        ]
      }
    ]
  },
  {
    name     = "duly-p-keda-id"
    rg_name  = "duly-p-app-aks-rg"
    location = "northcentralus"
    tags = {
      environment = "PRD"
    }
    baseline_keyvaults = []
  }
]