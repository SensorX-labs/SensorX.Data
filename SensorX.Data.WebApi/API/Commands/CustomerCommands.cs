using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Customers.CreateCustomer;
using SensorX.Data.Application.Commands.Customers.UpdateCustomer;
using SensorX.Data.Application.Commands.Customers.DeleteCustomer;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class CustomerCommands
{
    public static RouteGroupBuilder MapCustomerCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("customer").WithTags("Customer Commands");

        api.MapPost("/create", CreateCustomer).WithOpenApi();
        api.MapPut("", UpdateCustomer).WithOpenApi();
        api.MapDelete("/{customerId:guid}", DeleteCustomer).WithOpenApi();

        return api;
    }

    private static async Task<IResult> CreateCustomer(
        [FromBody] CreateCustomerCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> UpdateCustomer(
        [FromBody] UpdateCustomerCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteCustomer(
        [FromRoute] Guid customerId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new DeleteCustomerCommand(customerId));
        return result.ToResult();
    }
}
