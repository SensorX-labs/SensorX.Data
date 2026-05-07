using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Services;

public static class ImageService
{
    public static RouteGroupBuilder MapImageService(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("image").WithTags("Image Services");

        api.MapPost("/upload", UploadImage).DisableAntiforgery().WithOpenApi();
        api.MapDelete("/delete", DeleteImage).WithOpenApi();
        api.MapDelete("/delete-multiple", DeleteImages).WithOpenApi();
        return api;
    }

    [RequestSizeLimit(5 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    private static async Task<IResult> UploadImage(
        [FromForm] IFormFile? file,
        [FromServices] ICloudinaryService cloudinaryService
    )
    {
        if (file is null)
        {
            return TypedResults.BadRequest("No file provided");
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        Result<string> result = await cloudinaryService.UploadImageAsync(fileBytes, file.FileName, file.ContentType, "sensorx");
        return result.ToResult();
    }

    private static async Task<IResult> DeleteImage(
        [FromBody] string url,
        [FromServices] ICloudinaryService cloudinaryService
    )
    {
        if (url is null)
        {
            return TypedResults.BadRequest("No file provided");
        }

        Result<bool> result = await cloudinaryService.DeleteImageAsync(url);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteImages(
        [FromBody] List<string> urls,
        [FromServices] ICloudinaryService cloudinaryService
    )
    {
        if (urls is null)
        {
            return TypedResults.BadRequest("No file provided");
        }

        Result<bool> result = await cloudinaryService.DeleteImagesAsync(urls);
        return result.ToResult();
    }
}