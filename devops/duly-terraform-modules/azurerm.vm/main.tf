
# Get resource group data
data "azurerm_resource_group" "vm_rg" {
  name = var.vm_rg_name
}

# Get Vnet data
data "azurerm_virtual_network" "vnet" {
  name                = var.vm_vnet_name
  resource_group_name = var.vm_vnet_rg_name
}

# Get Subnet data
data "azurerm_subnet" "vmsubnet" {
  name                 = var.vm_subnet_name
  virtual_network_name = var.vm_vnet_name
  resource_group_name  = var.vm_vnet_rg_name
}

# Get KV data
data "azurerm_key_vault" "kv" {
  name                = var.kv_name
  resource_group_name = var.kv_rg_name
}
data "azurerm_key_vault_secret" "kv_secret" {
  name         = var.vm_admin_username
  key_vault_id = data.azurerm_key_vault.kv.id
}

# # Backup protected vm resource was commented out due to the fact that the deployment via AzureDevops is never stopped (falls into an infinite cycle),
# # but via a local deployment everything works perfectly

# # Get backup policy data
# data "azurerm_backup_policy_vm" "backup_policy" {
#   name                = var.backup_policy_name
#   recovery_vault_name = var.rec_vault_name
#   resource_group_name = var.rec_vault_rg_name
# }

# Create PIP
resource "azurerm_public_ip" "vm" {
  count               = var.vm_create_pip == true ? 1 : 0
  name                = "${var.vm_name}-pip"
  location            = data.azurerm_resource_group.vm_rg.location
  resource_group_name = var.vm_rg_name
  allocation_method   = var.vm_pip_allocation_method
  sku                 = "Standard"
  domain_name_label   = var.vm_name
  availability_zone   = "1"
}


# Create NIC
resource "azurerm_network_interface" "vmnic" {
  name                = "${var.vm_name}-nic"
  location            = data.azurerm_resource_group.vm_rg.location
  resource_group_name = var.vm_rg_name
  count               = 1
  dynamic "ip_configuration" {
    
    for_each = azurerm_public_ip.vm
    content {
      primary = true
      name                          = "${var.vm_name}-ipcfg"
      subnet_id                     = data.azurerm_subnet.vmsubnet.id
      private_ip_address_allocation = var.vm_private_ip_allocation_method
      private_ip_address            = var.vm_private_ip_address
      public_ip_address_id          = azurerm_public_ip.vm[count.index].id
    }
  }

  dynamic "ip_configuration" {
    for_each = var.vm_create_pip == false ? [1] : []
    content {
      name                          = "${var.vm_name}-ipcfg"
      subnet_id                     = data.azurerm_subnet.vmsubnet.id
      private_ip_address_allocation = var.vm_private_ip_allocation_method
      private_ip_address            = var.vm_private_ip_address
    }
  }
}

# Get application security group data
data "azurerm_application_security_group" "asg" {
  name                = var.asg_name
  resource_group_name = var.vm_vnet_rg_name
}

# Create nic interface application security group association
resource "azurerm_network_interface_application_security_group_association" "asg_association" {
  count                         = 1
  network_interface_id          = azurerm_network_interface.vmnic[count.index].id
  application_security_group_id = data.azurerm_application_security_group.asg.id
}

# Create managed data disk for VM
resource "azurerm_managed_disk" "datadisk" {
  count                = (var.enable_data_disk == true) ? 1 : 0
  name                 = "${var.vm_name}-datadisk"
  location             = data.azurerm_resource_group.vm_rg.location
  resource_group_name  = var.vm_rg_name
  storage_account_type = var.storage_account_type
  create_option        = "Empty"
  disk_size_gb         = var.data_disk_size_gb
  zones                = [ "1" ]
}

# # Create managed data disk attachment for windows vm
resource "azurerm_virtual_machine_data_disk_attachment" "windows_datadisk" {
  count              = var.enable_data_disk == true && var.vm_guest_os == "windows" ? 1 : 0
  managed_disk_id    = azurerm_managed_disk.datadisk[count.index].id
  virtual_machine_id = azurerm_windows_virtual_machine.vm_windows[count.index].id
  lun                = var.data_disk_lun 
  caching            = "ReadWrite"
}

# # Create managed data disk attachment for linux vm
resource "azurerm_virtual_machine_data_disk_attachment" "linux_datadisk" {
  count              = var.enable_data_disk == true && var.vm_guest_os == "linux" ? 1 : 0
  managed_disk_id    = azurerm_managed_disk.datadisk[count.index].id
  virtual_machine_id = azurerm_linux_virtual_machine.vm_linux[count.index].id
  lun                = var.data_disk_lun 
  caching            = "ReadWrite"
}


