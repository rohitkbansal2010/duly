version_set_name         = "a-enterprise-security-v1"
version_set_display_name = "a-enterprise-security"
apim_name                = "duly-q-apim"
apim_rg_name             = "duly-q-apim-rg"
api_name                 = "a-enterprise-security-v1"
api_display_name         = "a-enterprise-security-api"
api_description          = "Perform set of common operations to support Duly enterprise security policies. "
api_web_service_url      = ""
api_url_suffix           = "shared/aut/security"
operations = [
  {
    operation_id = "invalidate-token"
    display_name = "InvalidateToken"
    method       = "POST"
    url_template = "/invalidate"
    description  = "Invalidate JWT to avoid re-usage after logout."
  }
] 