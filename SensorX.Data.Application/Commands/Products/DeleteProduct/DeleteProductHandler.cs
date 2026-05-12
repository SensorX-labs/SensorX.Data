using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Products.DeleteProduct;

public class DeleteProductHandler(
    IRepository<Product> _productRepository,
    MassTransit.IPublishEndpoint _publishEndpoint
) : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return Result.Failure("Không tìm thấy sản phẩm");

        if (product.Status == ProductStatus.Active)
            return Result.Failure("Sản phẩm đang hoạt động không thể xóa. Vui lòng cập nhật trạng thái sang ngừng kinh doanh trước khi xóa.");

        await _productRepository.DeleteAsync(product, cancellationToken);

        // Sync to other services
        await _publishEndpoint.Publish(new Events.ProductDeletedEvent
        {
            ProductId = product.Id.Value,
            Timestamp = DateTimeOffset.UtcNow
        }, cancellationToken);

        return Result.Success("Xóa sản phẩm thành công");
    }
}