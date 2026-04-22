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
            p.Code.Value.Contains(term) ||
            p.TaxCode.Contains(term) ||
            p.Email.Value.Contains(term) ||
            p.Phone.Value.Contains(term)
        );
    }
}