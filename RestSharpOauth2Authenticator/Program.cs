using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharpOauth2Authenticator.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestSharpOauth2Authenticator
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static int Main(string[] args)
        {
            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            try
            {
                // Start!
                MainAsync(args).Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static async Task MainAsync(string[] args)
        {
            // Create service collection
            Log.Information("Creating service collection");
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                Log.Information("Starting service");
                await serviceProvider.GetService<App>().Run();
                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder
                    .AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // Add authentication service
            serviceCollection.AddAuthentication(options =>
            {
                options.AccountId = "1303591";
                options.AuthenticationBaseUri = "https://mcsfwcp0sprjghvyljylgbzt-n1y.auth.marketingcloudapis.com";
                options.ClientId = "kvkk4yxseu00qias164rngh5";
                options.ClientSecret = "mTMvpEg9n1clPLHOlSGsWi2v";
                options.Scope = "push_read push_write push_send";
            });

            // Add Marketing Cloud service
            serviceCollection.AddMarketingCloud(options =>
            {
                options.BaseUri = "https://mcsfwcp0sprjghvyljylgbzt-n1y.rest.marketingcloudapis.com";
            });

            // Add app
            serviceCollection.AddTransient<App>();
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        RestClient restClient = new RestClient("https://mcsfwcp0sprjghvyljylgbzt-n1y.auth.marketingcloudapis.com");

    //        Dictionary<string, string> authenticationParameters = new Dictionary<string, string>()
    //        {
    //            { "grant_type", "client_credentials"},
    //            { "client_id", "kvkk4yxseu00qias164rngh5"},
    //            { "client_secret", "mTMvpEg9n1clPLHOlSGsWi2v"},
    //            { "scope", "push_read push_write push_send"},
    //            { "account_id", "1303591"}
    //        };

    //        restClient.Authenticator = new OAuth2Authenticator(authenticationParameters);

    //        RestRequest appInforRequest = new RestRequest("/push/v1/application/{appId}", Method.GET);
    //        appInforRequest.AddParameter("appId", "57c976e6-dbe1-481e-8c04-00f848ca959e", ParameterType.UrlSegment);

    //        var appInforResponse = restClient.Execute(appInforRequest);
    //    }
    //}
}
