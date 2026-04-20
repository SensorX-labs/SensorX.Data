using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

public class GetStaffMetricsHandler(
    IRepository<Staff> staffRepository
) : IRequestHandler<GetStaffMetricsQuery, Result<GetStaffMetricsResponse>>
{
    public async Task<Result<GetStaffMetricsResponse>> Handle(
        GetStaffMetricsQuery request,
        CancellationToken cancellationToken)
    {
        var staffId = new StaffId(request.StaffId);
        var staff = await staffRepository.GetByIdAsync(staffId, cancellationToken);

        if (staff is null)
            return Result<GetStaffMetricsResponse>.Failure("Nhân viên không tồn tại");

        var response = new GetStaffMetricsResponse(
            StaffId: staff.Id.Value,
            StaffCode: staff.Code.Value,
            StaffName: staff.Name,
            Email: staff.Email.Value,
            Phone: staff.Phone.Value,
            Department: staff.Department.ToString(),
            CreatedAt: staff.CreatedAt.DateTime,
            UpdatedAt: staff.UpdatedAt?.DateTime
        );

        return Result<GetStaffMetricsResponse>.Success(response);
    }
}
