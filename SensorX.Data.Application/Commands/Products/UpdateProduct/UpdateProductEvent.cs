using MassTransit;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

[MessageUrn("Product-Updated-Event")]
[EntityName("Product-Updated-Event")]
public sealed record UpdateProductEvent(
    Guid Id,
    string Name,
    Guid SupplierId,
    Guid UnitOfQuantityId,
    DateTimeOffset? UpdatedAt
);
