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
) : IRequestHandler<GetPageListInternalPriceQuery, Result<InternalPriceOffsetPagedResult>>
{
    public async Task<Result<InternalPriceOffsetPagedResult>> Handle(GetPageListInternalPriceQuery request, CancellationToken cancellationToken)
    {
        var query = from internalPrice in _internalPriceQueryBuilder.QueryAsNoTracking
                    join product in _productQueryBuilder.QueryAsNoTracking
                        on internalPrice.ProductId equals product.Id
                    select new { product, internalPrice };

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i => i.product.Name.Contains(request.SearchTerm));
            query = query.Where(i => i.product.Code.Value.Contains(request.SearchTerm));
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
            !x.internalPrice.IsExpired(),
            x.internalPrice.CreatedAt,
            x.internalPrice.ExpiresAt,
            x.internalPrice.PriceTiers.Select(x => new PriceTierDto(
                x.Quantity.Value,
                x.Price.Amount,
                x.Price.Currency
            )).ToList()
        ));

        var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

        var result = new InternalPriceOffsetPagedResult
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<InternalPriceOffsetPagedResult>.Success(result);

    }
}