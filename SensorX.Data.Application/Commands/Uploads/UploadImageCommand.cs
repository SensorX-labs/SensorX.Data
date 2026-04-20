using MediatR;

namespace SensorX.Data.Application.Commands.Uploads
{
    public record UploadImageCommand(
        byte[] FileData,
        string FileName,
        string ContentType,
        string? Folder = "sensorx"
    ) : IRequest<string>;
}