using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;

namespace SensorX.Data.Application.Common.QueryExtensions.Search;

public static class CustomerSearch
{
    public static IQueryable<Customer> ApplySearch(
        this IQueryable<Customer> query,
        string? searchTerm
    )
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var term = searchTerm.Trim();

        return query.Where(p =>
            p.Name.Contains(term) ||
            ((string)p.Code).Contains(term) ||
            p.TaxCode != null && p.TaxCode.Contains(term) ||
            ((string)p.Email).Contains(term) ||
            (p.Phone != null && ((string)p.Phone).Contains(term))
        );
    }
}