using ImageGallery.API.Models;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ImageStorageService _storageService;
        private readonly ImageMetadataService _metadataService;
        private readonly ImageAnalysisService _imageAnalysisService;

        public ImageController(
            ImageStorageService storageService,
            ImageMetadataService metadataService,
            ImageAnalysisService imageAnalysisService)
        {
            _storageService = storageService;
            _metadataService = metadataService;
            _imageAnalysisService = imageAnalysisService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string? description)
        {
            var (blobName, blobUri) = await _storageService.UploadImageAsync(file);
            var sasUrl = _storageService.GetBlobSasUrl(blobName);
            var (tags, aiDescription, ocrText) = await _imageAnalysisService.AnalyzeImageAsync(sasUrl);

            var metadata = new ImageMetadata
            {
                FileName = file.FileName,
                BlobUrl = blobUri.ToString(),
                Description = description ?? aiDescription,
                Tags = tags,
                OcrText = ocrText
            };
            await _metadataService.AddImageMetadataAsync(metadata);

            return Ok(metadata);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _metadataService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var metadata = await _metadataService.GetByIdAsync(id);
            if (metadata == null) return NotFound();
            return Ok(metadata);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var results = await _metadataService.SearchAsync(term);
            return Ok(results);
        }

    }
}
