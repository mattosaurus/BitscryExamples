using RestSharpOauth2Authenticator.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpOauth2Authenticator.Authentication
{
    public interface IAuthenticationService
    {
        AuthorizationToken GetToken();

        Task<AuthorizationToken> GetTokenAsync();
    }
}
