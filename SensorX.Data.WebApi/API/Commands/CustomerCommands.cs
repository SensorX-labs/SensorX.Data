using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Customers.UpdateCustomerInfo;
using SensorX.Data.Application.Commands.Customers.UpdateShippingInfo;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class CustomerCommands
{
    public static RouteGroupBuilder MapCustomerCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("customer").WithTags("Customer Commands");

        api.MapPut("update-info", UpdateCustomerInfo).WithOpenApi();
        api.MapPut("update-shipping-info", UpdateShippingInfo).WithOpenApi();
        return api;
    }

    private static async Task<IResult> UpdateCustomerInfo(
        [FromBody] UpdateCustomerInfoCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> UpdateShippingInfo(
        [FromBody] UpdateShippingInfoCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}
