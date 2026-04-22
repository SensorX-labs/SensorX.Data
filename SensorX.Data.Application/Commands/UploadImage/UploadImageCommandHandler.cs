using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;

namespace SensorX.Data.Application.Commands.UploadImage;

public class UploadImageCommandHandler(
    ICloudinaryService _cloudinaryService
) : IRequestHandler<UploadImageCommand, Result<UploadImageSuccessResponse, UploadImageErrorResponse>>
{
    public async Task<Result<UploadImageSuccessResponse, UploadImageErrorResponse>> Handle(UploadImageCommand command, CancellationToken cancellationToken)
    {
        return await _cloudinaryService.UploadImageAsync(command.FileData, command.FileName, command.ContentType, command.Folder);
    }
}