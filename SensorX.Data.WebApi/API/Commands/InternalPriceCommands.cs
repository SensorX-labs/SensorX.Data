using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.DeactivateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.ExtendInternalPrice;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class InternalPriceCommands
{
    public static RouteGroupBuilder MapInternalPriceCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/internal-prices").WithTags("Internal Price Commands");

        api.MapPost("/create", CreateInternalPrice)
            .WithOpenApi()
            .WithSummary("Create new internal price");

        api.MapPatch("/{id:guid}/deactivate", DeactivateInternalPrice)
            .WithOpenApi()
            .WithSummary("Deactivate internal price");

        api.MapPatch("/{id:guid}/extend", ExtendInternalPrice)
            .WithOpenApi()
            .WithSummary("Extend internal price validity");

        return api;
    }

    private static async Task<IResult> CreateInternalPrice(
        [FromBody] CreateInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeactivateInternalPrice(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new DeactivateInternalPriceCommand(id));
        return result.ToResult();
    }

    private static async Task<IResult> ExtendInternalPrice(
        [FromRoute] Guid id,
        [FromBody] ExtendInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}
