using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Common.QueryExtensions;

public static class ProductExtensions
{
    public static IQueryable<Product> ApplySearch(
        this IQueryable<Product> query,
        string? searchTerm
    )
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var term = searchTerm.Trim();

        return query.Where(p =>
            p.Name.StartsWith(term) ||
            p.Code.Value.StartsWith(term) ||
            p.Manufacture.StartsWith(term)
        );
    }
}