using RestSharp;
using RestSharp.Authenticators;
using RestSharpOauth2Authenticator.Authentication;

namespace RestSharpOauth2Authenticator.Helpers
{
    public class OAuth2Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _tokenService;

        public OAuth2Authenticator(IAuthenticationService tokenService)
        {
            _tokenService = tokenService;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var authorizationToken = _tokenService.GetToken();

            request.AddHeader("Authorization", $"{authorizationToken.Type} {authorizationToken.Value}");
        }
    }
}
