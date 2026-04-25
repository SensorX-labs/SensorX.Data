using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffByAccountId;

public record GetStaffByAccountIdQuery(Guid AccountId) : IRequest<Result<GetDetailStaffByIdResponse>>;
