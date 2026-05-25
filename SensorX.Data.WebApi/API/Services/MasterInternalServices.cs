using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;
using SensorX.Data.Application.Queries.Customers.GetDetailCustomerByAccountId;
using SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;
using SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Services;

public static class MasterInternalServices
{
    public static RouteGroupBuilder MapMasterInternalServices(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("internal-api").WithTags("Master Internal Services");

        api.MapPost("/catalog/products/batch", GetProductPricingPolicy).WithOpenApi();
        api.MapGet("/customers/{customerId:guid}/buying-history", GetCustomerBuyingHistory).WithOpenApi();
        api.MapGet("/customers/account/{accountId:guid}", GetCustomerByAccountId).WithOpenApi();
        api.MapGet("/staff/{staffId:guid}/metrics", GetEmployeeMetrics).WithOpenApi();

        return api;
    }

    private static async Task<IResult> GetProductPricingPolicy(
        [FromBody] GetProductPricingPolicyQuery query,
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

    private static async Task<IResult> GetCustomerByAccountId(
        [FromRoute] Guid accountId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetDetailCustomerByAccountIdQuery(accountId));
        return result.ToResult();
    }

    private static async Task<IResult> GetEmployeeMetrics(
        [FromRoute] Guid staffId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetStaffMetricsQuery(staffId));
        return result.ToResult();
    }
}
