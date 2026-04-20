using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SensorX.Data.Application.Common.Interfaces;

namespace SensorX.Data.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _defaultFolder;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
        {
            var cloudName = configuration["Cloudinary:CloudName"] ?? throw new ArgumentException("Cloudinary CloudName is required", "CloudName");
            var apiKey = configuration["Cloudinary:ApiKey"] ?? throw new ArgumentException("Cloudinary ApiKey is required", "ApiKey");
            var apiSecret = configuration["Cloudinary:ApiSecret"] ?? throw new ArgumentException("Cloudinary ApiSecret is required", "ApiSecret");

            _defaultFolder = configuration["Cloudinary:Folder"] ?? "sensorx";
            _logger = logger;

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImageAsync(byte[] fileData, string fileName, string contentType, string? folder = null)
        {
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("File data is required and cannot be empty", nameof(fileData));

            var uploadFolder = string.IsNullOrWhiteSpace(folder) ? _defaultFolder : folder;

            using var memoryStream = new MemoryStream(fileData);
            
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, memoryStream),
                Folder = uploadFolder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError("Cloudinary upload error: {Error}", uploadResult.Error.Message);
                throw new InvalidOperationException($"Upload failed: {uploadResult.Error.Message}");
            }

            _logger.LogInformation("Image uploaded successfully to {Folder}", uploadFolder);
            return uploadResult.SecureUrl?.ToString() ?? throw new InvalidOperationException("Upload result URL is null");
        }
    }

    public class CloudinarySettings
    {
        public string CloudName { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ApiSecret { get; set; } = string.Empty;
        public string? Folder { get; set; }
    }
}