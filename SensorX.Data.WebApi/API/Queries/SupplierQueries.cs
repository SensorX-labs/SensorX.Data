using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Suppliers.GetAllSuppliers;
using SensorX.Data.Application.Queries.Suppliers.GetSupplierById;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Queries;

public static class SupplierQueries
{
    public static RouteGroupBuilder MapSupplierQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/suppliers").WithTags("Supplier Queries");

        api.MapGet("/list-all", GetAllSuppliers)
            .WithOpenApi()
            .WithSummary("Get all suppliers");

        api.MapGet("/{id:guid}", GetSupplierById)
            .WithOpenApi()
            .WithSummary("Get supplier by id");

        return api;
    }

    private static async Task<IResult> GetAllSuppliers(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetAllSuppliersResponse>> result = await mediator.Send(new GetAllSuppliersQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetSupplierById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetSupplierByIdQuery(id));
        return result.ToResult();
    }
}
