using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;

public class GetInternalPricesByProductIdHandler(
    IQueryBuilder<InternalPrice> _internalPriceBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetInternalPricesByProductIdQuery, Result<GetInternalPricesByProductIdResponse>>
{
    public async Task<Result<GetInternalPricesByProductIdResponse>> Handle(
        GetInternalPricesByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _internalPriceBuilder.QueryAsNoTracking
                .Where(x => x.ProductId == new ProductId(request.ProductId))
                .Select(x => new GetInternalPricesByProductIdResponse(
                    x.Id.Value,
                    x.ProductId.Value,
                    x.SuggestedPrice.Amount,
                    x.SuggestedPrice.Currency,
                    x.FloorPrice.Amount,
                    x.FloorPrice.Currency,
                    x.PriceTiers.Select(pt => new PriceTierResponse(
                        pt.Quantity.Value,
                        pt.Price.Amount,
                        pt.Price.Currency
                    )).ToList(),
                    x.CreatedAt
                ));

            var internalPrice = await _queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

            if (internalPrice is null)
                return Result<GetInternalPricesByProductIdResponse>.Failure("Không tìm thấy giá nội bộ cho sản phẩm này");

            return Result<GetInternalPricesByProductIdResponse>.Success(internalPrice);
        }
        catch (Exception ex)
        {
            return Result<GetInternalPricesByProductIdResponse>.Failure($"Lỗi khi lấy giá nội bộ: {ex.Message}");
        }
    }
}
