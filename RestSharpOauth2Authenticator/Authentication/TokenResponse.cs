using Newtonsoft.Json;

namespace RestSharpOauth2Authenticator.Authentication
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("rest_instance_url")]
        public string RestInstanceUrl { get; set; }

        [JsonProperty("soap_instance_url")]
        public string SoapInstanceUrl { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
