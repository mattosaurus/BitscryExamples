using System;

namespace RestSharpOauth2Authenticator.Authentication
{
    internal class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
