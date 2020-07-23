using Newtonsoft.Json;

namespace RestSharpOauth2Authenticator.Authentication
{
    public class TokenRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        public TokenRequest(string clientId, string clientSecret, string accountId, string scope)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.GrantType = "client_credentials";
            this.AccountId = accountId;
            this.Scope = scope ?? "";
        }
    }
}
