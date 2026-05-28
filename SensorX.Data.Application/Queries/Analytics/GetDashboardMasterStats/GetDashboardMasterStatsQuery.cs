using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Analytics.GetDashboardMasterStats;

public record GetDashboardMasterStatsQuery : IRequest<Result<GetDashboardMasterStatsResponse>>;

public class GetDashboardMasterStatsResponse
{
    public int TotalCustomers { get; set; }
    public int TotalProducts { get; set; }
    public int TotalStaffs { get; set; }
}
