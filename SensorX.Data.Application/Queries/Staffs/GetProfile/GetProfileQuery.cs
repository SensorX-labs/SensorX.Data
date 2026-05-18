using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetProfile;

public record GetProfileQuery() : IRequest<Result<GetProfileResponse>>;

public record GetProfileResponse(
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