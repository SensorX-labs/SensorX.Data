using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceListStats;

public sealed class GetInternalPriceListStatsHandler(
    IQueryBuilder<InternalPrice> _internalPriceQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetInternalPriceListStatsQuery, Result<GetInternalPriceListStatsResponse>>
{
    public async Task<Result<GetInternalPriceListStatsResponse>> Handle(GetInternalPriceListStatsQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _internalPriceQueryBuilder.QueryAsNoTracking;

        var totalCount = await _queryExecutor.CountAsync(baseQuery, cancellationToken);
        var activeCount = await _queryExecutor.CountAsync(baseQuery.IsActive(), cancellationToken);
        var expiringSoonCount = await _queryExecutor.CountAsync(baseQuery.IsExpiringSoon(7), cancellationToken);
        var expiredCount = totalCount - activeCount;

        var response = new GetInternalPriceListStatsResponse(
            totalCount,
            activeCount,
            expiringSoonCount,
            expiredCount
        );

        return Result<GetInternalPriceListStatsResponse>.Success(response);
    }

}
