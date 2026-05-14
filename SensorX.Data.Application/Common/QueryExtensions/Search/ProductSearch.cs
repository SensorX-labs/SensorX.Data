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

        var term = searchTerm.Trim().ToLower();

        return query.Where(p =>
            p.Name.ToLower().Contains(term) ||
            ((string)p.Code).ToLower().Contains(term) ||
            p.Manufacture.ToLower().Contains(term)
        );
    }
}