using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.ResponseClient;
using Sensorx.Data.Application.Commands.CreateProductCategory;

namespace SensorX.Data.WebApi.API;

public static class ProductCategoryApi
{
    public static RouteGroupBuilder MapProductCategoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog").WithTags("Catalog");

        api.MapPost("/productCategories", CreateCategory).WithOpenApi();
        
        return api;
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateCategory(
        [FromBody] CreateProductCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }
}
