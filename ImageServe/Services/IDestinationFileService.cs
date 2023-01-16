using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServe.Services
{
    public interface IDestinationFileService : IBaseFileService
    {
        Task SetFileStreamAsync(Stream inputStream, string directory, string fileName);
    }
}
