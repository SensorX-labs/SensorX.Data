using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetProductListStats;

public sealed record GetProductListStatsQuery : IRequest<Result<ProductListStatsResponse>>;