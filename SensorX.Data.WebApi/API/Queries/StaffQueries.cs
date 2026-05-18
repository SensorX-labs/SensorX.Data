using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;
using SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;
using SensorX.Data.Application.Queries.Staffs.GetProfile;
using SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;
using SensorX.Data.Application.Queries.Staffs.GetStaffListStats;
using SensorX.Data.WebApi.Configurations;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class StaffQueries
{
    public static RouteGroupBuilder MapStaffQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("staff").WithTags("Staff Queries");

        api.MapGet("/list", GetPageListStaffs)
            .WithOpenApi()
            .WithSummary("Get page list staffs")
            .WithDescription("""
                - SearchTerm: Lọc theo tên/mã nhân viên/email/điện thoại
                - PageNumber: Số trang để lấy (mặc định: 1)
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                """);

        api.MapGet("/list-stats", GetStaffListStats)
            .WithOpenApi()
            .WithSummary("Get staff list stats")
            .WithDescription("Lấy thông tin thống kê danh sách nhân viên");

        api.MapGet("/{staffId:guid}/metrics", GetEmployeeMetrics).WithOpenApi();
        api.MapGet("/{staffId:guid}", GetStaffById).WithOpenApi();
        api.MapGet("/profile", GetProfile).WithOpenApi();
        return api;
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> GetPageListStaffs(
        [AsParameters] GetPageListStaffsQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<OffsetPagedResult<GetPageListStaffsResponse>> result = await mediator.Send(query);
        return result.ToResult();
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> GetStaffListStats(
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetStaffListStatsQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetEmployeeMetrics(
        [FromRoute] Guid staffId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetStaffMetricsQuery(staffId));
        return result.ToResult();
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> GetStaffById(
        [FromRoute] Guid staffId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetDetailStaffByIdQuery(staffId));
        return result.ToResult();
    }

    [AuthorizeRole(Role.Manager, Role.SaleStaff, Role.WarehouseStaff)]
    private static async Task<IResult> GetProfile(
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetProfileQuery());
        return result.ToResult();
    }
}
