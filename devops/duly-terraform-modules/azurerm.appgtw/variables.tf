variable "name" {}
variable "location" {}
variable "resource_group_name" {}
variable "sku" {
  type = map(string)
}
variable "waf" {
  type = map(string)
}

variable "waf_disabled_rule_groups" {
  type    = list(any)
  default = []
}
variable "tags" {
  type    = map(string)
  default = {}
}
variable "autoscaling" {
  type    = bool
  default = false
}
variable "autoscale_configuration" {
  type = map(string)
}
variable "zones" {
  type    = list(any)
  default = []
}
variable "enable_http2" {
  type    = bool
  default = false
}
variable "gateway_ip_configurations" {
  type = list(any)
}
variable "frontend_ip_configurations" {
  type = list(any)
}
variable "app_definitions" {
  type = list(any)
}
variable "frontend_ports" {
  type = list(any)
}
variable "ssl_certificates" {
  type    = list(any)
  default = []
}
variable "user_assigned_identity" {
  type    = bool
  default = true
}
variable "log_analytics_workspace_id" {
  type        = string
  description = "Specifies the ID of a Log Analytics Workspace where Diagnostics Data should be sent"
}

variable "appgtw_diagnostic_setting_name" {
  type        = string
  description = "Specifies the name of the Diagnostic Setting"
}

variable "log_list" {
  type        = list(any)
  description = "Specifies the name of logs for diagnostic settings"
  default     = []
}