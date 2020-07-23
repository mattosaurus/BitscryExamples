using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharpOauth2Authenticator.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpOauth2Authenticator
{
    public class App
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<App> _logger;
        private readonly IMarketingCloudService _marketingCloudService;

        public App(IConfigurationRoot config, ILoggerFactory loggerFactory, IMarketingCloudService marketingCloudService)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _config = config;
            _marketingCloudService = marketingCloudService;
        }

        public async Task Run()
        {
            // Make request and get new token
            _marketingCloudService.GetAppInfo("57c976e6-dbe1-481e-8c04-00f848ca959e");

            // Make request using cached token
            _marketingCloudService.GetAppInfo("57c976e6-dbe1-481e-8c04-00f848ca959e");
        }
    }
}
