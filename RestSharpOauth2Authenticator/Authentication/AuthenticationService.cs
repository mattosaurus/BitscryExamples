using Microsoft.Extensions.Options;
using RestSharp;
using RestSharpOauth2Authenticator.Helpers;
using RestSharpOauth2Authenticator.Services;
using System.Threading.Tasks;

namespace RestSharpOauth2Authenticator.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<AuthenticationOptions> _options;
        private readonly IRestClient _restClient;
        protected ICacheService _cacheService;

        public AuthenticationService(IOptions<AuthenticationOptions> options, ICacheService cacheService)
        {
            _options = options;
            _cacheService = cacheService;
            _restClient = new RestClient(options.Value.AuthenticationBaseUri);

            _restClient.AddHandler("application/json", () => { return new NewtonsoftJsonRestSerializer(); });
            _restClient.UseSerializer(() => { return new NewtonsoftJsonRestSerializer(); });
        }

        public AuthorizationToken GetToken()
        {
            string cacheKey = _options.Value.ClientId;
            var cachedValue = _cacheService.Get(cacheKey);

            if (cachedValue == null)
            {
                var response = GetTokenResponse();

                _cacheService.AddOrUpdate(cacheKey, response);

                return new AuthorizationToken(response.AccessToken, response.TokenType);
            }
            else
            {
                return new AuthorizationToken(cachedValue.AccessToken, cachedValue.TokenType);
            }
        }

        public async Task<AuthorizationToken> GetTokenAsync()
        {
            string cacheKey = _options.Value.ClientId;
            var cachedValue = _cacheService.Get(cacheKey);

            if (cachedValue == null)
            {
                var response = await GetTokenResponseAsync();

                _cacheService.AddOrUpdate(cacheKey, response);

                return new AuthorizationToken(response.AccessToken, response.TokenType);
            }
            else
            {
                return new AuthorizationToken(cachedValue.AccessToken, cachedValue.TokenType);
            }
        }

        private TokenResponse GetTokenResponse()
        {
            var tokenRequest = new TokenRequest(_options.Value.ClientId, _options.Value.ClientSecret, _options.Value.AccountId, _options.Value.Scope);
            var restRequest = new RestRequest("/v2/token", Method.POST);
            restRequest.AddJsonBody(tokenRequest);

            var authorizationToken = _restClient.Execute<TokenResponse>(restRequest);

            return authorizationToken.Data;
        }

        private async Task<TokenResponse> GetTokenResponseAsync()
        {
            var tokenRequest = new TokenRequest(_options.Value.ClientId, _options.Value.ClientSecret, _options.Value.AccountId, _options.Value.Scope);
            var restRequest = new RestRequest("/v2/token", Method.POST);
            restRequest.AddJsonBody(tokenRequest);

            var authorizationToken = await _restClient.ExecuteAsync<TokenResponse>(restRequest);

            return authorizationToken.Data;
        }
    }
}
