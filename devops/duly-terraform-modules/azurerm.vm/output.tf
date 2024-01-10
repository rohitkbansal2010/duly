
# locals {
#   windows_vm_ids = [
#     for vm in azurerm_windows_virtual_machine.vm_windows : vm.id
#   ]
#   linux_vm_ids = [
#     for vm in azurerm_linux_virtual_machine.vm_linux : vm.id
#   ]
# }

# output "windows_vm_id" {
#    value = local.windows_vm_ids
# }

# output "linux_vm_id" {
#    value = local.linux_vm_ids
# }


#####################################################################


# output "windows_vm_id" {
#   value = azurerm_windows_virtual_machine.vm_windows.*.id
# }

# output "linux_vm_id" {
#    value = azurerm_linux_virtual_machine.vm_linux.*.id
# }
