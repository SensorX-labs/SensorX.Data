using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

public class GetStaffMetricsHandler(
    IQueryBuilder<Staff> staffQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetStaffMetricsQuery, Result<GetStaffMetricsResponse>>
{
    public async Task<Result<GetStaffMetricsResponse>> Handle(
        GetStaffMetricsQuery request,
        CancellationToken cancellationToken)
    {
        var query = staffQueryBuilder.QueryAsNoTracking
            .Where(x => x.Id == new StaffId(request.StaffId))
            .Select(x => new GetStaffMetricsResponse(
                x.Id.Value,
                x.Code.Value,
                x.Name,
                x.Email.Value,
                x.Phone != null ? x.Phone.Value : string.Empty,
                x.Department,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var staff = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

        if (staff is null)
            return Result<GetStaffMetricsResponse>.Failure("Nhân viên không tồn tại");

        return Result<GetStaffMetricsResponse>.Success(staff);
    }
}
