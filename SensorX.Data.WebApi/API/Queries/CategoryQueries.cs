using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Categories.GetAllCategories;
using SensorX.Data.Application.Queries.Categories.LoadMoreForModal;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class CategoryQueries
{
    public static RouteGroupBuilder MapCategoryQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/categories").WithTags("Categories Queries");

        api.MapGet("/list-all", GetAllCategories)
            .WithOpenApi()
            .WithSummary("Get all categories without pagination");

        api.MapGet("/load-more-for-modal", LoadMoreCategoriesForModal)
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
