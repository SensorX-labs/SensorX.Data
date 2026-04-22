using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
namespace SensorX.Data.Application.Commands.Products.CreateProductCommand;

public record CreateProductCommand(
    string Name,
    string Manufacture,
    Guid CategoryId,
    string Unit,
    ProductStatus Status = ProductStatus.Active,
    string? ShowcaseSummary = null,
    string? ShowcaseBody = null,
    List<string>? ImageUrls = null,
    List<ProductAttributeRequest>? Attributes = null
) : IRequest<Result<Guid>>
{
    public List<string> ImageUrls { get; init; } = ImageUrls ?? [];
    public List<ProductAttributeRequest> Attributes { get; init; } = Attributes ?? [];
}

public record ProductAttributeRequest(string AttributeName, string AttributeValue);
