using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.CreateStaff;
using SensorX.Data.Application.Commands.Staffs.UpdateStaff;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;
using SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;
using SensorX.Data.Application.Queries.Staffs.GetStaffByAccountId;
using SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class StaffQueries
{
    public static RouteGroupBuilder MapStaffQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("staff").WithTags("Staff Queries");

        api.MapGet("/list", GetPageListStaffs).WithOpenApi();
        api.MapGet("/{staffId:guid}/metrics", GetEmployeeMetrics).WithOpenApi();
        api.MapGet("/{staffId:guid}", GetStaffById).WithOpenApi();
        api.MapGet("/account/{accountId:guid}", GetStaffByAccountId).WithOpenApi();
        return api;
    }

    private static async Task<IResult> CreateStaff(
        [FromBody] CreateStaffCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetPageListStaffs(
        [AsParameters] GetPageListStaffsQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<OffsetPagedResult<GetPageListStaffsResponse>> result = await mediator.Send(query);
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

    private static async Task<IResult> UpdateStaff(
        [FromBody] UpdateStaffCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteStaff(
        [FromRoute] Guid staffId,
        [FromServices] IMediator mediator
    )
    {
        var command = new SensorX.Data.Application.Commands.Staffs.DeleteStaff.DeleteStaffCommand(staffId);
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetStaffById(
        [FromRoute] Guid staffId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetDetailStaffByIdQuery(staffId));
        return result.ToResult();
    }

    private static async Task<IResult> GetStaffByAccountId(
        [FromRoute] Guid accountId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetStaffByAccountIdQuery(accountId));
        return result.ToResult();
    }
}
