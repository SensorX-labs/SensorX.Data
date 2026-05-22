using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetUnitOfQuantityById;

public sealed record GetUnitOfQuantityByIdQuery(Guid Id) : IRequest<Result<GetUnitOfQuantityByIdResponse>>;
