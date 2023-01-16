using System.Net;
using ImageServe.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageServe
{
    public class ImageServe
    {
        private readonly ILogger<ImageServe> _logger;
        private readonly ISourceFileService _sourceService;
        private readonly string _container;

        public ImageServe(ISourceFileService sourceService, ILoggerFactory loggerFactory)
        {
            _sourceService = sourceService;
            _logger = loggerFactory.CreateLogger<ImageServe>();
            _container = Environment.GetEnvironmentVariable("StorageUrl")!;
        }

        [Function("{imageName}")]
        public async Task<HttpResponseData> GetImageAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, string imageName, int? width, int? height)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (!await _sourceService.GetFileExistsAsync(_container, imageName))
                return req.CreateResponse(HttpStatusCode.NotFound);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "image/jpeg");

            if (width.HasValue && height.HasValue)
            {
                Image.Load(await _sourceService.GetFileStreamAsync(_container, imageName))
                    .Clone(x => x.Resize(width.Value, height.Value))
                    .SaveAsJpeg(response.Body);
            }
            else if (width.HasValue && !height.HasValue)
            {
                Image.Load(await _sourceService.GetFileStreamAsync(_container, imageName))
                    .Clone(x => x.Resize(width.Value, 0))
                    .SaveAsJpeg(response.Body);
            }
            else if (!width.HasValue && height.HasValue)
            {
                Image.Load(await _sourceService.GetFileStreamAsync(_container, imageName))
                    .Clone(x => x.Resize(0, height.Value))
                    .SaveAsJpeg(response.Body);
            }
            else
            {
                response.Body = await _sourceService.GetFileStreamAsync(_container, imageName);
            }

            return response;
        }
    }
}
