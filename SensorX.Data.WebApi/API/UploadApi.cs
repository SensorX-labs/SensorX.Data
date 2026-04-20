using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Uploads;

namespace SensorX.Data.WebApi.API
{
    public static class UploadApi
    {
        private static readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        public static RouteGroupBuilder MapUploadApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("upload").WithTags("Upload");

            api.MapPost("/image", UploadImage).DisableAntiforgery().WithOpenApi();
            return api;
        }

        [RequestSizeLimit(5 * 1024 * 1024)]
        [Consumes("multipart/form-data")]
        private static async Task<Results<Ok<UploadImageSuccessResponse>,BadRequest<UploadImageErrorResponse>,ProblemHttpResult>>
        UploadImage([FromForm] IFormFile? file, [FromServices] IMediator mediator)
        {
            if (file is null)
            {
                return TypedResults.BadRequest(new UploadImageErrorResponse("No file provided"));
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (
                string.IsNullOrWhiteSpace(fileExtension)
                || Array.IndexOf(AllowedExtensions, fileExtension.ToLowerInvariant()) < 0
            )
            {
                return TypedResults.BadRequest(
                    new UploadImageErrorResponse("Invalid file type. Allowed: .jpg, .jpeg, .png, .webp")
                );
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var command = new UploadImageCommand(
                    fileBytes,
                    file.FileName,
                    file.ContentType,
                    "sensorx"
                );

                var imageUrl = await mediator.Send(command);
                return TypedResults.Ok(new UploadImageSuccessResponse(imageUrl, "Upload successful"));
            }
            catch (ArgumentException ex)
            {
                return TypedResults.BadRequest(new UploadImageErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(
                    title: "Upload failed",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private sealed record UploadImageSuccessResponse(string Url, string Message);

        private sealed record UploadImageErrorResponse(string Error);
    }
}