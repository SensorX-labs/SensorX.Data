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
            p.Name.StartsWith(term) ||
            p.Code.Value.StartsWith(term) ||
            p.TaxCode.StartsWith(term) ||
            p.Email.Value.StartsWith(term) ||
            p.Phone.Value.StartsWith(term)
        );
    }
}