using MediatR;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace SensorX.Data.Application.Features.Uploads
{
    public class UploadImageHandler : IRequestHandler<UploadImageCommand, Result<string>>
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public UploadImageHandler(ICloudinaryService cloudinaryService, ILogger<UploadImageHandler> logger)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<string>> Handle(UploadImageCommand command, CancellationToken cancellationToken)
        {
            if (command.FileData is null || command.FileData.Length == 0)
            {
                return Result<string>.Failure("No file provided");
            }

            var fileExtension = System.IO.Path.GetExtension(command.FileName);
            if (string.IsNullOrWhiteSpace(fileExtension) || Array.IndexOf(_allowedExtensions, fileExtension.ToLowerInvariant()) < 0)
            {
                return Result<string>.Failure("Invalid file type. Allowed: .jpg, .jpeg, .png, .webp");
            }

            try
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(command.FileData, command.FileName, command.ContentType, command.Folder, cancellationToken);
                return Result<string>.Success(imageUrl);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message);
            }
        }
    }
}
