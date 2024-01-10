version_set_name         = "q-enterprise-security-v1"
version_set_display_name = "q-enterprise-security"
apim_name                = "duly-q-apim"
apim_rg_name             = "duly-q-apim-rg"
api_name                 = "q-enterprise-security-v1"
api_display_name         = "q-enterprise-security-api"
api_description          = "Perform set of common operations to support Duly enterprise security policies. "
api_web_service_url      = ""
api_url_suffix           = "shared/qat/security"
operations = [
  {
    operation_id = "invalidate-token"
    display_name = "InvalidateToken"
    method       = "POST"
    url_template = "/invalidate"
    description  = "Invalidate JWT to avoid re-usage after logout."
  }
] 