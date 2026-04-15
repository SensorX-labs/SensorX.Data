using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Common.ResponseClient;


namespace SensorX.Data.WebApi.API;

public static class InternalPriceApi
{
    public static RouteGroupBuilder MapInternalPriceApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog").WithTags("Internal Prices");
        api.MapPost("/internalPrices", CreateInternalPrice).WithOpenApi();

        return api;
    }

    public static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateInternalPrice(
        [FromBody] CreateInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }
}