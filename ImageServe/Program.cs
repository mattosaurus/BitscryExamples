using ImageServe.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Examples.ImageServe
{
    public class Program
    {
        public static void Main()
        {
            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    // Add logging
                    services.AddSingleton(LoggerFactory.Create(builder =>
                    {
                        builder
                            .AddSerilog(dispose: true);
                    }));
                    // Add file service
                    services.AddSourceBlobStorageFileService(
                        Environment.GetEnvironmentVariable("StorageUrl")!,
                        Environment.GetEnvironmentVariable("AZURE_TENANT_ID")!
                        );
                })
                .Build();

            host.Run();
        }
    }
}