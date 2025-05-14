using Azure;
using Azure.AI.Vision.ImageAnalysis;
using ImageGallery.API.Configuration;
using Microsoft.Extensions.Options;

namespace ImageGallery.API.Services
{
    public class ImageAnalysisService
    {
        private readonly ImageAnalysisClient _client;

        public ImageAnalysisService(IOptions<ComputerVisionOptions> options)
        {
            var endpoint = new Uri(options.Value.Endpoint);
            var credential = new AzureKeyCredential(options.Value.Key);
            _client = new ImageAnalysisClient(endpoint, credential);
        }

        public async Task<(List<string> Tags, string? Description, string? OcrText)> AnalyzeImageAsync(string imageUrl)
        {
            var features = VisualFeatures.Tags |
                VisualFeatures.Caption |
                VisualFeatures.Read;

            ImageAnalysisResult result = await _client.AnalyzeAsync(new Uri(imageUrl), features);

            var tags = result.Tags?.Values.Select(t => t.Name).ToList() ?? new();
            var description = result.Caption?.Text;
            var ocrTextLines = result.Read?.Blocks
                .SelectMany(block => block.Lines)
                .Select(line => line.Text)
                .ToList() ?? new List<string>();

            string? ocrText = ocrTextLines.Any()
                ? string.Join(" ", ocrTextLines)
                : null;


            return (tags, description, ocrText);
        }
    }
}
