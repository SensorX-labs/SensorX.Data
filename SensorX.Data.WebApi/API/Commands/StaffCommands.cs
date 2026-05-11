using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.UpdateStaff;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class StaffCommands
{
    public static RouteGroupBuilder MapStaffCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("staff").WithTags("Staff Commands");

        api.MapPut("", UpdateStaff).WithOpenApi();

        return api;
    }

    private static async Task<IResult> UpdateStaff(
        [FromBody] UpdateStaffCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}
