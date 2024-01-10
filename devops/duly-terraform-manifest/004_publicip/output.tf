output "public_ip_ids" {
  value = module.public_ip.*.id
}

output "public_ip_names" {
  value = module.public_ip.*.name
}

output "public_ip_addresses" {
  value = module.public_ip.*.ip_address
}

output "fqdns" {
  value = module.public_ip.*.fqdn
}

output "public_ip_names_ids_map" {
  value = zipmap(module.public_ip.*.name, module.public_ip.*.id)
}

output "public_ip_names_addresses_map" {
  value = zipmap(module.public_ip.*.name, module.public_ip.*.ip_address)
}