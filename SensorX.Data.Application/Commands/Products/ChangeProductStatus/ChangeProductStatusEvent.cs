namespace SensorX.Data.Application.Commands.Products.ChangeProductStatus;

using MassTransit;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

[MessageUrn("Product-Status-Changed-Event")]
[EntityName("Product-Status-Changed-Event")]
public sealed record ChangeProductStatusEvent(
    Guid Id,
    ProductStatus Status,
    DateTimeOffset? UpdatedAt
);