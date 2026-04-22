using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.UploadImage;

namespace SensorX.Data.WebApi.API
{
    public static class UploadApi
    {
        public static RouteGroupBuilder MapUploadApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("upload").WithTags("Upload");

            api.MapPost("/image", UploadImage).DisableAntiforgery().WithOpenApi();
            return api;
        }

        [RequestSizeLimit(5 * 1024 * 1024)]
        [Consumes("multipart/form-data")]
        private static async Task<Results<Ok<string>, BadRequest<string>, ProblemHttpResult>>
        UploadImage([FromForm] IFormFile? file, [FromServices] IMediator mediator)
        {
            if (file is null)
            {
                return TypedResults.BadRequest("No file provided");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var command = new UploadImageCommand(
                fileBytes,
                file.FileName,
                file.ContentType,
                "sensorx"
            );

            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result.Value);
            }
            else
            {
                return TypedResults.BadRequest(result.Message ?? "Unknown error");
            }
        }
    }
}