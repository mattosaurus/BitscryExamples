using System;

namespace RestSharpOauth2Authenticator.Authentication
{
    internal interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
