using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.DeactivateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.ExtendInternalPrice;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.InternalPrices.GetHistoryPriceForProduct;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceListStats;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceSuggest;
using SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class InternalPriceQueries
{
    public static RouteGroupBuilder MapInternalPriceQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/internal-prices").WithTags("Internal Prices");

        api.MapGet("/product/{productId:guid}", GetInternalPricesByProductId)
            .WithOpenApi()
            .WithSummary("Get all internal prices for a product")
            .WithDescription("Lấy tất cả các chính sách giá nội bộ liên kết với một ID sản phẩm cụ thể.");

        api.MapGet("/list", GetPageListInternalPrice)
            .WithOpenApi()
            .WithSummary("Get paged list of internal prices")
            .WithDescription("Lấy danh sách phân trang của tất cả các chính sách giá nội bộ kèm chức năng tìm kiếm.");

        api.MapGet("/product/{productId:guid}/history", GetHistoryPriceForProduct)
            .WithOpenApi()
            .WithSummary("Get price history for a product")
            .WithDescription("Lấy lịch sử phân trang của tất cả các chính sách giá (đang hoạt động và đã hết hạn) cho một sản phẩm cụ thể.");

        api.MapGet("/stats", GetInternalPriceStats)
            .WithOpenApi()
            .WithSummary("Get internal price stats")
            .WithDescription("Lấy thống kê về các mức giá nội bộ.");

        api.MapPost("/suggest", GetInternalPriceSuggest)
            .WithOpenApi()
            .WithSummary("Get suggested internal prices for products")
            .WithDescription("Lấy danh sách các bảng giá đề xuất ưu tiên cho một danh sách các sản phẩm.");

        return api;
    }

    private static async Task<IResult> GetInternalPricesByProductId(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        Result<GetInternalPricesByProductIdResponse> result = await mediator.Send(new GetInternalPricesByProductIdQuery(productId));
        return result.ToResult();
    }

    private static async Task<IResult> GetPageListInternalPrice(
        [AsParameters] GetPageListInternalPriceQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<OffsetPagedResult<GetPageListInternalPriceResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetHistoryPriceForProduct(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        Result<GetHistoryPriceForProductResponse> result = await mediator.Send(new GetHistoryPriceForProductQuery(productId));
        return result.ToResult();
    }

    private static async Task<IResult> GetInternalPriceStats(
        [FromServices] IMediator mediator
    )
    {
        Result<GetInternalPriceListStatsResponse> result = await mediator.Send(new GetInternalPriceListStatsQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetInternalPriceSuggest(
        [FromBody] GetInternalPriceSuggestQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<List<InternalPriceResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }
}