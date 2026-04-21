using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Categories.CreateCategory;
using SensorX.Data.Application.Commands.Categories.DeleteCategory;
using SensorX.Data.Application.Commands.Categories.SetParentCategory;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Categories.GetPageListCategories;

namespace SensorX.Data.WebApi.API;

public static class CategoryApi
{
    public static RouteGroupBuilder MapCategoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Categories");

        api.MapPost("/categories/create", CreateCategory).WithOpenApi();
        api.MapPut("/categories/setParent", SetParent).WithOpenApi();
        api.MapDelete("/categories/delete/{id:guid}", DeleteCategory).WithOpenApi();
        api.MapGet("/categories/getPageList", GetPageListCategories).WithOpenApi();

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
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> SetParent(
        [FromBody] SetParentCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
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
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<CategoryCursorPagedResult>>, BadRequest<string>>> GetPageListCategories(
        [AsParameters] GetPageListCategoriesQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<CategoryCursorPagedResult> result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }
}
