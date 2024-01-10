variable "inbound_rules" {}
variable "outbound_rules" {}
variable "resource_group_name" {}
variable "nsg_name" {}
variable "location" {}
variable "subnet_associate" {
    type = list
    default = []
}
variable "nsg_flow_sa_id" {}
variable "network_watcher_name" {}
variable "log_analytics_workspace_id" {}
variable "log_analytics_workspace_resource_id" {}
variable "tags" {
    type = map(string)
    default = {}
}

variable "nsg_diagnostic_setting_name" {
    type = string
    description = "The name of the Diagnostic settings for nsg groupe"
}


