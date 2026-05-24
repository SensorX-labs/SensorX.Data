using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Suppliers.CreateSupplier;
using SensorX.Data.Application.Commands.Suppliers.DeleteSupplier;
using SensorX.Data.Application.Commands.Suppliers.UpdateSupplier;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.WebApi.Configurations;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class SupplierCommands
{
    public static RouteGroupBuilder MapSupplierCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/suppliers").WithTags("Supplier Commands");

        api.MapPost("/create", CreateSupplier)
            .WithOpenApi()
            .WithSummary("Create supplier");

        api.MapPut("/{id:guid}", UpdateSupplier)
            .WithOpenApi()
            .WithSummary("Update supplier");

        api.MapDelete("/{id:guid}", DeleteSupplier)
            .WithOpenApi()
            .WithSummary("Delete supplier");

        return api;
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> CreateSupplier(
        [FromBody] CreateSupplierCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> UpdateSupplier(
        [FromRoute] Guid id,
        [FromBody] UpdateSupplierCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    [AuthorizeRole(Role.Manager)]
    private static async Task<IResult> DeleteSupplier(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new DeleteSupplierCommand(id));
        return result.ToResult();
    }
}