# Windows VM NO DataDisk
resource "azurerm_windows_virtual_machine" "vm_windows" {
  count                 = var.vm_guest_os == "windows" ? 1 : 0
  name                  = var.vm_name
  location              = data.azurerm_resource_group.vm_rg.location
  resource_group_name   = data.azurerm_resource_group.vm_rg.name
  size                  = var.vm_size
  network_interface_ids = [azurerm_network_interface.vmnic[count.index].id]
  computer_name         = var.vm_name
  admin_username        = var.vm_admin_username
  admin_password        = data.azurerm_key_vault_secret.kv_secret.value  
  provision_vm_agent    = true
  zone                  = "1"
  tags                  = var.tags


  source_image_reference {
    publisher = var.vm_image_publisher
    offer     = var.vm_image_offer
    sku       = var.vm_image_sku
    version   = "latest"
  }

  os_disk {
    name                 = "${var.vm_name}-osdisk"
    caching              = "ReadWrite"
    storage_account_type = var.storage_account_type
    # disk_size_gb         = 127
  }

  boot_diagnostics {
    storage_account_uri = var.storage_account_uri
  } 
}

# # Backup protected vm resource was commented out due to the fact that the deployment via AzureDevops is never stopped (falls into an infinite cycle),
# # but via a local deployment everything works perfectly

# # Enable backup for windows vm
# resource "azurerm_backup_protected_vm" "vm_windows" {
#   count               = var.vm_guest_os == "windows" ? 1 : 0
#   resource_group_name = var.rec_vault_rg_name
#   recovery_vault_name = var.rec_vault_name
#   source_vm_id        = azurerm_windows_virtual_machine.vm_windows[count.index].id
#   backup_policy_id    = data.azurerm_backup_policy_vm.backup_policy.id
# }

# Set windows VM automation account for dsc management extension
resource "azurerm_automation_account" "dsc_account" {
  depends_on          = [azurerm_windows_virtual_machine.vm_windows]
  count               = var.vm_dsc_management_install == true && var.vm_guest_os == "windows" ? 1 : 0
  name                = "${var.vm_name}-account"
  location            = data.azurerm_resource_group.vm_rg.location
  resource_group_name = var.vm_rg_name
  sku_name            = "Basic"
}

# Set windows VM dsc management extension
resource "azurerm_automation_dsc_configuration" "dsc_config" {
  depends_on              = [azurerm_windows_virtual_machine.vm_windows]
  count                   = var.vm_dsc_management_install == true && var.vm_guest_os == "windows" ? 1 : 0
  name                    = "${var.vm_name}_dsc_config"
  resource_group_name     = var.vm_rg_name
  automation_account_name = azurerm_automation_account.dsc_account[count.index].name
  location                = data.azurerm_resource_group.vm_rg.location
  content_embedded        = "configuration ${var.vm_name}_dsc_config {}"
}

# Create RSA key for disk encryption
resource "azurerm_key_vault_key" "disk_encryption_key" {
  count        = var.vm_disk_encryption_install == true ? 1 : 0
  name         = "${var.vm_name}-diskencryptionkey"
  key_vault_id = data.azurerm_key_vault.kv.id
  key_type     = "RSA"
  key_size     = 2048

  key_opts = [
    "decrypt",
    "encrypt",
    "sign",
    "unwrapKey",
    "verify",
    "wrapKey",
  ]
}

# Set windows VM disk encryption extension
resource "azurerm_virtual_machine_extension" "vm_windows_disk_encryption" {
  depends_on                 = [azurerm_key_vault_key.disk_encryption_key, azurerm_virtual_machine_data_disk_attachment.windows_datadisk, azurerm_windows_virtual_machine.vm_windows]
  count                      = var.vm_disk_encryption_install == true && var.vm_guest_os == "windows" ? 1 : 0
  name                       = "${var.vm_name}_disk_encryption"
  virtual_machine_id         = azurerm_windows_virtual_machine.vm_windows[count.index].id
  publisher                  = "Microsoft.Azure.Security" 
  type                       = "AzureDiskEncryption"    
  type_handler_version       = "2.2"
  auto_upgrade_minor_version = true

  settings = <<PROTECTED_SETTINGS
    {
        "EncryptionOperation":    "${var.encrypt_operation}",
        "KeyVaultURL":            "${data.azurerm_key_vault.kv.vault_uri}",
        "KeyVaultResourceId":     "${data.azurerm_key_vault.kv.id}",					
        "KeyEncryptionKeyURL":    "${azurerm_key_vault_key.disk_encryption_key[count.index].id}",
        "KekVaultResourceId":     "${data.azurerm_key_vault.kv.id}",					
        "KeyEncryptionAlgorithm": "${var.encryption_algorithm}",
        "VolumeType":             "${var.volume_type}"
    } 
PROTECTED_SETTINGS
}

