using Ardalis.Specification;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate.Specifications;

public class ActiveInternalPriceByProductIdSpec : Specification<InternalPrice>
{
    public ActiveInternalPriceByProductIdSpec(ProductId productId)
    {
        Query.Where(p => p.ProductId == productId && p.ExpiresAt > DateTimeOffset.UtcNow);
    }
}
