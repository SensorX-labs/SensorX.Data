using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Categories.CreateCategory;
using SensorX.Data.Application.Commands.Categories.DeleteCategory;
using SensorX.Data.Application.Commands.Categories.SetParentCategory;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Categories.GetPageListCategories;

namespace SensorX.Data.WebApi.API.CategoryApi;

public static class CategoryApi
{
    public static RouteGroupBuilder MapCategoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Categories");

        api.MapPost("/categories/create", CreateCategory)
            .WithOpenApi()
            .WithSummary("Create category")
            .WithDescription("""
                - Name: Category name (must be unique)
                - ParentId: Optional, set parent category
                - Description: Optional description
                """);

        api.MapPut("/categories/set-parent", SetParent)
            .WithOpenApi()
            .WithSummary("Set parent category")
            .WithDescription("""
                - ParentId: Null to move category to root
                """);

        api.MapDelete("/categories/delete/{id:guid}", DeleteCategory)
            .WithOpenApi()
            .WithSummary("Delete category");

        api.MapGet("/categories/list", GetPageListCategories)
            .WithOpenApi()
            .WithSummary("Get page list categories")
            .WithDescription("""
                - SearchTerm: Filter by name/description
                - PageNumber: The page number to retrieve (default: 1)
                - PageSize: Number of items per page (default: 10)
                """);

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

    private static async Task<Results<Ok<Result>, BadRequest<string>>> SetParent(
        [FromBody] SetParentCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result>, BadRequest<string>>> DeleteCategory(
        [AsParameters] DeleteCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<CategoryOffsetPagedResult>>, BadRequest<string>>> GetPageListCategories(
        [AsParameters] GetPageListCategoriesQuery query,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }
}
