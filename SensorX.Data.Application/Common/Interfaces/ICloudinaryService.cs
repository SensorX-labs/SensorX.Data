using SensorX.Data.Application.Commands.UploadImage;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<Result<string>> UploadImageAsync(byte[] fileData, string fileName, string contentType, string? folder = null, CancellationToken cancellationToken = default);
}
