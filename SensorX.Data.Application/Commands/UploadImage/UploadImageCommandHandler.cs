using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.UploadImage;

public class UploadImageCommandHandler(
    ICloudinaryService _cloudinaryService
) : IRequestHandler<UploadImageCommand, Result<UploadImageSuccessResponse>>
{
    public async Task<Result<UploadImageSuccessResponse>> Handle(UploadImageCommand command, CancellationToken cancellationToken)
    {
        return await _cloudinaryService.UploadImageAsync(command.FileData, command.FileName, command.ContentType, command.Folder);
    }
}
