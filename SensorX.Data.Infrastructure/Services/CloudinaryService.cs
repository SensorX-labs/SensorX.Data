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
    public async Task<Result<bool>> DeleteImageAsync(string imageUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Result<bool>.Failure("Image URL is required");
        }

        try
        {
            var publicId = ExtractPublicIdFromUrl(imageUrl);
            if (string.IsNullOrEmpty(publicId))
            {
                _logger.LogWarning("Could not extract public ID from URL: {Url}", imageUrl);
                // If we can't extract, maybe it's not a Cloudinary URL or already deleted/invalid format
                // But we should return success if we want to ignore errors for non-existent images
                return Result<bool>.Failure("Could not extract public ID from URL");
            }

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                _logger.LogError("Cloudinary delete error: {Error}", result.Error.Message);
                return Result<bool>.Failure(result.Error.Message);
            }

            bool isDeleted = result.Result == "ok";
            if (isDeleted)
            {
                _logger.LogInformation("Image deleted successfully: {PublicId}", publicId);
            }
            else
            {
                _logger.LogWarning("Image deletion result was not 'ok': {Result} for {PublicId}", result.Result, publicId);
            }

            return Result<bool>.Success(isDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during Cloudinary deletion");
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteImagesAsync(List<string> imageUrls, CancellationToken cancellationToken = default)
    {
        if (imageUrls == null || imageUrls.Count == 0)
        {
            return Result<bool>.Success(true);
        }

        try
        {
            var publicIds = imageUrls
                .Select(ExtractPublicIdFromUrl)
                .Where(id => !string.IsNullOrEmpty(id))
                .Cast<string>()
                .ToList();

            if (publicIds.Count == 0)
            {
                return Result<bool>.Success(true); // Nothing to delete
            }

            var delResParams = new DelResParams
            {
                PublicIds = publicIds,
                Type = "upload",
                ResourceType = ResourceType.Image
            };

            var result = await _cloudinary.DeleteResourcesAsync(delResParams, cancellationToken);

            if (result.Error != null)
            {
                _logger.LogError("Cloudinary bulk delete error: {Error}", result.Error.Message);
                return Result<bool>.Failure(result.Error.Message);
            }

            _logger.LogInformation("Successfully processed bulk deletion for {Count} images", publicIds.Count);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during Cloudinary bulk deletion");
            return Result<bool>.Failure(ex.Message);
        }
    }

    private string? ExtractPublicIdFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex == -1) return null;

            var startIndex = uploadIndex + 1;
            // Skip version segment if present (e.g., v12345678)
            if (segments.Length > startIndex && segments[startIndex].StartsWith("v") && segments[startIndex].Length > 1 && char.IsDigit(segments[startIndex][1]))
            {
                startIndex++;
            }

            var publicIdWithExtension = string.Join("/", segments.Skip(startIndex));
            var lastDotIndex = publicIdWithExtension.LastIndexOf('.');
            return lastDotIndex != -1 ? publicIdWithExtension.Substring(0, lastDotIndex) : publicIdWithExtension;
        }
        catch
        {
            return null;
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
