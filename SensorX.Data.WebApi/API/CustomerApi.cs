using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;
using SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

namespace SensorX.Data.WebApi.API
{
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

        private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateCustomer(
            [FromBody] CreateCustomerCommand command,
            [FromServices] IMediator mediator
        )
        {
            Result<Guid> result = await mediator.Send(command);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Message);
        }

        private static async
            Task<Results<Ok<Result<CustomerCursorPagedResult>>, BadRequest<string>>>
            GetPageListCustomers(
                [FromServices] IMediator mediator,
                [AsParameters] GetPageListCustomersQuery query
        )
        {
            var result = await mediator.Send(query);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result.Message ?? "Lỗi khi lấy danh sách khách hàng");
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
    }
}
