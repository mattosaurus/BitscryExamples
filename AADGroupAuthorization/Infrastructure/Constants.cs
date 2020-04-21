using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADGroupAuthorization.Infrastructure
{
    public static class Constants
    {
        public const string ScopeUserRead = "User.Read";
        public const string ScopeUserReadAll = "User.ReadBasic.All";
        public const string BearerAuthorizationScheme = "Bearer";
        public const string ScopeDirectoryReadAll = "Directory.Read.All";
    }
}
