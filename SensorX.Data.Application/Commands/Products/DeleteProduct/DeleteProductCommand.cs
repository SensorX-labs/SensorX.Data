using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
namespace SensorX.Data.Application.Commands.Products.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IRequest<Result>;
