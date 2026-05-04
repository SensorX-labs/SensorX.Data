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
            ((string)p.Code).Contains(term) ||
            ((string)p.Email).Contains(term) ||
            (p.Phone != null && ((string)p.Phone).Contains(term)) ||
            (p.CitizenId != null && ((string)p.CitizenId).Contains(term))
        );
    }
}