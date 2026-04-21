using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

public class GetProductPricingPolicyHandler(
    IQueryBuilder<Product> productQueryBuilder,
    IQueryBuilder<InternalPrice> internalPriceQueryBuilder,
    IQueryExecutor queryExecutor
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
        var query = from p in productQueryBuilder.QueryAsNoTracking
                    join ip in internalPriceQueryBuilder.QueryAsNoTracking
                    on p.Id equals ip.ProductId
                    where request.ProductIds.Contains(p.Id.Value)
                    select new GetProductPricingPolicyResponse(
                        p.Id.Value,
                        p.Code.Value,
                        p.Name,
                        p.Manufacture,
                        p.Unit,
                        p.Status,
                        ip.SuggestedPrice.Amount,
                        ip.FloorPrice.Amount,
                        ip.PriceTiers.Select(pt => new ProductPriceTier(
                            pt.Quantity.Value,
                            pt.Price.Amount
                        )).ToList(),
                        p.CreatedAt,
                        p.UpdatedAt
                    );

        var products = await queryExecutor.ToListAsync(query, cancellationToken);

        if (products.Count == 0)
            return Result<List<GetProductPricingPolicyResponse>>.Failure("Không tìm thấy sản phẩm nào với các ID được cung cấp");


        return Result<List<GetProductPricingPolicyResponse>>.Success(products);
    }
}
