using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetWarehouseProductContext;

public sealed class GetWarehouseProductContextHandler(
    IQueryBuilder<Product> _productBuilder,
    IQueryBuilder<Category> _categoryBuilder,
    IQueryBuilder<InternalPrice> _internalPriceBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetWarehouseProductContextQuery, Result<List<WarehouseProductContextDto>>>
{
    public async Task<Result<List<WarehouseProductContextDto>>> Handle(GetWarehouseProductContextQuery request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var activePrices = _internalPriceBuilder.QueryAsNoTracking
            .Where(p => p.ExpiresAt > now);

        var query = from product in _productBuilder.QueryAsNoTracking
                    join c in _categoryBuilder.QueryAsNoTracking on product.CategoryId equals c.Id into categoryGroup
                    from category in categoryGroup.DefaultIfEmpty()
                    join p in activePrices on product.Id equals p.ProductId into priceGroup
                    from price in priceGroup.DefaultIfEmpty()
                    select new WarehouseProductContextDto(
                        product.Id.Value,
                        category != null ? category.Name : "Khác",
                        price != null ? price.SuggestedPrice : 0
                    );

        var result = await _queryExecutor.ToListAsync(query, cancellationToken);

        return Result<List<WarehouseProductContextDto>>.Success(result);
    }
}
