using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Features.Uploads;
using SensorX.Data.Application.Common.ResponseClient;

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
        private static async Task<Results<Ok<Result<string>>, BadRequest<string>>>
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
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result.Error);
        }
    }
}