# Set windows VM diagnostic setting extension
resource "azurerm_virtual_machine_extension" "vm_windows_diagnostic_setting" {
  depends_on                 = [azurerm_virtual_machine_data_disk_attachment.windows_datadisk, azurerm_windows_virtual_machine.vm_windows]
  count                      = var.vm_diagnostic_setting_install == true && var.vm_guest_os == "windows" ? 1 : 0
  name                       = "${var.vm_name}-diagnostic_setting"
  virtual_machine_id         = azurerm_windows_virtual_machine.vm_windows[count.index].id
  publisher                  = "Microsoft.Azure.Diagnostics"
  type                       = "IaaSDiagnostics"
  type_handler_version       = "1.9"
  auto_upgrade_minor_version = true
  settings = <<SETTINGS
    {
      "StorageAccount": "${var.bootdiag_storage_name}",
      "WadCfg": {
        "DiagnosticMonitorConfiguration": {
          "overallQuotaInMB": 5120,
          "PerformanceCounters": {
            "scheduledTransferPeriod": "PT1M",
            "PerformanceCounterConfiguration": [
              {
                "counterSpecifier": "\\Processor Information(_Total)\\% Processor Time",
                "unit": "Percent",
                "sampleRate": "PT60S"
              }
            ]
          },
          "WindowsEventLog": {
            "scheduledTransferPeriod": "PT1M",
            "DataSource": [
              {
                "name": "Application!*[System[(Level=1 or Level=2 or Level=3)]]"
              }
            ]
          }
        }
      }
    }

  SETTINGS
  protected_settings = <<PROTECTED_SETTINGS
    {
      "storageAccountName": "${var.bootdiag_storage_name}",
      "storageAccountKey":  "${var.bootdiag_storage_primary_access_key}"
    }
  PROTECTED_SETTINGS
} 



# Linux VM NO DataDisk
resource "azurerm_linux_virtual_machine" "vm_linux" {
  count                           = var.vm_guest_os == "linux" ? 1 : 0
  name                            = var.vm_name
  location                        = data.azurerm_resource_group.vm_rg.location
  resource_group_name             = var.vm_rg_name
  size                            = var.vm_size
  network_interface_ids           = [azurerm_network_interface.vmnic[count.index].id]
  computer_name                   = var.vm_name
  admin_username                  = var.vm_admin_username
  admin_password                  = data.azurerm_key_vault_secret.kv_secret.value
  provision_vm_agent              = true  
  disable_password_authentication = var.vm_disable_password_authentication
  zone                            = "1"
  tags                            = var.tags

 
  source_image_reference {
    publisher = var.vm_image_publisher 
    offer     = var.vm_image_offer    
    sku       = var.vm_image_sku      
    version   = "latest"
  }

  os_disk {
    name                 = "${var.vm_name}-osdisk"
    caching              = "ReadWrite"
    storage_account_type = var.storage_account_type
    # disk_size_gb         = 45
  }

  boot_diagnostics {
    storage_account_uri = var.storage_account_uri
  }
}

# # Backup protected vm resource was commented out due to the fact that the deployment via AzureDevops is never stopped (falls into an infinite cycle),
# # but via a local deployment everything works perfectly
# # Enable backup for linux vm
# resource "azurerm_backup_protected_vm" "vm_linux" {
#   count               = var.vm_guest_os == "linux" ? 1 : 0
#   resource_group_name = var.rec_vault_rg_name
#   recovery_vault_name = var.rec_vault_name
#   source_vm_id        = azurerm_linux_virtual_machine.vm_linux[count.index].id
#   backup_policy_id    = data.azurerm_backup_policy_vm.backup_policy.id
# }

