using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Common.QueryExtensions.Search;

public static class StaffSearch
{
    public static IQueryable<Staff> ApplySearch(
        this IQueryable<Staff> query,
        string? searchTerm
    )
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var term = searchTerm.Trim();

        return query.Where(p =>
            p.Name.StartsWith(term) ||
            p.Code.Value.StartsWith(term) ||
            p.Email.Value.StartsWith(term) ||
            p.Phone.Value.StartsWith(term) ||
            p.CitizenId.Value.StartsWith(term)
        );
    }
}