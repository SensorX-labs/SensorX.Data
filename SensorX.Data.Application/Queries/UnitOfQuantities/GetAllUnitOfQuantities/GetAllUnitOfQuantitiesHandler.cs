using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetAllUnitOfQuantities;

public sealed class GetAllUnitOfQuantitiesHandler(
    IQueryBuilder<UnitOfQuantity> _unitQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetAllUnitOfQuantitiesQuery, Result<List<GetAllUnitOfQuantitiesResponse>>>
{
    public async Task<Result<List<GetAllUnitOfQuantitiesResponse>>> Handle(GetAllUnitOfQuantitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _unitQueryBuilder.QueryAsNoTracking
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllUnitOfQuantitiesResponse(
                x.Id.Value,
                x.Name,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var items = await _queryExecutor.ToListAsync(query, cancellationToken);
        return Result<List<GetAllUnitOfQuantitiesResponse>>.Success(items);
    }
}
