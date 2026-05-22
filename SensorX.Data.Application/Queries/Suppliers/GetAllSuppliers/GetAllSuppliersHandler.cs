using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;

namespace SensorX.Data.Application.Queries.Suppliers.GetAllSuppliers;

public sealed class GetAllSuppliersHandler(
    IQueryBuilder<Supplier> _supplierQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetAllSuppliersQuery, Result<List<GetAllSuppliersResponse>>>
{
    public async Task<Result<List<GetAllSuppliersResponse>>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        var query = _supplierQueryBuilder.QueryAsNoTracking
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllSuppliersResponse(
                x.Id.Value,
                x.Name,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var items = await _queryExecutor.ToListAsync(query, cancellationToken);
        return Result<List<GetAllSuppliersResponse>>.Success(items);
    }
}
