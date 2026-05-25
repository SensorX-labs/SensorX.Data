using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;

public sealed class GetPageListInternalPriceHandler(
    IQueryBuilder<InternalPrice> _internalPriceQueryBuilder,
    IQueryBuilder<Product> _productQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListInternalPriceQuery, Result<OffsetPagedResult<GetPageListInternalPriceResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListInternalPriceResponse>>> Handle(GetPageListInternalPriceQuery request, CancellationToken cancellationToken)
    {
        var query = from internalPrice in _internalPriceQueryBuilder.QueryAsNoTracking
                    join product in _productQueryBuilder.QueryAsNoTracking
                        on internalPrice.ProductId equals product.Id
                    select new { product, internalPrice };

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(i =>
                i.product.Name.Contains(term)
                || ((string)i.product.Code).Contains(term)
            );
        }
        if (!string.IsNullOrWhiteSpace(request.ProductCode))
        {
            var codeTerm = request.ProductCode.Trim();
            query = query.Where(i => ((string)i.product.Code).Contains(codeTerm));
        }
        if (!string.IsNullOrWhiteSpace(request.ProductName))
        {
            var nameTerm = request.ProductName.Trim();
            query = query.Where(i => i.product.Name.Contains(nameTerm));
        }
        if (request.Status.HasValue)
        {
            var now = DateTimeOffset.UtcNow;
            query = request.Status.Value switch
            {
                InternalPriceStatus.Expired =>
                    query.Where(i => i.internalPrice.ExpiresAt <= now),

                InternalPriceStatus.ExpiringSoon =>
                    query.Where(i => i.internalPrice.ExpiresAt <= now.AddDays(7) && i.internalPrice.ExpiresAt > now),

                InternalPriceStatus.Active =>
                    query.Where(i => i.internalPrice.ExpiresAt > now),

                _ => query
            };
        }
        if (request.ExpiresFrom.HasValue)
        {
            var expiresFrom = new DateTimeOffset(
                request.ExpiresFrom.Value.ToDateTime(TimeOnly.MinValue),
                TimeSpan.Zero);
            query = query.Where(i => i.internalPrice.ExpiresAt >= expiresFrom);
        }
        if (request.ExpiresTo.HasValue)
        {
            var expiresToExclusive = new DateTimeOffset(
                request.ExpiresTo.Value.ToDateTime(TimeOnly.MinValue),
                TimeSpan.Zero).AddDays(1);
            query = query.Where(i => i.internalPrice.ExpiresAt < expiresToExclusive);
        }
        if (request.SuggestedPriceFrom.HasValue)
        {
            query = query.Where(i => i.internalPrice.SuggestedPrice.Amount >= request.SuggestedPriceFrom.Value);
        }
        if (request.SuggestedPriceTo.HasValue)
        {
            query = query.Where(i => i.internalPrice.SuggestedPrice.Amount <= request.SuggestedPriceTo.Value);
        }
        if (request.FloorPriceFrom.HasValue)
        {
            query = query.Where(i => i.internalPrice.FloorPrice.Amount >= request.FloorPriceFrom.Value);
        }
        if (request.FloorPriceTo.HasValue)
        {
            query = query.Where(i => i.internalPrice.FloorPrice.Amount <= request.FloorPriceTo.Value);
        }
        var totalCount = await _queryExecutor.CountAsync(query, cancellationToken);

        var pagedQuery = query
            .OrderByDescending(x => x.internalPrice.CreatedAt)
            .ThenByDescending(x => x.internalPrice.Id)
            .ApplyOffsetPagination(request);

        var dtoQuery = pagedQuery.Select(x => new GetPageListInternalPriceResponse(
            x.internalPrice.Id.Value,
            x.product.Id.Value,
            x.product.Code.Value,
            x.product.Name,
            x.internalPrice.SuggestedPrice.Amount,
            x.internalPrice.SuggestedPrice.Currency,
            x.internalPrice.FloorPrice.Amount,
            x.internalPrice.FloorPrice.Currency,
            x.internalPrice.IsExpired() ? InternalPriceStatus.Expired : x.internalPrice.IsExpiringSoon(7) ? InternalPriceStatus.ExpiringSoon : InternalPriceStatus.Active,
            x.internalPrice.CreatedAt,
            x.internalPrice.ExpiresAt,
            x.internalPrice.PriceTiers.Select(x => new PriceTierDto(
                x.Quantity.Value,
                x.Price.Amount,
                x.Price.Currency
            )).ToList()
        ));

        var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

        var result = new OffsetPagedResult<GetPageListInternalPriceResponse>
        {
            Items = items,
            PageNumber = request.PageNumber ?? 1,
            PageSize = request.PageSize ?? 10,
            TotalCount = totalCount
        };

        return Result<OffsetPagedResult<GetPageListInternalPriceResponse>>.Success(result);

    }
}
