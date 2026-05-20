using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.UnitOfQuantities.DeleteUnitOfQuantity;

public sealed record DeleteUnitOfQuantityCommand([property: JsonIgnore] Guid Id) : IRequest<Result>;
