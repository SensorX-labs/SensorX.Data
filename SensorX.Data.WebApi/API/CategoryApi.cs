using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Categories.CreateCategory;
using SensorX.Data.Application.Commands.Categories.DeleteCategory;
using SensorX.Data.Application.Commands.Categories.SetParentCategory;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Queries.Categories.GetAllCategories;
using SensorX.Data.Application.Queries.Categories.GetPageListCategories;
using SensorX.Data.Application.Queries.Categories.LoadMoreForModal;
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
                - Name: Tên danh mục (phải là duy nhất)
                - ParentId: Tùy chọn, đặt danh mục cha
                - Description: Tùy chọn mô tả
                """);

        api.MapPut("/categories/{id:guid}/parent", SetParent)
            .WithOpenApi()
            .WithSummary("Set parent category")
            .WithDescription("""
                - ParentId: Null để di chuyển danh mục về thư mục gốc
                """);

        api.MapDelete("/categories/{id:guid}", DeleteCategory)
            .WithOpenApi()
            .WithSummary("Delete category");

        api.MapGet("/categories/list", GetPageListCategories)
            .WithOpenApi()
            .WithSummary("Get page list categories")
            .WithDescription("""
                - SearchTerm: Lọc theo tên/mô tả
                - PageNumber: Số trang để lấy (mặc định: 1)
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                """);

        api.MapGet("/categories/list-all", GetAllCategories)
            .WithOpenApi()
            .WithSummary("Get all categories without pagination");

        api.MapGet("/categories/load-more-for-modal", LoadMoreCategoriesForModal)
            .WithOpenApi()
            .WithSummary("Load more categories for modal")
            .WithDescription("""
                - SearchTerm: Lọc theo tên danh mục
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                - LastValue: Giá trị cuối cùng của danh mục trước đó (được mã hóa cursor từ Id và CreatedAt)
                - LastId: ID cuối cùng của danh mục trước đó
                - IsDescending: true nếu sắp xếp giảm dần, false nếu sắp xếp tăng dần
                """);
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
        Result<OffsetPagedResult<GetPageListCategoriesResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetAllCategories(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetAllCategoriesResponse>> result = await mediator.Send(new GetAllCategoriesQuery());
        return result.ToResult();
    }

    private static async Task<IResult> LoadMoreCategoriesForModal(
        [AsParameters] LoadMoreForModalQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<LoadMoreForModalResult> result = await mediator.Send(query);
        return result.ToResult();
    }
}
