using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.UploadImage;

public sealed record UploadImageCommand(
    byte[] FileData,
    string FileName,
    string ContentType,
    string? Folder = "sensorx"
) : IRequest<Result<string>>;
