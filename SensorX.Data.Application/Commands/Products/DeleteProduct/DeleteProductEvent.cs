using MassTransit;
namespace SensorX.Data.Application.Commands.Products.DeleteProduct;

[MessageUrn("Product-Deleted-Event")]
[EntityName("Product-Deleted-Event")]
public sealed record DeleteProductEvent(Guid Id);