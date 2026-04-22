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
            p.Name.Contains(term) ||
            p.Code.Value.Contains(term) ||
            p.Email.Value.Contains(term) ||
            p.Phone.Value.Contains(term) ||
            p.CitizenId.Value.Contains(term)
        );
    }
}