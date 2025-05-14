using ImageGallery.API.Configuration;
using ImageGallery.API.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace ImageGallery.API.Services
{
    public class ImageMetadataService
    {
        private readonly Container _container;

        public ImageMetadataService(IOptions<CosmosDbOptions> options)
        {
            var client = new CosmosClient(options.Value.ConnectionString);
            var db = client.CreateDatabaseIfNotExistsAsync(options.Value.DatabaseName).Result;
            _container = db.Database.CreateContainerIfNotExistsAsync(
                options.Value.ContainerName,
                "/id"
            ).Result.Container;
        }

        public async Task AddImageMetadataAsync(ImageMetadata metadata)
        {
            await _container.CreateItemAsync(metadata, new PartitionKey(metadata.Id));
        }

        public async Task<IEnumerable<ImageMetadata>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<ImageMetadata>("SELECT * FROM c");
            var results = new List<ImageMetadata>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<ImageMetadata?> GetByIdAsync(string id) => 
            await _container.ReadItemAsync<ImageMetadata>(id, new PartitionKey(id))
                .ContinueWith(t => t.Status == TaskStatus.RanToCompletion ? t.Result.Resource : null);

        public async Task<IEnumerable<ImageMetadata>> SearchAsync(string term)
        {
            var sql = "SELECT * FROM c WHERE ARRAY_CONTAINS(c.tags, @term) OR CONTAINS(c.ocrText, @term)";
            var query = _container.GetItemQueryIterator<ImageMetadata>(
                new QueryDefinition(sql).WithParameter("@term", term));
            var list = new List<ImageMetadata>();
            while (query.HasMoreResults)
                list.AddRange((await query.ReadNextAsync()).Resource);
            return list;
        }


    }
}
