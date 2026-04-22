using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Infrastructure.Services;

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
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<Result<string>> UploadImageAsync(byte[] fileData, string fileName, string contentType, string? folder = null, CancellationToken cancellationToken = default)
    {
        if (fileData == null || fileData.Length == 0)
        {
            return Result<string>.Failure("File data is required and cannot be empty");
        }

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

        try
        {
            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (uploadResult.Error != null)
            {
                _logger.LogError("Cloudinary upload error: {Error}", uploadResult.Error.Message);
                return Result<string>.Failure(uploadResult.Error.Message);
            }

            var secureUrl = uploadResult.SecureUrl?.ToString();
            if (string.IsNullOrEmpty(secureUrl))
            {
                _logger.LogError("Cloudinary upload result URL is null");
                return Result<string>.Failure("Upload result URL is null");
            }

            _logger.LogInformation("Image uploaded successfully to {Folder}", uploadFolder);
            return Result<string>.Success(secureUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during Cloudinary upload");
            return Result<string>.Failure(ex.Message);
        }
    }
}

public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
    public string? Folder { get; set; }
}
