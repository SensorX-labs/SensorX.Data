using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Customers.UpdateCustomer;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class CustomerCommands
{
    public static RouteGroupBuilder MapCustomerCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("customer").WithTags("Customer Commands");

        api.MapPut("", UpdateCustomer).WithOpenApi();
        return api;
    }

    private static async Task<IResult> UpdateCustomer(
        [FromBody] UpdateCustomerCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}
