using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;

public record GetDetailStaffByIdQuery(Guid StaffId) : IRequest<Result<GetDetailStaffByIdResponse>>;

public record GetDetailStaffByIdResponse(
  Guid Id,
  string Code,
  string Name,
  string Phone,
  string Email,
  string CitizenId,
  string? Biography,
  DateTimeOffset JoinDate,
  Department Department,
  StaffStatus Status,
  DateTimeOffset CreatedAt,
  string? AvatarUrl
);
