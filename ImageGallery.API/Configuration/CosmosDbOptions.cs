namespace ImageGallery.API.Configuration
{
    public class CosmosDbOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    }
}
