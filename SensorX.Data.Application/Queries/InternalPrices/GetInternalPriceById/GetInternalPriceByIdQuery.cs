using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceById;

public sealed record GetInternalPriceByIdQuery(Guid Id) : IRequest<Result<GetInternalPriceByIdResponse>>;
