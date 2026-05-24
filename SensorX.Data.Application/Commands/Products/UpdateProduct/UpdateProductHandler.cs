using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

public class UpdateProductHandler(
    IRepository<Product> productRepository,
    IRepository<Category> categoryRepository,
    IRepository<Supplier> supplierRepository,
    IRepository<UnitOfQuantity> unitOfQuantityRepository,
    ICloudinaryService cloudinaryService,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var product = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            if (request.Images is { Count: > 0 })
                await cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
            return Result.Failure("Không tìm thấy sản phẩm");
        }

        var categoryId = new CategoryId(request.CategoryId);
        var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
            return Result.Failure("Không tìm thấy danh mục sản phẩm");

        var supplierId = new SupplierId(request.SupplierId);
        var supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);
        if (supplier is null)
            return Result.Failure("Không tìm thấy nhà cung cấp");

        var unitOfQuantityId = new UnitOfQuantityId(request.UnitOfQuantityId);
        var unitOfQuantity = await unitOfQuantityRepository.GetByIdAsync(unitOfQuantityId, cancellationToken);
        if (unitOfQuantity is null)
            return Result.Failure("Không tìm thấy đơn vị tính");

        product.UpdateProduct(request.Name, supplierId, unitOfQuantityId);
        product.SetShowcase(request.Showcase);

        var images = (request.Images ?? []).Select(url => new ProductImage(url)).ToList();
        var imagesToRemove = product.Images.Where(oldImg => !images.Contains(oldImg)).ToList();
        foreach (var img in imagesToRemove)
        {
            product.RemoveImage(img);
            await cloudinaryService.DeleteImageAsync(img.ImageUrl, cancellationToken);
        }

        var imagesToAdd = images.Where(newImg => !product.Images.Contains(newImg)).ToList();
        foreach (var img in imagesToAdd)
        {
            product.AddImage(img);
        }

        var attributes = (request.Attributes ?? []).Select(attr => new ProductAttribute(attr.Name.Trim(), attr.Value.Trim())).ToList();
        var attributesToRemove = product.Attributes.Where(oldAttr => !attributes.Contains(oldAttr)).ToList();
        foreach (var attr in attributesToRemove)
        {
            product.RemoveProductAttribute(attr);
        }

        var attributesToAdd = attributes.Where(newAttr => !product.Attributes.Contains(newAttr)).ToList();
        foreach (var attr in attributesToAdd)
        {
            product.AddProductAttribute(attr);
        }

        product.ChangeCategory(categoryId);

        await publishEndpoint.Publish(new UpdateProductEvent(
            product.Id,
            product.Name,
            supplier.Name,
            unitOfQuantity.Name,
            product.UpdatedAt
        ), cancellationToken);

        await productRepository.UpdateAsync(product, cancellationToken);

        return Result.Success("Cập nhật sản phẩm thành công");
    }
}
