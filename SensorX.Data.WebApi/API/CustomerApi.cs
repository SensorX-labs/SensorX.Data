using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.WebApi.API
{
    public static class CustomerApi
    {
        public static RouteGroupBuilder MapCustomerApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/customer").WithTags("Customer");

            api.MapPost("/createCustomer", CreateCustomer).WithOpenApi();
            return api;
        }

        private static async Task<Results<Ok<Guid>, BadRequest<string>, ProblemHttpResult>> CreateCustomer(
            [FromBody] CreateCustomerCommand command,
            [FromServices] IMediator mediator
        )
        {
            Result<Guid> result = await mediator.Send(command);
            return result ? TypedResults.Ok(result.Value) : TypedResults.BadRequest(result.Error);
        }

        private static async Task<Results<Ok<Guid>, BadRequest<string>, ProblemHttpResult>> GetPageListCustomer(
            [FromBody] GetPageListCustomerQuery query,
            [FromServices] IMediator mediator
        )
        {
            Result<Guid> result = await mediator.Send(query);
            return result ? TypedResults.Ok(result.Value) : TypedResults.BadRequest(result.Error);
        }
    }
}
