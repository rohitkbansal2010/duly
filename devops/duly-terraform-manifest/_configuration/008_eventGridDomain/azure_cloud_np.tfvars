egds = [
  {
    name     = "duly-q-communication-hub-egd"
    location = "northcentralus"
    rg_name  = "duly-q-communication-hub-rg"
    roles = [
      {
        user_assigned_identity_name    = "duly-q-component-pod-id"
        user_assigned_identity_rg_name = "duly-d-app-aks-rg"
        role_definition_name           = "EventGrid Contributor"
      },
      {
        user_assigned_identity_name    = "keda-autoscaler-id"
        user_assigned_identity_rg_name = "duly-d-devops-rg"
        role_definition_name           = "EventGrid Contributor"
      },
      {
        user_assigned_identity_name    = "duly-q-component-pod-id"
        user_assigned_identity_rg_name = "duly-d-app-aks-rg"
        role_definition_name           = "EventGrid Data Sender"
      },
      {
        user_assigned_identity_name    = "keda-autoscaler-id"
        user_assigned_identity_rg_name = "duly-d-devops-rg"
        role_definition_name           = "EventGrid Data Sender"
      }
    ]
  },
  {
    name     = "duly-u-communication-hub-egd"
    location = "northcentralus"
    rg_name  = "duly-u-communication-hub-rg"

    roles = [
      {
        user_assigned_identity_name    = "duly-u-component-pod-id"
        user_assigned_identity_rg_name = "duly-d-app-aks-rg"
        role_definition_name           = "EventGrid Contributor"
      },
      {
        user_assigned_identity_name    = "duly-u-component-pod-id"
        user_assigned_identity_rg_name = "duly-d-app-aks-rg"
        role_definition_name           = "EventGrid Data Sender"
      },
      {
        user_assigned_identity_name    = "keda-autoscaler-id"
        user_assigned_identity_rg_name = "duly-d-devops-rg"
        role_definition_name           = "EventGrid Data Sender"
      },
      {
        user_assigned_identity_name    = "keda-autoscaler-id"
        user_assigned_identity_rg_name = "duly-d-devops-rg"
        role_definition_name           = "EventGrid Contributor"
      }
    ]
  }
]
