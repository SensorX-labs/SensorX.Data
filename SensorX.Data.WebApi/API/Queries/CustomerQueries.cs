using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Customers.CreateCustomer;
using SensorX.Data.Application.Commands.Customers.DeleteCustomer;
using SensorX.Data.Application.Commands.Customers.UpdateCustomer;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;
using SensorX.Data.Application.Queries.Customers.GetCustomerById;
using SensorX.Data.Application.Queries.Customers.GetDetailCustomerByAccountId;
using SensorX.Data.Application.Queries.Customers.GetPageListCustomers;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class CustomerQueries
{
    public static RouteGroupBuilder MapCustomerQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("customer").WithTags("Customer Queries");

        api.MapGet("/list", GetPageListCustomers).WithOpenApi();
        api.MapGet("/{customerId:guid}/buying-history", GetCustomerBuyingHistory).WithOpenApi();
        api.MapGet("/{customerId:guid}", GetCustomerById).WithOpenApi();
        api.MapGet("/account/{accountId:guid}", GetCustomerByAccountId).WithOpenApi();
        return api;
    }

    private static async Task<IResult> GetPageListCustomers(
        [AsParameters] GetPageListCustomersQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<OffsetPagedResult<GetPageListCustomersResponse>> result = await mediator.Send(query);
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

    private static async Task<IResult> GetCustomerById(
        [FromRoute] Guid customerId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetCustomerByIdQuery(customerId));
        return result.ToResult();
    }

    private static async Task<IResult> GetCustomerByAccountId(
        [FromRoute] Guid accountId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetDetailCustomerByAccountIdQuery(accountId));
        return result.ToResult();
    }
}