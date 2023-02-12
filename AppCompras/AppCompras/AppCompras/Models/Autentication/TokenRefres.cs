using Newtonsoft.Json;

namespace AppCompras.Models.Autentication
{
    public class TokenRefres
    {
        [JsonProperty("grant_type")]
        public string Grant_type => "refresh_token";

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
