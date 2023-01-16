using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServe.Services
{
    public interface IBaseFileService
    {
        public Task<long> GetFileSizeAsync(string directory, string fileName);

        public Task<bool> GetFileExistsAsync(string directory, string fileName);
    }
}
