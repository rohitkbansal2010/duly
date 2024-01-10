egds = [
  {
    name     = "duly-p-communication-hub-egd"
    location = "northcentralus"
    rg_name  = "duly-p-communication-hub-rg"
    roles = [
      {
        user_assigned_identity_name    = "duly-p-component-pod-id"
        user_assigned_identity_rg_name = "duly-p-app-aks-rg"
        role_definition_name           = "EventGrid Contributor"
      },
      {
        user_assigned_identity_name    = "duly-p-keda-id"
        user_assigned_identity_rg_name = "duly-p-app-aks-rg"
        role_definition_name           = "EventGrid Contributor"
      },
      {
        user_assigned_identity_name    = "duly-p-component-pod-id"
        user_assigned_identity_rg_name = "duly-p-app-aks-rg"
        role_definition_name           = "EventGrid Data Sender"
      },
      {
        user_assigned_identity_name    = "duly-p-keda-id"
        user_assigned_identity_rg_name = "duly-p-app-aks-rg"
        role_definition_name           = "EventGrid Data Sender"
      }
    ]
  }
]
