variable "name" {
    description = "Specifies the name of the Public IP resource . Changing this forces a new resource to be created."
    type = string
}
variable "rg_name" {
    description = "The name of the Resource group"
    type = string
}
variable "allocation_method" {
    description = "Defines the allocation method for this IP address. Possible values are Static or Dynamic."
    type = string
}
variable "sku" {
    description = "The SKU of the Public IP. Accepted values are Basic and Standard. Defaults to Basic."
    type = string
}
variable "ip_version" {
    description = "The IP Version to use, IPv6 or IPv4."
    type = string
}
variable "domain_name_label" {
    description = "Label for the Domain Name. Will be used to make up the FQDN. If a domain name label is specified, an A DNS record is created for the public IP in the Microsoft Azure DNS system."
    type = string
}

variable "idle_timeout_in_minutes" {
    description = "Specifies the timeout for the TCP idle connection. The value can be set between 4 and 30 minutes."
    type = number
    default = 4
}
variable "availability_zone" {
    default = "No-Zone"
}
variable "reverse_fqdn" {
    description = "A fully qualified domain name that resolves to this public IP address. If the reverseFqdn is specified, then a PTR DNS record is created pointing from the IP address in the in-addr.arpa domain to the reverse FQDN."
    type = string
    default = ""
}
variable "public_ip_prefix_id" {
    description = "If specified then public IP address allocated will be provided from the public IP prefix resource."
    type = string
    default = ""
}
variable "tags" {
    type = map(string)
    default = {}
}