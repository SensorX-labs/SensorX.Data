using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Staffs.ChangeStaffStatus;
using SensorX.Data.Application.Commands.Staffs.UpdateProfile;
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

        api.MapPut("profile", UpdateProfile).WithOpenApi();
        api.MapPut("update-avatar", UpdateStaffAvatar).WithOpenApi();
        api.MapPut("{id:guid}/status", ChangeStaffStatus).WithOpenApi();

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
    private static async Task<IResult> UpdateProfile(
        [FromBody] UpdateProfileCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> ChangeStaffStatus(
        [FromRoute] Guid id,
        [FromBody] ChangeStaffStatusCommand command,
        [FromServices] IMediator mediator
    )
    {
        command.Id = id;
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}
