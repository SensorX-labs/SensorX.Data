namespace SensorX.Data.Application.Commands.Products.ChangeProductStatus;

using MassTransit;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Enums;

[MessageUrn("Product-Status-Changed-Event")]
[EntityName("Product-Status-Changed-Event")]
public sealed record ChangeProductStatusEvent(
    Guid Id,
    ProductStatus Status,
    DateTimeOffset? UpdatedAt
);