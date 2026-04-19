using MediatR;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SensorX.Data.Application.Commands.Uploads;

namespace SensorX.Data.Application.Handlers.Uploads
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>
    {
        private readonly ICloudinaryService _cloudinaryService;

        public UploadImageCommandHandler(ICloudinaryService cloudinaryService, ILogger<UploadImageCommandHandler> logger)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<string> Handle(UploadImageCommand command, CancellationToken cancellationToken)
        {
            return await _cloudinaryService.UploadImageAsync(command.FileData, command.FileName, command.ContentType, command.Folder);
        }
    }
}