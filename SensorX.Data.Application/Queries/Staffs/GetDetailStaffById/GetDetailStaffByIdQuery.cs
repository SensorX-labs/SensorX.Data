using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;

public record GetDetailStaffByIdQuery(Guid StaffId) : IRequest<Result<GetDetailStaffByIdResponse>>;