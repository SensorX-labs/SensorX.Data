using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Suppliers.CreateSupplier;

public sealed record CreateSupplierCommand(
    string Name,
    string? Description = null
) : IRequest<Result<Guid>>;
