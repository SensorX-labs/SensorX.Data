using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceListStats;

public sealed record GetInternalPriceListStatsQuery : IRequest<Result<GetInternalPriceListStatsResponse>>;