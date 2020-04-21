using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADGroupAuthorization.Infrastructure
{
    public static class GraphScopes
    {
        public const string UserRead = "User.Read";
        public const string UserReadBasicAll = "User.ReadBasic.All";
        public const string DirectoryReadAll = "Directory.Read.All";
    }
}
