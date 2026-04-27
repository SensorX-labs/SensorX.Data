namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceSuggest;

using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public class GetInternalPriceSuggestHandler(
    IQueryBuilder<InternalPrice> _internalPriceQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetInternalPriceSuggestQuery, Result<List<InternalPriceResponse>>>
{
    public async Task<Result<List<InternalPriceResponse>>> Handle(
        GetInternalPriceSuggestQuery request,
        CancellationToken cancellationToken)
    {
        var query = _internalPriceQueryBuilder.QueryAsNoTracking
                        .Where(x => request.ProductIds.Contains((Guid)x.ProductId))
                        .IsActive()
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenBy(x => x.ExpiresAt)
                        .ThenByDescending(x => x.Id);

        var dtoInternalPriceListQuery = query.Select(x => new InternalPriceResponse(
            x.Id.Value,
            x.ProductId.Value,
            x.SuggestedPrice.Amount,
            x.SuggestedPrice.Currency,
            x.FloorPrice.Amount,
            x.FloorPrice.Currency,
            x.PriceTiers.Select(tier => new PriceTierResponse(
                tier.Quantity.Value,
                tier.Price.Amount,
                tier.Price.Currency
            )).ToList(),
            x.CreatedAt
        ));

        var allActivePrices = await _queryExecutor.ToListAsync(dtoInternalPriceListQuery, cancellationToken);

        var internalPriceList = allActivePrices
            .GroupBy(x => x.ProductId)
            .Select(g => g.First())
            .ToList();

        return Result<List<InternalPriceResponse>>.Success(internalPriceList);
    }
}