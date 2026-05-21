using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Suppliers.DeleteSupplier;

public sealed record DeleteSupplierCommand([property: JsonIgnore] Guid Id) : IRequest<Result>;
