variable "rg_list" {}

# locals {
#   resource_groups_new_values = [
#     for item in var.rg : [
#       for value in values(item) :
#       replace(value, "#ORG_PREFIX#", var.org)
#     ]
#   ]

#   resource_groups = [
#     for item in var.rg :
#     zipmap(keys(item), local.resource_groups_new_values[index(var.rg, item)])
#   ]
# }