# Set linux VM disk encryption extension
resource "azurerm_virtual_machine_extension" "vm_linux_disk_encryption" {
  depends_on                 = [azurerm_key_vault_key.disk_encryption_key, azurerm_virtual_machine_data_disk_attachment.linux_datadisk, azurerm_linux_virtual_machine.vm_linux]
  count                      = var.vm_disk_encryption_install == true && var.vm_guest_os == "linux" ? 1 : 0
  name                       = "${var.vm_name}_disk_encryption"
  virtual_machine_id         = azurerm_linux_virtual_machine.vm_linux[count.index].id
  publisher                  = "Microsoft.Azure.Security"
  type                       = "AzureDiskEncryptionForLinux"
  type_handler_version       = "1.1"
  auto_upgrade_minor_version = true

  settings = <<PROTECTED_SETTINGS
    {
        "EncryptionOperation":    "${var.encrypt_operation}",
        "KeyVaultURL":            "${data.azurerm_key_vault.kv.vault_uri}",
        "KeyVaultResourceId":     "${data.azurerm_key_vault.kv.id}",					
        "KeyEncryptionKeyURL":    "${azurerm_key_vault_key.disk_encryption_key[count.index].id}",
        "KekVaultResourceId":     "${data.azurerm_key_vault.kv.id}",					
        "KeyEncryptionAlgorithm": "${var.encryption_algorithm}",
        "VolumeType":             "${var.volume_type}"
    } 
  PROTECTED_SETTINGS
}


# Set linux VM diagnostic setting extension
# To install this extension you need to generate a sas token in your storage account and pass it into a variable "storageAccountSasToken".

# resource "azurerm_monitor_diagnostic_setting" "vm_diagnostic_setting" {
#   name               = "${var.vm_name}-diagnostic_setting"
#   target_resource_id = data.azurerm_key_vault.kv.id
#   bootdiag_storage_id = var.bootdiag_storage_id

#   log {
#     category = "AuditEvent"
#     enabled  = true

#     retention_policy {
#       enabled = false
#     }
#   }

#   metric {
#     category = "AllMetrics"

#     retention_policy {
#       enabled = false
#     }
#   }
# }

# resource "azurerm_virtual_machine_extension" "vm_linux_diagnostic_setting" {
#   depends_on                 = [azurerm_virtual_machine_data_disk_attachment.linux_datadisk]
#   count                      = var.vm_diagnostic_setting_install == true && var.vm_guest_os == "linux" ? 1 : 0
#   name                       = "${var.vm_name}-diagnostic_setting"
#   virtual_machine_id         = azurerm_linux_virtual_machine.vm_linux[count.index].id
#   publisher                  = "Microsoft.Azure.Diagnostics"
#   type                       = "LinuxDiagnostic"
#   type_handler_version       = "4.0"
#   auto_upgrade_minor_version = true
#   settings = <<SETTINGS
#   {
#     "StorageAccount": "${var.bootdiag_storage_name}",
#     "ladCfg": {
#       "sampleRateInSeconds": 15,
#       "diagnosticMonitorConfiguration": {
#         "performanceCounters": {
#           "performanceCounterConfiguration": [
#             {
#               "unit": "Percent",
#               "type": "builtin",
#               "counter": "PercentProcessorTime",
#               "counterSpecifier": "/builtin/Processor/PercentProcessorTime",
#               "annotation": [
#                 {
#                   "locale": "en-us",
#                   "displayName": "Aggregate CPU %utilization"
#                 }
#               ],
#               "condition": "IsAggregate=TRUE",
#               "class": "Processor"
#             },
#             {
#               "unit": "Bytes",
#               "type": "builtin",
#               "counter": "UsedSpace",
#               "counterSpecifier": "/builtin/FileSystem/UsedSpace",
#               "annotation": [
#                 {
#                   "locale": "en-us",
#                   "displayName": "Used disk space on /"
#                 }
#               ],
#               "condition": "Name=\"/\"",
#               "class": "Filesystem"
#             }
#           ]
#         },
#         "metrics": {
#           "metricAggregation": [
#             {
#               "scheduledTransferPeriod": "PT1H"
#             },
#             {
#               "scheduledTransferPeriod": "PT1M"
#             }
#           ],
#           "resourceId": "${azurerm_linux_virtual_machine.vm_linux[count.index].id}"
#         },
#         "eventVolume": "Large",
#         "syslogEvents": {
#           "syslogEventConfiguration": {
#             "LOG_USER": "LOG_INFO"
#           }
#         }
#       }
#     },
#     "fileLogs": [
#       {
#         "file": "/var/log/myladtestlog",
#         "table": "MyLadTestLog"
#       }
#     ]
#   }
#   SETTINGS
#   protected_settings = <<PROTECTED_SETTINGS
#     {
#       "storageAccountName": "${var.bootdiag_storage_name}",
#       "storageAccountSasToken": "SAS access token"
#     }
#   PROTECTED_SETTINGS
# } 
