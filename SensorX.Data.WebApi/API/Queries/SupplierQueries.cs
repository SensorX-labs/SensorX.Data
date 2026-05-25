using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Suppliers.GetAllSuppliers;
using SensorX.Data.Application.Queries.Suppliers.GetPageListSuppliers;
using SensorX.Data.Application.Queries.Suppliers.GetSupplierById;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class SupplierQueries
{
    public static RouteGroupBuilder MapSupplierQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/suppliers").WithTags("Supplier Queries");

        api.MapGet("/list", GetPageListSuppliers)
            .WithOpenApi()
            .WithSummary("Get page list suppliers")
            .WithDescription("""
                - SearchTerm: Lọc theo tên/mô tả
                - HasDescription: true để chỉ lấy bản ghi có mô tả, false để lấy bản ghi thiếu mô tả
                - IsUpdated: true để chỉ lấy bản ghi đã cập nhật, false để lấy bản ghi chưa cập nhật
                - CreatedFrom: Ngày tạo bắt đầu (yyyy-MM-dd)
                - CreatedTo: Ngày tạo kết thúc (yyyy-MM-dd)
                - PageNumber: Số trang để lấy (mặc định: 1)
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                """);

        api.MapGet("/list-all", GetAllSuppliers)
            .WithOpenApi()
            .WithSummary("Get all suppliers");

        api.MapGet("/{id:guid}", GetSupplierById)
            .WithOpenApi()
            .WithSummary("Get supplier by id");

        return api;
    }

    private static async Task<IResult> GetPageListSuppliers(
        [FromServices] IMediator mediator,
        [AsParameters] GetPageListSuppliersQuery query
    )
    {
        Result<OffsetPagedResult<GetPageListSuppliersResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetAllSuppliers(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetAllSuppliersResponse>> result = await mediator.Send(new GetAllSuppliersQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetSupplierById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetSupplierByIdQuery(id));
        return result.ToResult();
    }
}
