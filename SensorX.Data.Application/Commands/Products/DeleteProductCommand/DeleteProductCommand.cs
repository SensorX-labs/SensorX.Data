using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Commands.Products.DeleteProductCommand;

public record DeleteProductCommand(Guid ProductId) : IRequest<Result>;
