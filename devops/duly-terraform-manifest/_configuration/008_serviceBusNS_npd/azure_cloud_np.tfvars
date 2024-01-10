servicebus_namespace         = "duly-d-app-ns"
servicebus_namespace_rg_name = "duly-d-app-aks-rg"
location                     = "northcentralus"
queues = [
  "q-integration-clinic-audit-sbq",
  "a-integration-clinic-audit-sbq",
  "u-integration-clinic-audit-sbq",
  "q-comm-hub-callback-sbq",
  "a-comm-hub-callback-sbq",
  "u-comm-hub-callback-sbq",
  "q-appt-mngm-notification-sbq",
  "a-appt-mngm-notification-sbq",
  "u-appt-mngm-notification-sbq",
  "q-comm-hub-delivery-sbq",
  "a-comm-hub-delivery-sbq",
  "u-comm-hub-delivery-sbq",
  "q-comm-hub-notification-sbq",
  "a-comm-hub-notification-sbq",
  "u-comm-hub-notification-sbq",
  "q-comm-hub-response-sbq",
  "a-comm-hub-response-sbq",
  "u-comm-hub-response-sbq",
  "q-comm-hub-timeout-sbq",
  "a-comm-hub-timeout-sbq",
  "u-comm-hub-timeout-sbq"
]

roles = [
  {
    user_assigned_identity_name    = "duly-q-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Owner"

  },
  {
    user_assigned_identity_name    = "duly-q-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Sender"

  },
  {
    user_assigned_identity_name    = "duly-q-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Receiver"

  },
  # these roles are added manually
  # {
  #   user_assigned_identity_name    = "keda-autoscaler-id"
  #   user_assigned_identity_rg_name = "duly-d-devops-rg"
  #   role_definition_name           = "Azure Service Bus Data Owner"
  # },
  # {
  #   user_assigned_identity_name    = "keda-autoscaler-id"
  #   user_assigned_identity_rg_name = "duly-d-devops-rg"
  #   role_definition_name           = "Azure Service Bus Data Sender"

  # },
  # {
  #   user_assigned_identity_name    = "keda-autoscaler-id"
  #   user_assigned_identity_rg_name = "duly-d-devops-rg"
  #   role_definition_name           = "Azure Service Bus Data Receiver"

  # },
  {
    user_assigned_identity_name    = "duly-a-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Owner"

  },
  {
    user_assigned_identity_name    = "duly-a-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Sender"

  },
  {
    user_assigned_identity_name    = "duly-a-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Receiver"

  },
  {
    user_assigned_identity_name    = "duly-u-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Owner"

  },
  {
    user_assigned_identity_name    = "duly-u-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Sender"

  },
  {
    user_assigned_identity_name    = "duly-u-component-pod-id"
    user_assigned_identity_rg_name = "duly-d-app-aks-rg"
    role_definition_name           = "Azure Service Bus Data Receiver"

  }
]