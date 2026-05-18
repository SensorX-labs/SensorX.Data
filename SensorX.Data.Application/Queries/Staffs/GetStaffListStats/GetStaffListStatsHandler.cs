using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffListStats;

public sealed class GetStaffListStatsHandler(
    IQueryBuilder<Staff> _staffBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetStaffListStatsQuery, Result<GetStaffListStatsResponse>>
{
    public async Task<Result<GetStaffListStatsResponse>> Handle(GetStaffListStatsQuery request, CancellationToken cancellationToken)
    {
        var query = _staffBuilder.QueryAsNoTracking;

        var totalCount = await _queryExecutor.CountAsync(query, cancellationToken);
        var activeCount = await _queryExecutor.CountAsync(query.Where(s => s.Status == StaffStatus.Active), cancellationToken);
        var onLeaveCount = await _queryExecutor.CountAsync(query.Where(s => s.Status == StaffStatus.OnLeave), cancellationToken);
        var resignedCount = await _queryExecutor.CountAsync(query.Where(s => s.Status == StaffStatus.Resigned), cancellationToken);

        var result = new GetStaffListStatsResponse(
            totalCount,
            activeCount,
            onLeaveCount,
            resignedCount
        );

        return Result<GetStaffListStatsResponse>.Success(result);
    }
}
