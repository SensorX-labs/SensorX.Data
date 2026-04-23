using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;


namespace SensorX.Data.WebApi.API;

public static class InternalPriceApi
{
    public static RouteGroupBuilder MapInternalPriceApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Internal Prices");
        api.MapPost("/internalPrices", CreateInternalPrice).WithOpenApi();
        api.MapGet("/internalPrices/product/{productId:guid}", GetInternalPricesByProductId).WithOpenApi();

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
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result<GetInternalPricesByProductIdResponse>>, BadRequest<string>>> GetInternalPricesByProductId(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        var query = new GetInternalPricesByProductIdQuery(productId);
        Result<GetInternalPricesByProductIdResponse> result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
    }
}