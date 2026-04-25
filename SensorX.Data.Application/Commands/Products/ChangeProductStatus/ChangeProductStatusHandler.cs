using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Products.ChangeProductStatus;

public class ChangeProductStatusHandler(
    IRepository<Product> _productRepository
) : IRequestHandler<ChangeProductStatusCommand, Result>
{
    public async Task<Result> Handle(ChangeProductStatusCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return Result.Failure("Sản phẩm không tồn tại");

        if (request.Status == ProductStatus.Active)
            product.Activate();
        else
            product.Inactivate();
        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success("Cập nhật trạng thái sản phẩm thành công");
    }
}