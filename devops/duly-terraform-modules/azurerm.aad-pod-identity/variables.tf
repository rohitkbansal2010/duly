variable "name" {}
variable "rg_name" {}
variable "location" {}
variable "tags" {}
# set access policy for a list of baseline infrastructure level keyvaults
# optional
variable "baseline_keyvaults" {
    default = []
}
variable "chart_path" {}