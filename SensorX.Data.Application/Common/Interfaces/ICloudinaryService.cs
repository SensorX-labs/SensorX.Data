using SensorX.Data.Application.Commands.UploadImage;

namespace SensorX.Data.Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<Result<UploadImageSuccessResponse, UploadImageErrorResponse>> UploadImageAsync(byte[] fileData, string fileName, string contentType, string? folder = null, CancellationToken cancellationToken = default);
}
