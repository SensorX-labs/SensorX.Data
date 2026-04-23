using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Products.DeleteProduct;

public sealed record DeleteProductCommand([property: JsonIgnore] Guid Id) : IRequest<Result>;
