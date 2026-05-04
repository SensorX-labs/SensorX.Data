using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Products.ChangeProductStatus;
using SensorX.Data.Application.Commands.Products.CreateProduct;
using SensorX.Data.Application.Commands.Products.DeleteProduct;
using SensorX.Data.Application.Commands.Products.UpdateProduct;
using SensorX.Data.Application.Common.QueryExtensions.KeysetPagination;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Products.GetPageListProducts;
using SensorX.Data.Application.Queries.Products.GetPageProductDetail;
using SensorX.Data.Application.Queries.Products.GetProductDetail;
using SensorX.Data.Application.Queries.Products.GetProductListStats;
using SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;
using SensorX.Data.Application.Queries.Products.LoadMoreProducts;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class ProductQueries
{
    public static RouteGroupBuilder MapProductQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/products").WithTags("Product Queries");

        api.MapGet("/list", GetPageListProducts)
            .WithOpenApi()
            .WithSummary("Get page list products")
            .WithDescription("""
                - SearchTerm: Lọc theo tên/mô tả
                - PageNumber: Số trang để lấy (mặc định: 1)
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                """);

        api.MapPost("/pricing-policy/batch", GetProductPricingPolicy)
            .WithOpenApi()
            .WithSummary("Get product pricing policy")
            .WithDescription("""
                - ProductIds: Danh sách ID sản phẩm
                """);

        api.MapGet("/{id:guid}", GetProductDetail)
            .WithOpenApi()
            .WithSummary("Get product detail basic information")
            .WithDescription("Lấy thông tin cơ bản của sản phẩm");

        api.MapGet("/load-more-products-for-modal", LoadMoreProducts)
            .WithOpenApi()
            .WithSummary("Load more products")
            .WithDescription("""
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                - SearchTerm: Tìm kiếm theo tên/mã sản phẩm/Hãng
                - LastValue: Giá trị của trường được sort tại mục cuối cùng (dùng để load trang tiếp theo)
                - LastId: ID của mục cuối cùng (dùng để load trang tiếp theo)
                - IsDescending: true để sort giảm dần, false để sort tăng dần (mặc định: true)
                """);

        api.MapGet("/list-stats", GetProductListStats)
            .WithOpenApi()
            .WithSummary("Get product list stats")
            .WithDescription("Lấy thông tin thống kê danh sách sản phẩm");

        api.MapGet("/detail/{id:guid}", GetPageProductDetail)
            .WithOpenApi()
            .WithSummary("Get product detail for product page")
            .WithDescription("Lấy thông tin chi tiết sản phẩm bao gồm cả thông tin đề xuất giá nội bộ để hiển thị trên trang chi tiết sản phẩm.");

        return api;
    }

    private static async Task<IResult> GetPageListProducts(
        [FromServices] IMediator mediator,
        [AsParameters] GetPageListProductsQuery query
    )
    {
        Result<OffsetPagedResult<GetPageListProductsResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetProductDetail(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        Result<GetProductDetailResponse> result = await mediator.Send(new GetProductDetailQuery(id));
        return result.ToResult();
    }

    private static async Task<IResult> GetProductPricingPolicy(
        [FromBody] GetProductPricingPolicyQuery query,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> LoadMoreProducts(
        [FromServices] IMediator mediator,
        [AsParameters] LoadMoreProductsQuery query
    )
    {
        Result<KeysetPagedResult<LoadMoreProductsResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetProductListStats(
        [FromServices] IMediator mediator,
        [AsParameters] GetProductListStatsQuery query
    )
    {
        Result<ProductListStatsResponse> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetPageProductDetail(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetPageProductDetailQuery(id));
        return result.ToResult();
    }
}