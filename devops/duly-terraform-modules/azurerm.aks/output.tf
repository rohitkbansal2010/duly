output "id" {
  value = azurerm_kubernetes_cluster.k8s.id
}
output "name" {
  value = azurerm_kubernetes_cluster.k8s.name
}
output "username" {
  value = azurerm_kubernetes_cluster.k8s.kube_config.0.username
}
output "password" {
  value = azurerm_kubernetes_cluster.k8s.kube_config.0.password
}
output "host" {
  value = azurerm_kubernetes_cluster.k8s.kube_config.0.host
}
output "client_certificate" {
  value = azurerm_kubernetes_cluster.k8s.kube_config.0.client_certificate
}
output "client_key" {
  value = azurerm_kubernetes_cluster.k8s.kube_config.0.client_key
}
output "cluster_ca_certificate" {
  value = azurerm_kubernetes_cluster.k8s.kube_config.0.cluster_ca_certificate
}
output "kube_config_raw" {
  value = azurerm_kubernetes_cluster.k8s.kube_config_raw
}