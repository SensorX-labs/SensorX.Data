using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Products.DeleteProductCommand;

public class DeleteProductHandler(
    IRepository<Product> _productRepository
) : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            return Result.Failure("Không tìm thấy sản phẩm");
        }

        await _productRepository.DeleteAsync(product, cancellationToken);
        return Result.Success();
    }
}