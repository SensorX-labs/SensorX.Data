using MassTransit;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Commands.Products.CreateProduct;

[MessageUrn("Product-Created-Event")]
[EntityName("Product-Created-Event")]
public sealed record CreateProductEvent(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    string CategoryName,
    string Unit,
    ProductStatus Status,
    DateTimeOffset CreatedAt
);