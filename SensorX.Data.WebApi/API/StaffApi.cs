using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.CreateStaff;
using SensorX.Data.Application.Commands.Staffs.UpdateStaff;
using SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;
using SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;
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

        private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> UpdateStaff(
            [FromBody] UpdateStaffCommand command,
            [FromServices] IMediator mediator
        )
        {
            Result<Guid> result = await mediator.Send(command);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Message);
        }

        private static async Task<Results<Ok<Result<bool>>, BadRequest<string>>> DeleteStaff(
            [FromRoute] Guid staffId,
            [FromServices] IMediator mediator
        )
        {
            var command = new SensorX.Data.Application.Commands.Staffs.DeleteStaff.DeleteStaffCommand(staffId);
            Result<bool> result = await mediator.Send(command);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Message);
        }
    }
}
