using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Products.ChangeProductStatus;

public class ChangeProductStatusHandler(
    IRepository<Product> _productRepository,
    MassTransit.IPublishEndpoint _publishEndpoint
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

        // Sync to other services (e.g. Warehouse)
        await _publishEndpoint.Publish(new Events.ProductSyncEvent
        {
            ProductId = product.Id.Value,
            Code = product.Code.Value,
            Name = product.Name,
            Unit = product.Unit,
            Manufacture = product.Manufacture,
            Status = product.Status.ToString(),
            Timestamp = DateTimeOffset.UtcNow
        }, cancellationToken);

        return Result.Success("Cập nhật trạng thái sản phẩm thành công");
    }
}