using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.UnitOfQuantities.GetAllUnitOfQuantities;
using SensorX.Data.Application.Queries.UnitOfQuantities.GetUnitOfQuantityById;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class UnitOfQuantityQueries
{
    public static RouteGroupBuilder MapUnitOfQuantityQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/unit-of-quantities").WithTags("Unit Of Quantity Queries");

        api.MapGet("/list-all", GetAllUnitOfQuantities)
            .WithOpenApi()
            .WithSummary("Get all unit of quantities");

        api.MapGet("/{id:guid}", GetUnitOfQuantityById)
            .WithOpenApi()
            .WithSummary("Get unit of quantity by id");

        return api;
    }

    private static async Task<IResult> GetAllUnitOfQuantities(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetAllUnitOfQuantitiesResponse>> result = await mediator.Send(new GetAllUnitOfQuantitiesQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetUnitOfQuantityById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetUnitOfQuantityByIdQuery(id));
        return result.ToResult();
    }
}
