using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceById;

public sealed class GetInternalPriceByIdHandler(
    IQueryBuilder<InternalPrice> _priceQueryBuilder,
    IQueryBuilder<Product> _productQueryBuilder,
    IQueryBuilder<UnitOfQuantity> _unitOfQuantityQueryBuilder,
    IQueryExecutor _queryExecutor
    ) : IRequestHandler<GetInternalPriceByIdQuery, Result<GetInternalPriceByIdResponse>>
{
    public async Task<Result<GetInternalPriceByIdResponse>> Handle(GetInternalPriceByIdQuery request, CancellationToken cancellationToken)
    {
        var priceQuery = _priceQueryBuilder.QueryAsNoTracking
            .Where(x => x.Id == new InternalPriceId(request.Id));

        var productQuery = _productQueryBuilder.QueryAsNoTracking;

        var resultQuery = from price in priceQuery
                          join product in productQuery on price.ProductId equals product.Id
                          join unit in _unitOfQuantityQueryBuilder.QueryAsNoTracking on product.UnitOfQuantityId equals unit.Id
                          select new GetInternalPriceByIdResponse(
                              price.Id.Value,
                              product.Id.Value,
                              product.Name,
                              product.Code.Value,
                              unit.Name,
                              price.SuggestedPrice.Amount,
                              price.SuggestedPrice.Currency,
                              price.FloorPrice.Amount,
                              price.FloorPrice.Currency,
                              price.PriceTiers.Select(t => new PriceTierDetailResponse(
                                  t.Quantity.Value,
                                  t.Price.Amount,
                                  t.Price.Currency
                              )).ToList(),
                              price.CreatedAt,
                              price.ExpiresAt,
                              price.IsExpired() ? InternalPriceStatus.Expired : price.IsExpiringSoon(7) ? InternalPriceStatus.ExpiringSoon : InternalPriceStatus.Active
                          );

        var response = await _queryExecutor.FirstOrDefaultAsync(resultQuery, cancellationToken);

        if (response == null)
        {
            return Result<GetInternalPriceByIdResponse>.Failure("Không tìm thấy chính sách giá này.");
        }

        return Result<GetInternalPriceByIdResponse>.Success(response);
    }
}
