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

        private static async Task<Results<Ok<Result<GetCustomerBuyingHistoryResponse>>, BadRequest<string>>> GetCustomerBuyingHistory(
            [FromRoute] Guid customerId,
            [FromServices] IMediator mediator
        )
        {
            var query = new GetCustomerBuyingHistoryQuery(customerId);
            var result = await mediator.Send(query);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result.Message ?? "Lỗi khi lấy lịch sử mua hàng");
        }

        private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> UpdateCustomer(
            [FromBody] SensorX.Data.Application.Commands.Customers.UpdateCustomer.UpdateCustomerCommand command,
            [FromServices] IMediator mediator
        )
        {
            Result<Guid> result = await mediator.Send(command);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Message);
        }

        private static async Task<Results<Ok<Result<bool>>, BadRequest<string>>> DeleteCustomer(
            [FromRoute] Guid customerId,
            [FromServices] IMediator mediator
        )
        {
            var command = new SensorX.Data.Application.Commands.Customers.DeleteCustomer.DeleteCustomerCommand(customerId);
            Result<bool> result = await mediator.Send(command);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Message);
        }
    }
}