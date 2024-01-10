variable "name" {}
variable "location" {}
variable "resource_group_name" {}
variable "routes" {}
variable "subnet_associate" {}
variable "disable_bgp_route_propagation" {
    type = bool
    default = false
}
variable "tags" {
    type = map(string)
    default = {}
}
