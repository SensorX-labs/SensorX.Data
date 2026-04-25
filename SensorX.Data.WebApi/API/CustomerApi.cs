using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Customers.CreateCustomer;
using SensorX.Data.Application.Commands.Customers.UpdateCustomer;
using SensorX.Data.Application.Commands.Customers.DeleteCustomer;
using SensorX.Data.Application.Queries.Customers.GetCustomerById;
using SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;
using SensorX.Data.Application.Queries.Customers.GetPageListCustomers;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

public static class CustomerApi
{
    public static RouteGroupBuilder MapCustomerApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("customer").WithTags("Customer");

            api.MapPost("/create", CreateCustomer).WithOpenApi();
            api.MapGet("/list", GetPageListCustomers).WithOpenApi();
            api.MapGet("/{customerId:guid}/buying-history", GetCustomerBuyingHistory).WithOpenApi();
            api.MapGet("/{customerId:guid}", GetCustomerById).WithOpenApi();
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

    private static async Task<IResult> GetPageListCustomers(
        [AsParameters] GetPageListCustomersQuery query,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetCustomerBuyingHistory(
        [FromRoute] Guid customerId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetCustomerBuyingHistoryQuery(customerId));
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

    private static async Task<IResult> GetCustomerById(
        [FromRoute] Guid customerId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetCustomerByIdQuery(customerId));
        return result.ToResult();
    }
}