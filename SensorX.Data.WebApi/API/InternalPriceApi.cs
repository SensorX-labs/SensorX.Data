using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;
using SensorX.Data.WebApi.Extensions;

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

    public static async Task<IResult> CreateInternalPrice(
        [FromBody] CreateInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetInternalPricesByProductId(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetInternalPricesByProductIdQuery(productId));
        return result.ToResult();
    }
}