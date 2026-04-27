using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.DeactivateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.ExtendInternalPrice;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.InternalPrices.GetHistoryPriceForProduct;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceListStats;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceSuggest;
using SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

public static class InternalPriceApi
{
    public static RouteGroupBuilder MapInternalPriceApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Internal Prices");
        api.MapPost("/internalPrices/create", CreateInternalPrice)
            .WithOpenApi()
            .WithSummary("Create new internal price")
            .WithDescription("Tạo chính sách giá nội bộ mới cho sản phẩm, bao gồm giá đề xuất, giá sàn và các mức chiết khấu theo số lượng.");

        api.MapGet("/internalPrices/product/{productId:guid}", GetInternalPricesByProductId)
            .WithOpenApi()
            .WithSummary("Get all internal prices for a product")
            .WithDescription("Lấy tất cả các chính sách giá nội bộ liên kết với một ID sản phẩm cụ thể.");

        api.MapPatch("/internalPrices/{id:guid}/deactivate", DeactivateInternalPrice)
            .WithOpenApi()
            .WithSummary("Deactivate internal price")
            .WithDescription("Làm hết hạn ngay lập tức một chính sách giá nội bộ đang hoạt động bằng cách đặt ngày hết hạn về thời điểm hiện tại.");

        api.MapPatch("/internalPrices/{id:guid}/extend", ExtendInternalPrice)
            .WithOpenApi()
            .WithSummary("Extend internal price validity")
            .WithDescription("Gia hạn ngày hết hạn của một chính sách giá nội bộ bằng cách cung cấp ngày kết thúc mới hoặc khoảng thời gian.");

        api.MapGet("/internalPrices/list", GetPageListInternalPrice)
            .WithOpenApi()
            .WithSummary("Get paged list of internal prices")
            .WithDescription("Lấy danh sách phân trang của tất cả các chính sách giá nội bộ kèm chức năng tìm kiếm.");

        api.MapGet("/internalPrices/product/{productId:guid}/history", GetHistoryPriceForProduct)
            .WithOpenApi()
            .WithSummary("Get price history for a product")
            .WithDescription("Lấy lịch sử phân trang của tất cả các chính sách giá (đang hoạt động và đã hết hạn) cho một sản phẩm cụ thể.");

        api.MapGet("/internalPrices/stats", GetInternalPriceStats)
            .WithOpenApi()
            .WithSummary("Get internal price stats")
            .WithDescription("Lấy thống kê về các mức giá nội bộ.");

        api.MapPost("/internalPrices/suggest", GetInternalPriceSuggest)
            .WithOpenApi()
            .WithSummary("Get suggested internal prices for products")
            .WithDescription("Lấy danh sách các bảng giá đề xuất ưu tiên cho một danh sách các sản phẩm.");

        return api;
    }

    public static async Task<IResult> CreateInternalPrice(
        [FromBody] CreateInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetInternalPricesByProductId(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        Result<GetInternalPricesByProductIdResponse> result = await mediator.Send(new GetInternalPricesByProductIdQuery(productId));
        return result.ToResult();
    }

    private static async Task<IResult> DeactivateInternalPrice(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        Result result = await mediator.Send(new DeactivateInternalPriceCommand(id));
        return result.ToResult();
    }

    private static async Task<IResult> ExtendInternalPrice(
        [FromRoute] Guid id,
        [FromBody] ExtendInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        Result result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetPageListInternalPrice(
        [AsParameters] GetPageListInternalPriceQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<InternalPriceOffsetPagedResult> result = await mediator.Send(query);
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