variable "db_name" {
    type = string
    description = "Specifies the name of the PostgreSQL Database, which needs to be a valid PostgreSQL identifier."
    default = "epampostgresqldb"
}
variable "collation" {
    type = string
    description = "Specifies the collation of the database."
    default = "English_United States.1252"
}
variable "resource_group_name" {
    type = string
    description = " The name of the resource group in which the PostgreSQL Server exists. "
}
variable "server_name" {
    type = string
    description = "Specifies the name of the PostgreSQL Server."
}
variable "charset" {
    type = string
    description = "Specifies the Charset for the PostgreSQL Database, which needs to be a valid PostgreSQL Charset."
    default = "UTF8"
}