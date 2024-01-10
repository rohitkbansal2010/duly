resource "azurerm_eventgrid_domain_topic" "egdt" {
  name                = var.egdts.name
  domain_name         = var.egdts.domain_name
  resource_group_name = var.egdts.rg_name
}
data "azurerm_servicebus_queue" "q" {
  name                = var.egdts.service_bus_queue
  resource_group_name = var.egdts.service_bus_rg_name
  namespace_name      = var.egdts.service_bus_ns_name
}

resource "azurerm_eventgrid_event_subscription" "egevs" {
  name                          = var.egdts.name
  scope                         = azurerm_eventgrid_domain_topic.egdt.id
  event_delivery_schema         = "CloudEventSchemaV1_0"
  service_bus_queue_endpoint_id = data.azurerm_servicebus_queue.q.id
}