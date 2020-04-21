using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADGroupAuthorization.Services
{
    public interface IGraphApiOperations
    {
        Task<dynamic> GetUserInformation(string accessToken);
        Task<string> GetPhotoAsBase64Async(string accessToken);
    }
}
