using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Customers.CreateCustomer;
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
}