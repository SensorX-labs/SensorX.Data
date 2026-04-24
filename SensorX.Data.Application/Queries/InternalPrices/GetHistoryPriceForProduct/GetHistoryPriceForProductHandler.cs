using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.InternalPrices.GetHistoryPriceForProduct;

public class GetHistoryPriceForProductHandler(
    IQueryBuilder<Product> _productQueryBuilder,
    IQueryBuilder<InternalPrice> _internalPriceQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetHistoryPriceForProductQuery, Result<GetHistoryPriceForProductResponse>>
{
    public async Task<Result<GetHistoryPriceForProductResponse>> Handle(
        GetHistoryPriceForProductQuery request,
        CancellationToken cancellationToken)
    {

        var dtoProductQuery = _productQueryBuilder.QueryAsNoTracking.Where(x => x.Id == request.ProductId);
        var product = await _queryExecutor.FirstOrDefaultAsync(dtoProductQuery, cancellationToken);
        if (product is null) return Result<GetHistoryPriceForProductResponse>.Failure("Sản phẩm không tồn tại");

        var query = _internalPriceQueryBuilder.QueryAsNoTracking
                        .Where(x => x.ProductId == request.ProductId)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.Id);

        var dtoInternalPriceListQuery = query.Select(x => new InternalPriceDto(
            x.Id.Value,
            x.SuggestedPrice.Amount,
            x.SuggestedPrice.Currency,
            x.FloorPrice.Amount,
            x.FloorPrice.Currency,
            !x.IsExpired(),
            x.CreatedAt,
            x.ExpiresAt,
            x.PriceTiers.Select(tier => new PriceTierDto(
                tier.Quantity.Value,
                tier.Price.Amount,
                tier.Price.Currency
            )).ToList()
        ));

        var internalPriceList = await _queryExecutor.ToListAsync(dtoInternalPriceListQuery, cancellationToken);

        var result = new GetHistoryPriceForProductResponse(
            product.Id.Value,
            product.Code.Value,
            product.Name,
            internalPriceList
        );

        return Result<GetHistoryPriceForProductResponse>.Success(result);
    }
}