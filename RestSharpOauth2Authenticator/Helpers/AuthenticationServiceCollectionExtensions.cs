using Microsoft.Extensions.DependencyInjection;
using RestSharpOauth2Authenticator.Authentication;
using RestSharpOauth2Authenticator.Services;
using System;

namespace RestSharpOauth2Authenticator.Helpers
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection collection,
            Action<AuthenticationOptions> setupAction)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            collection.Configure(setupAction);
            return collection
                .AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>()
                .AddSingleton<ICacheService, CacheService>()
                .AddSingleton<IAuthenticationService, AuthenticationService>();
        }
    }
}
