using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAppSettings
{
    public class App
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<App> _logger;

        public App(IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _config = config;
        }

        public async Task Run()
        {
            List<string> emailAddresses = _config.GetSection("EmailAddresses").Get<List<string>>();
            foreach (string emailAddress in emailAddresses)
            {
                _logger.LogInformation("Email address: {@EmailAddress}", emailAddress);
            }
        }
    }
}
