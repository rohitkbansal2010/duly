namespace Duly.UI.Flourish.GeneratePDF.AuthenticationModel
{
    public class AuthResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
}
