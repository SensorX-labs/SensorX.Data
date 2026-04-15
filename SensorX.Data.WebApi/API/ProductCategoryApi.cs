using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Commands.CreateProductCategory;
using SensorX.Data.Application.Commands.SetParentProductCategory;
using SensorX.Data.Application.Commands.DeleteProductCategory;

namespace SensorX.Data.WebApi.API;

public static class ProductCategoryApi
{
    public static RouteGroupBuilder MapProductCategoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog").WithTags("Categories");

        api.MapPost("/productCategories", CreateCategory).WithOpenApi();
        api.MapPut("/productCategories/setParent", SetParent).WithOpenApi();
        api.MapDelete("/productCategories/{id:guid}", DeleteCategory).WithOpenApi();

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

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> SetParent(
        [FromBody] SetParentProductCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> DeleteCategory(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var command = new DeleteProductCategoryCommand { Id = id };
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }
}
