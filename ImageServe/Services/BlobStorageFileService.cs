using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace ImageServe.Services
{
    public class BlobStorageFileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobStorageFileService> _logger;

        public BlobStorageFileService(ILoggerFactory loggerFactory, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _logger = loggerFactory.CreateLogger<BlobStorageFileService>();
        }

        public async Task DeleteFileAsync(string container, string fileName)
        {
            await _blobServiceClient
                .GetBlobContainerClient(container)
                .DeleteBlobAsync(fileName);
        }

        public async Task<Stream> GetFileStreamAsync(string container, string fileName)
        {
            return await _blobServiceClient
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName)
                .OpenReadAsync();
        }

        public async IAsyncEnumerable<string> ListFilesAsync(string container, string prefix = null)
        {
            // Enumerate the blobs returned for each page.
            await foreach (BlobItem blobItem in _blobServiceClient.GetBlobContainerClient(container).GetBlobsAsync(prefix: prefix))
            {
                yield return blobItem.Name;
            }
        }

        public async Task SetFileStreamAsync(Stream inputStream, string container, string fileName)
        {
            await _blobServiceClient
                .GetBlobContainerClient(container)
                .UploadBlobAsync(fileName, inputStream);
        }

        public async Task<long> GetFileSizeAsync(string container, string fileName)
        {
            var properties = await _blobServiceClient
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName)
                .GetPropertiesAsync();

            return properties.Value.ContentLength;
        }

        public async Task<bool> GetFileExistsAsync(string container, string fileName)
        {
            return await _blobServiceClient
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName)
                .ExistsAsync();
        }
    }
}
