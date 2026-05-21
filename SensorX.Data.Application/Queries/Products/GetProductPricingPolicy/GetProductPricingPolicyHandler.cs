using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

public class GetProductPricingPolicyHandler(
    IQueryBuilder<Product> productQueryBuilder,
    IQueryBuilder<Supplier> supplierQueryBuilder,
    IQueryBuilder<UnitOfQuantity> unitOfQuantityQueryBuilder,
    IQueryBuilder<InternalPrice> internalPriceQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetProductPricingPolicyQuery, Result<List<GetProductPricingPolicyResponse>>>
{
    public async Task<Result<List<GetProductPricingPolicyResponse>>> Handle(
        GetProductPricingPolicyQuery request,
        CancellationToken cancellationToken)
    {
        if (request.ProductIds == null || request.ProductIds.Count == 0)
            return Result<List<GetProductPricingPolicyResponse>>.Failure("Danh sách ProductId không được rỗng");

        var productIds = request.ProductIds.Select(id => new ProductId(id)).ToList();

        var query = from p in productQueryBuilder.QueryAsNoTracking
                    join s in supplierQueryBuilder.QueryAsNoTracking on p.SupplierId equals s.Id
                    join u in unitOfQuantityQueryBuilder.QueryAsNoTracking on p.UnitOfQuantityId equals u.Id
                    join ip in internalPriceQueryBuilder.QueryAsNoTracking on p.Id equals ip.ProductId
                    where productIds.Contains(p.Id)
                    select new GetProductPricingPolicyResponse(
                        p.Id.Value,
                        p.Code.Value,
                        p.Name,
                        s.Name,
                        u.Name,
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
