using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    [property: JsonIgnore] Guid Id,
    string Name,
    string Manufacture,
    Guid CategoryId,
    string Unit,
    string? Showcase = null,
    List<string>? ImageUrls = null,
    List<ProductAttributeCommand>? Attributes = null
) : IRequest<Result>;

public sealed record ProductAttributeCommand(string AttributeName, string AttributeValue);
