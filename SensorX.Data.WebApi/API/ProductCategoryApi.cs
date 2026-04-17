using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Categories.CreateCategory;
using SensorX.Data.Application.Commands.Categories.DeleteCategory;
using SensorX.Data.Application.Commands.Categories.SetParentCategory;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.WebApi.API;

public static class ProductCategoryApi
{
    public static RouteGroupBuilder MapProductCategoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Categories");

        api.MapPost("/productCategories", CreateCategory).WithOpenApi();
        api.MapPut("/productCategories/setParent", SetParent).WithOpenApi();
        api.MapDelete("/productCategories/{id:guid}", DeleteCategory).WithOpenApi();

        return api;
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateCategory(
        [FromBody] CreateCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> SetParent(
        [FromBody] SetParentCategoryCommand command,
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
        var command = new DeleteCategoryCommand { Id = id };
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }
}
