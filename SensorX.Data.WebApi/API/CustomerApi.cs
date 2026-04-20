using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Customers.GetPageListCustomers;
using SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;

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
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Error);
        }

        private static async
            Task<Results<Ok<Result<PaginatedResult<GetPageListCustomersResponse>>>, BadRequest<string>>>
            GetPageListCustomers(
                [FromServices] IMediator mediator,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] string? searchTerm = null,
                [FromQuery] Guid? customerId = null
            )
        {
            var query = new GetPageListCustomersQuery(pageNumber, pageSize, searchTerm, customerId);
            var result = await mediator.Send(query);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result.Error ?? "Lỗi khi lấy danh sách khách hàng");
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
                : TypedResults.BadRequest(result.Error ?? "Lỗi khi lấy lịch sử mua hàng");
        }
    }
}
