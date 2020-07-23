using RestSharpOauth2Authenticator.Authentication;

namespace RestSharpOauth2Authenticator.Services
{
    public interface ICacheService
    {
        TokenResponse Get(string key);

        void AddOrUpdate(string key, TokenResponse value);
    }
}
