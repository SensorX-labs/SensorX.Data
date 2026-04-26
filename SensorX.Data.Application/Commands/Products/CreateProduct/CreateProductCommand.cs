using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Manufacture,
    Guid? CategoryId,
    string Unit,
    string? Showcase = null,
    List<string>? ImageUrls = null,
    List<ProductAttributeRequest>? Attributes = null
) : IRequest<Result<Guid>>;

public sealed record ProductAttributeRequest(string AttributeName, string AttributeValue);
