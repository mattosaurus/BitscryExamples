using Amazon.Runtime;
using Amazon.S3;
using Azure.Identity;
using ImageServe.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace ImageServe.Extensions
{
    public static class FileServiceCollectionExtensions
    {
        public static IServiceCollection AddSourceS3FileService(this IServiceCollection collection, string accessKey, string secretKey)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (accessKey == null) throw new ArgumentNullException(nameof(accessKey));
            if (secretKey == null) throw new ArgumentNullException(nameof(secretKey));

            // Create an S3 client object.
            BasicAWSCredentials awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Client s3Client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.EUWest2);
            return collection
                .AddSingleton<IAmazonS3>(s3Client)
                .AddSingleton<ISourceFileService, S3FileService>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="storageUrl">The URL of the storage container</param>
        /// <param name="tenantId">The tennant ID of the the user accessing the resource</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddSourceBlobStorageFileService(this IServiceCollection collection, string storageUrl, string? tenantId = null)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (storageUrl == null) throw new ArgumentNullException(nameof(storageUrl));

            if (tenantId != null)
                Environment.SetEnvironmentVariable("AZURE_TENANT_ID", tenantId);

            // Add blob storage client
            collection.AddAzureClients(builder =>
            {
                // Add a storage account client
                builder.AddBlobServiceClient(new Uri(storageUrl));

                // Select the appropriate credentials based on enviroment
                builder.UseCredential(new DefaultAzureCredential());
            });

            return collection
                .AddSingleton<ISourceFileService, BlobStorageFileService>();
        }
    }
}
