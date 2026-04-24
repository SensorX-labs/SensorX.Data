using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.DeactivateInternalPrice;
using SensorX.Data.Application.Commands.InternalPrices.ExtendInternalPrice;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.InternalPrices.GetHistoryPriceForProduct;
using SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;
using SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

public static class InternalPriceApi
{
    public static RouteGroupBuilder MapInternalPriceApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Internal Prices");
        api.MapPost("/internalPrices/create", CreateInternalPrice)
            .WithOpenApi()
            .WithSummary("Create new internal price")
            .WithDescription("Creates a new internal price policy for a product, including suggested price, floor price, and tiered volume discounts.");

        api.MapGet("/internalPrices/product/{productId:guid}", GetInternalPricesByProductId)
            .WithOpenApi()
            .WithSummary("Get all internal prices for a product")
            .WithDescription("Retrieves all internal price policies associated with a specific product ID.");

        api.MapPatch("/internalPrices/{id:guid}/deactivate", DeactivateInternalPrice)
            .WithOpenApi()
            .WithSummary("Deactivate internal price")
            .WithDescription("Immediately expires an active internal price policy by setting its expiration date to current time.");

        api.MapPatch("/internalPrices/{id:guid}/extend", ExtendInternalPrice)
            .WithOpenApi()
            .WithSummary("Extend internal price validity")
            .WithDescription("Extends the expiration date of an internal price policy by providing a new end date or a duration.");

        api.MapGet("/internalPrices/list", GetPageListInternalPrice)
            .WithOpenApi()
            .WithSummary("Get paged list of internal prices")
            .WithDescription("Retrieves a paged list of all internal price policies with search functionality.");

        api.MapGet("/internalPrices/product/{productId:guid}/history", GetHistoryPriceForProduct)
            .WithOpenApi()
            .WithSummary("Get price history for a product")
            .WithDescription("Retrieves a paged history of all price policies (active and expired) for a specific product.");

        return api;
    }

    public static async Task<IResult> CreateInternalPrice(
        [FromBody] CreateInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetInternalPricesByProductId(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        Result<GetInternalPricesByProductIdResponse> result = await mediator.Send(new GetInternalPricesByProductIdQuery(productId));
        return result.ToResult();
    }

    private static async Task<IResult> DeactivateInternalPrice(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        Result result = await mediator.Send(new DeactivateInternalPriceCommand(id));
        return result.ToResult();
    }

    private static async Task<IResult> ExtendInternalPrice(
        [FromRoute] Guid id,
        [FromBody] ExtendInternalPriceCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        Result result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> GetPageListInternalPrice(
        [AsParameters] GetPageListInternalPriceQuery query,
        [FromServices] IMediator mediator
    )
    {
        Result<InternalPriceOffsetPagedResult> result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetHistoryPriceForProduct(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator
    )
    {
        Result<GetHistoryPriceForProductResponse> result = await mediator.Send(new GetHistoryPriceForProductQuery(productId));
        return result.ToResult();
    }
}