resource "azurerm_network_interface" "vmnic" {
  name                = "${var.name}-nic"
  location            = var.location
  resource_group_name = var.rg_name
  count               = 1
  dynamic "ip_configuration" {
    
    for_each = azurerm_public_ip.vm
    content {
      primary = true
      name                          = "${var.name}-ipcfg"
      subnet_id                     = data.azurerm_subnet.vmsubnet.id
      private_ip_address_allocation = var.vm_private_ip_allocation_method
      private_ip_address            = var.vm_private_ip_address
    }
  }

resource "azurerm_linux_virtual_machine" "vm" {
  name                = var.name
  resource_group_name = var.rg_name
  location            = var.location
  size                = var.size
  admin_username      = var.admin_username
  network_interface_ids = [
    azurerm_network_interface.vmnic.id,
  ]

  admin_ssh_key {
    username   = var.admin_username
    public_key = file("~/.ssh/id_rsa.pub")
  }

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = var.image_publisher
    offer     = var.image_offer
    sku       = var.image_sku
    version   = var.image_version
  }
}

# resource "azurerm_virtual_machine_extension" "payload" {
#   name                 = "${var.name}-payload"
#   virtual_machine_id   = azurerm_linux_virtual_machine.vm.id
#   publisher            = "Microsoft.Azure.Extensions"
#   type                 = "CustomScript"
#   type_handler_version = "2.0"

# #   settings = <<SETTINGS
# #     {
# #         "commandToExecute": "hostname && uptime"
# #     }
# # SETTINGS


#   tags = {
#     environment = "Development"
#   }
# }