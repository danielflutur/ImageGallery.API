using Newtonsoft.Json;

namespace ImageGallery.API.Models
{
    public class ImageMetadata
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("blobUrl")]
        public string BlobUrl { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("tags")]
        public List<string>? Tags { get; set; }

        [JsonProperty("ocrText")]
        public string? OcrText { get; set; }

        [JsonProperty("uploadedAt")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
