using Microsoft.Extensions.DependencyInjection;
using RestSharpOauth2Authenticator.Services;
using System;

namespace RestSharpOauth2Authenticator.Helpers
{
    public static class MarketingCloudServiceCollectionExtensions
    {
        public static IServiceCollection AddMarketingCloud(this IServiceCollection collection,
            Action<MarketingCloudOptions> setupAction)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            collection.Configure(setupAction);
            return collection
                .AddSingleton<IMarketingCloudService, MarketingCloudService>();
        }
    }
}
