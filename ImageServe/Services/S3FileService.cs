using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Logging;

namespace ImageServe.Services
{
    public class S3FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3FileService> _logger;

        public S3FileService(IAmazonS3 s3Client, ILoggerFactory loggerFactory)
        {
            _s3Client = s3Client;
            _logger = loggerFactory.CreateLogger<S3FileService>();
        }

        public async Task DeleteFileAsync(string bucketName, string key)
        {
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        public async Task<Stream> GetFileStreamAsync(string bucketName, string key)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            return response.ResponseStream;
        }

        public async IAsyncEnumerable<string> ListFilesAsync(string bucketName, string prefix = null)
        {
            ListObjectsV2Request request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };

            ListObjectsV2Response response;

            do
            {
                response = await _s3Client.ListObjectsV2Async(request);

                foreach (S3Object s3Object in response.S3Objects)
                {
                    yield return s3Object.Key;
                }

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
        }

        public async Task SetFileStreamAsync(Stream inputStream, string bucketName, string key)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key,
                InputStream = inputStream
            };

            await _s3Client.PutObjectAsync(request);
        }

        public async Task<long> GetFileSizeAsync(string bucketName, string key)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            return response.ContentLength;
        }

        public async Task<bool> GetFileExistsAsync(string bucketName, string key)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
