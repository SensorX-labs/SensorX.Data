using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Suppliers.UpdateSupplier;

public sealed record UpdateSupplierCommand(
    [property: JsonIgnore] Guid Id,
    string Name,
    string? Description = null
) : IRequest<Result>;
