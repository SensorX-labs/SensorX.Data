using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

public record GetStaffMetricsQuery(Guid StaffId) : IRequest<Result<GetStaffMetricsResponse>>;