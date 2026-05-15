using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.UpdateStaff;
using SensorX.Data.Application.Commands.Staffs.UpdateStaffAvatar;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.WebApi.Configurations;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class StaffCommands
{
    public static RouteGroupBuilder MapStaffCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("staff").WithTags("Staff Commands");

        api.MapPut("", UpdateStaff).WithOpenApi();
        api.MapPut("update-avatar", UpdateStaffAvatar).WithOpenApi();

        return api;
    }

    [AuthorizeRole(Role.WarehouseStaff, Role.SaleStaff, Role.Manager)]
    private static async Task<IResult> UpdateStaffAvatar(
        [FromBody] UpdateStaffAvatarCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    [AuthorizeRole(Role.WarehouseStaff, Role.SaleStaff, Role.Manager)]
    private static async Task<IResult> UpdateStaff(
        [FromBody] UpdateStaffCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}
