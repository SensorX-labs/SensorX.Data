using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Commands.CreateProductCategory;
using SensorX.Data.Application.Commands.UpdateProductCategory;
using SensorX.Data.Application.Commands.DeleteProductCategory;

namespace SensorX.Data.WebApi.API;

public static class ProductCategoryApi
{
    public static RouteGroupBuilder MapProductCategoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog").WithTags("Catalog");

        api.MapPost("/productCategories", CreateCategory).WithOpenApi();
        api.MapPut("/productCategories", UpdateCategory).WithOpenApi();
        api.MapDelete("/productCategories", DeleteCategory).WithOpenApi();

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

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> UpdateCategory(
        [FromBody] UpdateProductCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> DeleteCategory(
        [FromBody] DeleteProductCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }
}
