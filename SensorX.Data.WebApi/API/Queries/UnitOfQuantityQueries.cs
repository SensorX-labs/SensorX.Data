using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.UnitOfQuantities.GetAllUnitOfQuantities;
using SensorX.Data.Application.Queries.UnitOfQuantities.GetPageListUnitOfQuantities;
using SensorX.Data.Application.Queries.UnitOfQuantities.GetUnitOfQuantityById;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class UnitOfQuantityQueries
{
    public static RouteGroupBuilder MapUnitOfQuantityQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/unit-of-quantities").WithTags("Unit Of Quantity Queries");

        api.MapGet("/list", GetPageListUnitOfQuantities)
            .WithOpenApi()
            .WithSummary("Get page list unit of quantities")
            .WithDescription("""
                - SearchTerm: Tìm nhanh theo tên/mô tả
                - Name: Lọc riêng theo tên đơn vị tính
                - Description: Lọc riêng theo mô tả
                - HasDescription: true để chỉ lấy bản ghi có mô tả, false để lấy bản ghi thiếu mô tả
                - IsUpdated: true để chỉ lấy bản ghi đã cập nhật, false để lấy bản ghi chưa cập nhật
                - CreatedFrom: Ngày tạo bắt đầu (yyyy-MM-dd)
                - CreatedTo: Ngày tạo kết thúc (yyyy-MM-dd)
                - PageNumber: Số trang để lấy (mặc định: 1)
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                """);

        api.MapGet("/list-all", GetAllUnitOfQuantities)
            .WithOpenApi()
            .WithSummary("Get all unit of quantities");

        api.MapGet("/{id:guid}", GetUnitOfQuantityById)
            .WithOpenApi()
            .WithSummary("Get unit of quantity by id");

        return api;
    }

    private static async Task<IResult> GetPageListUnitOfQuantities(
        [FromServices] IMediator mediator,
        [AsParameters] GetPageListUnitOfQuantitiesQuery query
    )
    {
        Result<OffsetPagedResult<GetPageListUnitOfQuantitiesResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetAllUnitOfQuantities(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetAllUnitOfQuantitiesResponse>> result = await mediator.Send(new GetAllUnitOfQuantitiesQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetUnitOfQuantityById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetUnitOfQuantityByIdQuery(id));
        return result.ToResult();
    }
}
