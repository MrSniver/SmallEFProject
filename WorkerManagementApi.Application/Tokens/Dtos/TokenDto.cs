using Newtonsoft.Json;

namespace WorkerManagementApi.Application.Tokens.Dtos
{
    [JsonObject("token")]
    public class TokenDto
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("expiry")]
        public int Expiry { get; set; }
    }
}
