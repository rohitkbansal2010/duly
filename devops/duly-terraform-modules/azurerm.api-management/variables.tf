
variable "apim_name" { 
  description = "The name of the api management."
  type        = string
  default     = ""
}

variable "vnet_name" { 
  description = "The name of the vnet."
  type        = string
}

variable "vnet_rg_name" { 
  description = "The name of the resource group of the vnet."
  type        = string
}

variable "subnet_name" { 
  description = "The name of the subnet."
  type        = string
}

variable "rg_name" { 
  description = "Name of the resource group"
  type        = string
}

variable "location" { 

}

variable "publisher_name" { 
  description = "The name of publisher/company."
  type        = string
  default     = "EPAM"
}

variable "publisher_email" { 
  description = "The email of publisher/company."
  type        = string
  default     = "publisheremail@email.com"
}

variable "sku_name" { 
  type        = string
  description = "String consisting of two parts separated by an underscore. The fist part is the name, valid values include: Developer, Basic, Standard and Premium. The second part is the capacity"
  default     = "Developer_1"
}

variable "management_hostname_configuration" { 
  type        = list(map(string))
  description = "List of management hostname configurations"
  default     = []
}

variable "portal_hostname_configuration" {
  type        = list(map(string))
  description = "Legacy portal hostname configurations"
  default     = []
}

variable "developer_portal_hostname_configuration" { 
  type        = list(map(string))
  description = "Developer portal hostname configurations"
  default     = []
}

variable "proxy_hostname_configuration" { 
  type        = list(map(string))
  description = "List of proxy hostname configurations"
  default     = []
}

variable "scm_hostname_configuration" {
  type        = list(map(string))
  description = "List of scm hostname configurations"
  default     = []
}

variable "policy_configuration" {
  type        = list(map(string))
  description = "Map of policy configuration"
  default     = []
}

variable "certificate_configuration" {
  type        = list(map(string))
  description = "List of certificate configurations"
  default     = []
}

variable "enable_http2" {
  type        = bool
  description = "Should HTTP/2 be supported by the API Management Service?"
  default     = false
}

variable "notification_sender_email" {
  type        = string
  description = "Email address from which the notification will be sent"
  default     = null
}
#
variable "security_configuration" {
  type        = map(string)
  description = "Map of security configuration"
  default     = {}
}

variable "subscription_required" {
  type        = bool
  description = "Should this API require a subscription key?"
  default     = true
}

variable "kv_name" { 

}

variable "kv_rg_name" { 

}

variable "app_insights_name" {

}

variable "app_insights_rg_name" {

}

variable "named_values" {

}

