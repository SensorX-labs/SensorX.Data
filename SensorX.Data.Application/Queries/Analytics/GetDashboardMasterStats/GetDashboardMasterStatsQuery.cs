using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Analytics.GetDashboardMasterStats;

public record GetDashboardMasterStatsQuery(
    string TimeRange = "month" // today, week, month, year, all
) : IRequest<Result<GetDashboardMasterStatsResponse>>;

public class GetDashboardMasterStatsResponse
{
    public int TotalCustomers { get; set; }
    public int TotalProducts { get; set; }
    public int TotalStaffs { get; set; }
    public int NewCustomers { get; set; }
    public int PreviousNewCustomers { get; set; }
}
