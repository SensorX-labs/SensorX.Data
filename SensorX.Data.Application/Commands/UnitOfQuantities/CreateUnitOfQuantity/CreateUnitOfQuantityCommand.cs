using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.UnitOfQuantities.CreateUnitOfQuantity;

public sealed record CreateUnitOfQuantityCommand(
    string Name,
    string? Description = null
) : IRequest<Result<Guid>>;
