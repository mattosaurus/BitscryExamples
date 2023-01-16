using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServe.Services
{
    public interface ISourceFileService : IBaseFileService
    {
        Task<Stream> GetFileStreamAsync(string directory, string fileName);

        IAsyncEnumerable<string> ListFilesAsync(string directory, string prefix = null);

        Task DeleteFileAsync(string directory, string fileName);
    }
}
