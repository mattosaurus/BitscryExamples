namespace RestSharpOauth2Authenticator.Authentication
{
    public class AuthenticationOptions
    {
        public string AuthenticationBaseUri { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string GrantType { get; set; }

        public string AccountId { get; set; }

        public string Scope { get; set; }
    }
}
