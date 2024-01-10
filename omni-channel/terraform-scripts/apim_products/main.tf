module "product" {
  source                 = "git@ssh.dev.azure.com:v3/Next-Generation-Data-Platform/duly.digital.devops/duly-terraform-modules//azurerm.apim-product?ref=feature/one_secret_to_many_keyvaults"
  count                  = length(var.products)
  product_id             = var.products[count.index].product_id
  api_management_name    = var.products[count.index].api_management_name
  api_management_rg_name = var.products[count.index].api_management_rg_name
  product_display_name   = var.products[count.index].product_display_name
  product_description    = var.products[count.index].product_description
  subscriptions          = var.products[count.index].subscriptions
}
