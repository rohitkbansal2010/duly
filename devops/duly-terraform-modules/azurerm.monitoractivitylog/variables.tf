variable "log_analytics_workspace_id" {
  type        = string
  description = "Specifies the ID of a Log Analytics Workspace where Diagnostics Data should be sent"
}

variable "diagnostic_setting_name" {
  type        = string
  description = "Specifies the name of the Diagnostic Setting"
}

variable "log_list" {
  type        = list
  description = "Specifies the name of logs for diagnostic settings"
  default     = []
}