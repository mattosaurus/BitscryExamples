using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharpOauth2Authenticator.Authentication;
using RestSharpOauth2Authenticator.Helpers;

namespace RestSharpOauth2Authenticator.Services
{
    public class MarketingCloudService : IMarketingCloudService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRestClient _restClient;
        private readonly ILogger<IMarketingCloudService> _logger;

        public MarketingCloudService(IOptions<MarketingCloudOptions> options, ILoggerFactory loggerFactory, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            _restClient = new RestClient(options.Value.BaseUri)
            {
                Authenticator = new OAuth2Authenticator(_authenticationService)
            };

            _logger = loggerFactory.CreateLogger<IMarketingCloudService>();
        }

        public void GetAppInfo(string appId)
        {
            RestRequest appInforRequest = new RestRequest("/push/v1/application/{appId}", Method.GET);
            appInforRequest.AddParameter("appId", appId, ParameterType.UrlSegment);

            var appInforResponse = _restClient.Execute(appInforRequest);
        }
    }
}
