output "server_name" {
  value = azurerm_mssql_server.sql_server.name
}
output "server_id" {
  value = azurerm_mssql_server.sql_server.id
}
output "fully_qualified_domain_name" {
  value = azurerm_mssql_server.sql_server.fully_qualified_domain_name
}