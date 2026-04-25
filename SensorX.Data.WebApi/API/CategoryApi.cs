using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Categories.CreateCategory;
using SensorX.Data.Application.Commands.Categories.DeleteCategory;
using SensorX.Data.Application.Commands.Categories.SetParentCategory;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Categories.GetAllCategories;
using SensorX.Data.Application.Queries.Categories.GetPageListCategories;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

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

        api.MapPut("/categories/{id:guid}/parent", SetParent)
            .WithOpenApi()
            .WithSummary("Set parent category")
            .WithDescription("""
                - ParentId: Null to move category to root
                """);

        api.MapDelete("/categories/{id:guid}", DeleteCategory)
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

        api.MapGet("/categories/list-all", GetAllCategories)
            .WithOpenApi()
            .WithSummary("Get all categories without pagination");

        return api;
    }

    private static async Task<IResult> CreateCategory(
        [FromBody] CreateCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> SetParent(
        [FromRoute] Guid id,
        [FromBody] SetParentCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        Result result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteCategory(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        Result result = await mediator.Send(new DeleteCategoryCommand(id));
        return result.ToResult();
    }

    private static async Task<IResult> GetPageListCategories(
        [AsParameters] GetPageListCategoriesQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<CategoryOffsetPagedResult> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetAllCategories(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetAllCategoriesResponse>> result = await mediator.Send(new GetAllCategoriesQuery());
        return result.ToResult();
    }
}
