
using ImageGallery.API.Configuration;
using ImageGallery.API.Services;

namespace ImageGallery.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            // Add services to the container.
            builder.Services.Configure<BlobStorageOptions>(
                builder.Configuration.GetSection("Azure:BlobStorage"));
            builder.Services.Configure<CosmosDbOptions>(
                builder.Configuration.GetSection("Azure:CosmosDb"));
            builder.Services.Configure<ComputerVisionOptions>(
                builder.Configuration.GetSection("Azure:ComputerVision"));

            builder.Services.AddSingleton<ImageStorageService>();
            builder.Services.AddSingleton<ImageMetadataService>();
            builder.Services.AddSingleton<ImageAnalysisService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
