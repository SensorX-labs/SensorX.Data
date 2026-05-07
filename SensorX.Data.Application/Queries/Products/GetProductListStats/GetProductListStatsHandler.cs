using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetProductListStats;

public sealed class GetProductListStatsHandler(
    IQueryBuilder<Product> _productBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetProductListStatsQuery, Result<ProductListStatsResponse>>
{

    public async Task<Result<ProductListStatsResponse>> Handle(GetProductListStatsQuery request, CancellationToken cancellationToken)
    {
        var query = _productBuilder.QueryAsNoTracking;

        var totalCount = await _queryExecutor.CountAsync(query, cancellationToken);
        var activeCount = await _queryExecutor.CountAsync(query.Where(p => p.Status == ProductStatus.Active), cancellationToken);
        var inactiveCount = await _queryExecutor.CountAsync(query.Where(p => p.Status == ProductStatus.Inactive), cancellationToken);

        var result = new ProductListStatsResponse(
            totalCount,
            activeCount,
            inactiveCount
        );

        return Result<ProductListStatsResponse>.Success(result);
    }
}