using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Common.QueryExtensions.Search;

public static class ProductSearch
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
            p.Name.Contains(term) ||
            p.Code.Value.Contains(term) ||
            p.Manufacture.Contains(term)
        );
    }
}