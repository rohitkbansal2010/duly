servicebus_namespace         = "duly-p-app-ns"
servicebus_namespace_rg_name = "duly-p-app-aks-rg"
location                     = "northcentralus"
queues = [
  "p-integration-clinic-audit-sbq",
  "p-comm-hub-callback-sbq",
  "p-appt-mngm-notification-sbq",
  "p-comm-hub-delivery-sbq",
  "p-comm-hub-notification-sbq",
  "p-comm-hub-response-sbq",
  "p-comm-hub-timeout-sbq"
]

roles = [
  {
    user_assigned_identity_name    = "duly-p-component-pod-id"
    user_assigned_identity_rg_name = "duly-p-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Owner"

  },
  {
    user_assigned_identity_name    = "duly-p-component-pod-id"
    user_assigned_identity_rg_name = "duly-p-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Sender"

  },
  {
    user_assigned_identity_name    = "duly-p-component-pod-id"
    user_assigned_identity_rg_name = "duly-p-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Receiver"

  },
  {
    user_assigned_identity_name    = "duly-p-keda-id"
    user_assigned_identity_rg_name = "duly-p-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Owner"
  },
  {
    user_assigned_identity_name    = "duly-p-keda-id"
    user_assigned_identity_rg_name = "duly-p-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Sender"

  },
  {
    user_assigned_identity_name    = "duly-p-keda-id"
    user_assigned_identity_rg_name = "duly-p-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Receiver"

  }
]