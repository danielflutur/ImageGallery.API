using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ImageGallery.API.Configuration;
using Microsoft.Extensions.Options;

namespace ImageGallery.API.Services
{
    public class ImageStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public ImageStorageService(IOptions<BlobStorageOptions> options)
        {
            var blobServiceClient = new BlobServiceClient(options.Value.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<(string BlobName, Uri BlobUri)> UploadImageAsync(IFormFile file)
        {
            var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return (blobName, blobClient.Uri);
        }

        public string GetBlobSasUrl(string blobName, int validMinutes = 10)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            if (!blobClient.Exists())
                throw new InvalidOperationException($"Blob '{blobName}' does not exist.");

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(validMinutes)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }
    }
}
