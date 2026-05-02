using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.CreateStaff;
using SensorX.Data.Application.Commands.Staffs.UpdateStaff;
using SensorX.Data.Application.Commands.Staffs.DeleteStaff;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class StaffCommands
{
    public static RouteGroupBuilder MapStaffCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("staff").WithTags("Staff Commands");

        api.MapPost("/create", CreateStaff).WithOpenApi();
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
        var result = await mediator.Send(new DeleteStaffCommand(staffId));
        return result.ToResult();
    }
}
