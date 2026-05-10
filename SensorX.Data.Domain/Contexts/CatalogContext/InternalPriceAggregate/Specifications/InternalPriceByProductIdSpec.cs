using Ardalis.Specification;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate.Specifications;

public class InternalPriceByProductIdSpec : Specification<InternalPrice>
{
    public InternalPriceByProductIdSpec(ProductId productId)
    {
        Query.Where(p => p.ProductId == productId);
    }
}
