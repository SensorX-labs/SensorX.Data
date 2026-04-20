using System.Threading.Tasks;

namespace SensorX.Data.Application.Common.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(byte[] fileData, string fileName, string contentType, string? folder = null);
    }
}
