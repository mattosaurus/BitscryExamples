using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServe.Services
{
    public interface IFileService : ISourceFileService, IDestinationFileService
    {
    }
}
