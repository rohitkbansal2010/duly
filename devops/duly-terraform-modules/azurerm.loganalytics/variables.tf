variable "name" {}
variable "rg_name" {}
variable "pricing_tier" {}
variable "retention_in_days" {}
# variable "solutions" {}
variable "tags" {
    type = map(string)
    default = {}
}