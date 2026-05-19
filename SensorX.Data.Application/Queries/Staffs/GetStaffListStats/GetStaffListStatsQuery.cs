using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffListStats;

public sealed record GetStaffListStatsQuery() : IRequest<Result<GetStaffListStatsResponse>>;
