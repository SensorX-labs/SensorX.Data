using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

public class GetProductPricingPolicyHandler(
    IRepository<Product> productRepository,
    IRepository<InternalPrice> internalPriceRepository
) : IRequestHandler<GetProductPricingPolicyQuery, Result<List<GetProductPricingPolicyResponse>>>
{
    public async Task<Result<List<GetProductPricingPolicyResponse>>> Handle(
        GetProductPricingPolicyQuery request,
        CancellationToken cancellationToken)
    {
        // Validate input
        if (request.ProductIds == null || request.ProductIds.Count == 0)
            return Result<List<GetProductPricingPolicyResponse>>.Failure("Danh sách ProductId không được rỗng");

        // Lấy tất cả products theo danh sách ProductIds
        var allProducts = await productRepository.ListAsync(cancellationToken);
        var products = allProducts
            .Where(p => request.ProductIds.Contains(p.Id.Value))
            .ToList();

        if (products.Count == 0)
            return Result<List<GetProductPricingPolicyResponse>>.Failure("Không tìm thấy sản phẩm nào với các ID được cung cấp");

        // Lấy tất cả InternalPrices
        var allInternalPrices = await internalPriceRepository.ListAsync(cancellationToken);

        // Map các products thành responses
        var responses = products
            .Select(product =>
            {
                var internalPrice = allInternalPrices.FirstOrDefault(ip => ip.ProductId.Value == product.Id.Value);

                var floorPrice = internalPrice?.FloorPrice?.Amount ?? 0;
                var suggestedPrice = internalPrice?.SuggestedPrice?.Amount ?? 0;

                var priceTiers = internalPrice?.PriceTiers?
                    .Select(pt => new ProductPriceTier(
                        Quantity: pt.Quantity.Value,
                        Price: pt.Price.Amount
                    ))
                    .ToList() ?? new();

                return new GetProductPricingPolicyResponse(
                    ProductId: product.Id.Value,
                    ProductCode: product.Code.Value,
                    ProductName: product.Name,
                    Manufacture: product.Manufacture,
                    Unit: product.Unit,
                    ProductStatus: (int)product.Status,
                    FloorPrice: floorPrice,
                    SuggestedPrice: suggestedPrice,
                    PriceTiers: priceTiers,
                    CreatedAt: product.CreatedAt.DateTime,
                    UpdatedAt: product.UpdatedAt?.DateTime
                );
            })
            .ToList();

        return Result<List<GetProductPricingPolicyResponse>>.Success(responses);
    }
}
