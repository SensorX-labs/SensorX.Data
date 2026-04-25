using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Commands.Products.ChangeProductStatus;

public sealed record ChangeProductStatusCommand(
    [property: JsonIgnore] Guid Id,
    ProductStatus Status
) : IRequest<Result>;