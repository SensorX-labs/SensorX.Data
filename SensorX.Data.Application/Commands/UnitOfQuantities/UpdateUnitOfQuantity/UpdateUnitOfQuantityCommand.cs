using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.UnitOfQuantities.UpdateUnitOfQuantity;

public sealed record UpdateUnitOfQuantityCommand(
    [property: JsonIgnore] Guid Id,
    string Name,
    string? Description = null
) : IRequest<Result>;
