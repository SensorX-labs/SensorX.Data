using MassTransit;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

[MessageUrn("Product-Updated-Event")]
[EntityName("Product-Updated-Event")]
public sealed record UpdateProductEvent(
    Guid Id,
    string Name,
    string Manufacture,
    string Unit,
    DateTimeOffset? UpdatedAt
);