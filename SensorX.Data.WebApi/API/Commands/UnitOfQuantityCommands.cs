using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.UnitOfQuantities.CreateUnitOfQuantity;
using SensorX.Data.Application.Commands.UnitOfQuantities.DeleteUnitOfQuantity;
using SensorX.Data.Application.Commands.UnitOfQuantities.UpdateUnitOfQuantity;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class UnitOfQuantityCommands
{
    public static RouteGroupBuilder MapUnitOfQuantityCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/unit-of-quantities").WithTags("Unit Of Quantity Commands");

        api.MapPost("/create", CreateUnitOfQuantity)
            .WithOpenApi()
            .WithSummary("Create unit of quantity");

        api.MapPut("/{id:guid}", UpdateUnitOfQuantity)
            .WithOpenApi()
            .WithSummary("Update unit of quantity");

        api.MapDelete("/{id:guid}", DeleteUnitOfQuantity)
            .WithOpenApi()
            .WithSummary("Delete unit of quantity");

        return api;
    }

    private static async Task<IResult> CreateUnitOfQuantity(
        [FromBody] CreateUnitOfQuantityCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> UpdateUnitOfQuantity(
        [FromRoute] Guid id,
        [FromBody] UpdateUnitOfQuantityCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteUnitOfQuantity(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new DeleteUnitOfQuantityCommand(id));
        return result.ToResult();
    }
}
