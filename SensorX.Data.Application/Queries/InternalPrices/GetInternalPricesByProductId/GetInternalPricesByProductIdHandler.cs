using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;

public class GetInternalPricesByProductIdHandler(
    IRepository<InternalPrice> _internalPriceRepository,
    IRepository<Product> _productRepository
) : IRequestHandler<GetInternalPricesByProductIdQuery, Result<GetInternalPricesByProductIdResponse>>
{
    public async Task<Result<GetInternalPricesByProductIdResponse>> Handle(
        GetInternalPricesByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Kiểm tra sản phẩm có tồn tại không
            var productId = new ProductId(request.ProductId);
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
                return Result<GetInternalPricesByProductIdResponse>.Failure("Không tìm thấy sản phẩm");

            // Lấy giá nội bộ của sản phẩm
            var allInternalPrices = await _internalPriceRepository.ListAsync(cancellationToken);
            var internalPrice = allInternalPrices.FirstOrDefault(ip => ip.ProductId.Value == request.ProductId);
            
            if (internalPrice is null)
                return Result<GetInternalPricesByProductIdResponse>.Failure("Không tìm thấy giá nội bộ cho sản phẩm này");

            // Chuyển đổi thành DTO
            var response = new GetInternalPricesByProductIdResponse
            {
                Id = internalPrice.Id.Value,
                ProductId = internalPrice.ProductId.Value,
                SuggestedPriceAmount = internalPrice.SuggestedPrice.Amount,
                SuggestedPriceCurrency = internalPrice.SuggestedPrice.Currency,
                FloorPriceAmount = internalPrice.FloorPrice.Amount,
                FloorPriceCurrency = internalPrice.FloorPrice.Currency,
                CreatedAt = internalPrice.CreatedAt,
                PriceTiers = internalPrice.PriceTiers.Select(pt => new PriceTierResponse
                {
                    Quantity = pt.Quantity.Value,
                    PriceAmount = pt.Price.Amount,
                    PriceCurrency = pt.Price.Currency
                }).ToList()
            };

            return Result<GetInternalPricesByProductIdResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetInternalPricesByProductIdResponse>.Failure($"Lỗi khi lấy giá nội bộ: {ex.Message}");
        }
    }
}
