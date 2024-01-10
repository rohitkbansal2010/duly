aks_name    = "duly-d-app-aks"
aks_rg_name = "duly-d-app-aks-rg"
app_uai = [
  {
    name     = "duly-d-component-pod-id"
    rg_name  = "duly-d-app-aks-rg"
    location = "northcentralus"
    tags = {
      environment         = "Dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "02/01/2022"
    }

    baseline_keyvaults = [
      {
        name    = "duly-d-certs-kv"
        rg_name = "duly-d-sharedsvc-rg"

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
    name     = "duly-q-component-pod-id"
    rg_name  = "duly-d-app-aks-rg"
    location = "northcentralus"
    tags = {
      environment         = "QAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "02/01/2022"
    }

    baseline_keyvaults = [
      {
        name    = "duly-d-certs-kv"
        rg_name = "duly-d-sharedsvc-rg"

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
    name     = "duly-a-component-pod-id"
    rg_name  = "duly-d-app-aks-rg"
    location = "northcentralus"
    tags = {
      environment         = "AUT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "02/01/2022"
    }

    baseline_keyvaults = [
      {
        name    = "duly-d-certs-kv"
        rg_name = "duly-d-sharedsvc-rg"

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
    name     = "duly-u-component-pod-id"
    rg_name  = "duly-d-app-aks-rg"
    location = "northcentralus"
    tags = {
      environment         = "UAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "02/01/2022"
    }

    baseline_keyvaults = [
      {
        name    = "duly-d-certs-kv"
        rg_name = "duly-d-sharedsvc-rg"

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
  }
]