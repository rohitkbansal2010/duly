#resource "azurerm_virtual_machine" "test" {
#  count                 = 1
#  name                  = "duly-${var.env}-wrapper-vm"
#  location              = var.location
#  #availability_set_id   = azurerm_availability_set.avset.id
#  resource_group_name   = var.resource_group_name
#  network_interface_ids = [element(azurerm_network_interface.main.*.id, count.index)]
#  vm_size               = "Standard_B2s"
#
#  delete_os_disk_on_termination = var.os_disk_auto_delete
#  delete_data_disks_on_termination = var.data_disk_auto_delete
#
#  storage_image_reference {
#    publisher = "Canonical"
#    offer     = "UbuntuServer"
#    sku       = "20.04-LTS"
#    version   = "latest"
#  }
#
#  storage_os_disk {
#    name              = "duly-${var.env}-wrapper-vm-osdisk_${count.index}"
#    caching           = "ReadWrite"
#    create_option     = "FromImage"
#    managed_disk_type = var.managed_disk_type
#  }
#
#  # Optional data disks
#  storage_data_disk {
#    name              = "duly-${var.env}-wrapper-vm-datadisk_${count.index}"
#    managed_disk_type = var.managed_disk_type
#    create_option     = "Empty"
#    lun               = 0
#    disk_size_gb      = "64"
#  }
#
#  storage_data_disk {
#    name            = element(azurerm_managed_disk.test.*.name, count.index)
#    managed_disk_id = element(azurerm_managed_disk.test.*.id, count.index)
#    create_option   = "Attach"
#    lun             = 1
#    disk_size_gb    = element(azurerm_managed_disk.test.*.disk_size_gb, count.index)
#  }
#
#  os_profile {
#    computer_name  = "hostname"
#    admin_username = "testadmin"
#    admin_password = "Password1234!"
#  }
#
#  os_profile_linux_config {
#    disable_password_authentication = false
#  }
#
#  tags = {
#    environment = "staging"
#  }
#}
