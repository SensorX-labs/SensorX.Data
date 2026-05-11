using MassTransit;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

[MessageUrn("Product-Updated-Event")]
[EntityName("Product-Updated-Event")]
public sealed record UpdateProductEvent(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    string CategoryName,
    string Unit,
    ProductStatus Status,
    DateTimeOffset? UpdatedAt
);