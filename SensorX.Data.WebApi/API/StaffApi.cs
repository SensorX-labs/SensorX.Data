using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.CreateStaff;
using SensorX.Data.Application.Commands.Staffs.UpdateStaff;
using SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;
using SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;
using SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;
using SensorX.Data.Application.Queries.Staffs.GetStaffByAccountId;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

public static class StaffApi
{
    public static RouteGroupBuilder MapStaffApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("staff").WithTags("Staff");

        api.MapPost("/create", CreateStaff).WithOpenApi();
        api.MapGet("/list", GetPageListStaffs).WithOpenApi();
        api.MapGet("/{staffId:guid}/metrics", GetEmployeeMetrics).WithOpenApi();
        api.MapGet("/{staffId:guid}", GetStaffById).WithOpenApi();
        api.MapGet("/account/{accountId:guid}", GetStaffByAccountId).WithOpenApi();
        api.MapPut("", UpdateStaff).WithOpenApi();
        api.MapDelete("/{staffId:guid}", DeleteStaff).WithOpenApi();
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
        var result = await mediator.Send(query);
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
