using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;

public sealed record GetInternalPricesByProductIdQuery(Guid ProductId) : IRequest<Result<GetInternalPricesByProductIdResponse>>;
