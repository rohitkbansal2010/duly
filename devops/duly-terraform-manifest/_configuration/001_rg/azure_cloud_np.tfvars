rg_list = [
  {
    name     = "duly-d-vnet-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-agw-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "env"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-apim-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-q-apim-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "QA"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-u-apim-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "UAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-devops-aks-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "DevOps"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-devops-vm-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "DevOps"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-devops-data-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "DevOps"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-app-aks-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-q-test-vm-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "QA"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-collaboration-view-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "Dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-d-data-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "Dev"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = ""
    }
  },
  {
    name     = "duly-q-collaboration-view-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "QAT"
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
    name     = "duly-u-collaboration-view-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
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
    name     = "duly-a-collaboration-view-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "AUT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "12/29/2021"
    }
  },
  {
    name     = "duly-q-appointment-management-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "QAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "03/30/2022"
    }
  },
  {
    name     = "duly-a-appointment-management-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "QAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "03/30/2022"
    }
  },
  {
    name     = "duly-u-appointment-management-rg"
    location = "NorthCentralUS"
    lockName = "deleteLock"
    lockType = "CanNotDelete"
    tags = {
      environment         = "UAT"
      provision           = "Terraform"
      businessCriticality = ""
      businessUnit        = ""
      businessOwner       = ""
      platformSupport     = ""
      functionalSupport   = ""
      reviewedOn          = "03/30/2022"
    }
  }
]

