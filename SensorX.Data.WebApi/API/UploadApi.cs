using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using SensorX.Data.Application.Features.Uploads;
using SensorX.Data.Application.Common.ResponseClient;
=======
using SensorX.Data.Application.Commands.UploadImage;
>>>>>>> 670d248c2d659306b94af4fd9a94f1ace1b62395

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
<<<<<<< HEAD
        private static async Task<Results<Ok<Result<string>>, BadRequest<string>>>
=======
        private static async Task<Results<Ok<UploadImageSuccessResponse>, BadRequest<UploadImageErrorResponse>, ProblemHttpResult>>
>>>>>>> 670d248c2d659306b94af4fd9a94f1ace1b62395
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