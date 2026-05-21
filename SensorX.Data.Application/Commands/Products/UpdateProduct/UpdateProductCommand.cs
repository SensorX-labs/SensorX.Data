using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    [property: JsonIgnore] Guid Id,
    string Name,
    Guid SupplierId,
    Guid CategoryId,
    Guid UnitOfQuantityId,
    string? Showcase = null,
    List<string>? Images = null,
    List<ProductAttributeCommand>? Attributes = null
) : IRequest<Result>;

public sealed record ProductAttributeCommand(string Name, string Value);
