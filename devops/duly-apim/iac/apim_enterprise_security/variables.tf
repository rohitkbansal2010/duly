variable "apim_name" {}
variable "apim_rg_name" {}
variable "version_set_name" {}
variable "version_set_display_name" {}
variable "api_name" {}
variable "api_display_name" {}
variable "api_description" {}
variable "api_url_suffix" {}
variable "api_web_service_url" {}
variable "subscription_required" {
  default = false
}
variable "operations" {}