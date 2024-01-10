variable "apim_name" {}
variable "apim_rg_name" {}
variable "version_set_name" {}
variable "version_set_display_name" {}
variable "api_name" {}
variable "api_display_name" {}
variable "subscription_required" {
  default = true
}
variable "api_description" {}
variable "api_product_ids" {}
variable "api_url_suffix" {}
variable "api_web_service_url" {}
variable "oauth2_server_name" {
  default = null
}
variable "oauth2_override_scope" {
  default = null
}
