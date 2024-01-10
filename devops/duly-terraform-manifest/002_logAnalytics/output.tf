output "log_analytics_workspace_id" {
  value = module.logAnalytics.*.log_analytics_workspace_id
}

output "log_analytics_id" {
  value = module.logAnalytics.*.id
}

output "log_analytics_workspace_name" {
  value = module.logAnalytics.*.log_analytics_workspace_name
}