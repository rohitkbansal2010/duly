
data "azurerm_kubernetes_cluster" "aks" {
  name                = var.aks_name
  resource_group_name = var.aks_rg_name
}

resource "null_resource" "download_chart" {
  triggers = {
    always_run = "${timestamp()}"
  }

  provisioner "local-exec" {
    command = <<-EOT
      git clone git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-aks-helmcharts
    EOT
  }
}

provider "helm" {
  kubernetes {
    host                   = data.azurerm_kubernetes_cluster.aks.kube_config.0.host
    cluster_ca_certificate = base64decode(data.azurerm_kubernetes_cluster.aks.kube_config.0.cluster_ca_certificate)
    token                  = data.azurerm_kubernetes_cluster.aks.kube_config.0.password
  }
}

module "uai" {
  source             = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.aad-pod-identity?ref=dev"
  count              = length(var.app_uai)
  name               = var.app_uai[count.index].name
  rg_name            = var.app_uai[count.index].rg_name
  location           = var.app_uai[count.index].location
  baseline_keyvaults = var.app_uai[count.index].baseline_keyvaults
  tags               = var.app_uai[count.index].tags
  chart_path         = "./duly-aks-helmcharts/duly-p-app-aks/azure-identity/"

  depends_on = [
    null_resource.download_chart
  ]
}
