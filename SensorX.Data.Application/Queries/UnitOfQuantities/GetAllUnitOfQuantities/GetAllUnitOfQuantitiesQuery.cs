using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetAllUnitOfQuantities;

public sealed record GetAllUnitOfQuantitiesQuery : IRequest<Result<List<GetAllUnitOfQuantitiesResponse>>>;
