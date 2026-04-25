using Ardalis.Specification;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate.Specifications;

public class ActiveInfiniteInternalPriceSpec : Specification<InternalPrice>
{
    public ActiveInfiniteInternalPriceSpec(ProductId productId)
    {
        Query.Where(p => p.ProductId == productId && p.ExpiresAt == DateTimeOffset.MaxValue);
    }
}
