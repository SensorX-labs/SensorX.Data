using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Categories.CreateCategory;
using SensorX.Data.Application.Commands.Categories.DeleteCategory;
using SensorX.Data.Application.Commands.Categories.SetParentCategory;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class CategoryCommands
{
    public static RouteGroupBuilder MapCategoryCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/categories").WithTags("Category Commands");

        api.MapPost("/create", CreateCategory)
            .WithOpenApi()
            .WithSummary("Create category")
            .WithDescription("""
                - Name: Tên danh mục (phải là duy nhất)
                - ParentId: Tùy chọn, đặt danh mục cha
                - Description: Tùy chọn mô tả
                """);

        api.MapPut("/{id:guid}/parent", SetParent)
            .WithOpenApi()
            .WithSummary("Set parent category")
            .WithDescription("""
                - ParentId: Null để di chuyển danh mục về thư mục gốc
                """);

        api.MapDelete("/{id:guid}", DeleteCategory)
            .WithOpenApi()
            .WithSummary("Delete category");

        return api;
    }

    private static async Task<IResult> CreateCategory(
        [FromBody] CreateCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> SetParent(
        [FromRoute] Guid id,
        [FromBody] SetParentCategoryCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteCategory(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new DeleteCategoryCommand(id));
        return result.ToResult();
    }
}
