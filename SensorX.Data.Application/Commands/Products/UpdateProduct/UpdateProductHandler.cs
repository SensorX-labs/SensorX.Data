using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Products.UpdateProduct;

public class UpdateProductHandler(
    IRepository<Product> _productRepository,
    IRepository<Category> _categoryRepository,
    ICloudinaryService _cloudinaryService,
    IPublishEndpoint _publishEndpoint
) : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            if (request.Images != null && request.Images.Count > 0)
                await _cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
            return Result.Failure("Không tìm thấy sản phẩm");
        }

        // Update basic information
        product.UpdateProduct(request.Name.Trim(), request.Manufacture.Trim(), request.Unit.Trim());
        product.SetShowcase(request.Showcase);

        // Update images
        var images = (request.Images ?? []).Select(url => new ProductImage(url)).ToList();
        var imagesToRemove = product.Images.Where(oldImg => !images.Contains(oldImg)).ToList();
        foreach (var img in imagesToRemove)
        {
            product.RemoveImage(img);
            await _cloudinaryService.DeleteImageAsync(img.ImageUrl, cancellationToken);
        }

        var imagesToAdd = images.Where(newImg => !product.Images.Contains(newImg)).ToList();
        foreach (var img in imagesToAdd)
        {
            product.AddImage(img);
        }

        // Update attributes
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

        var categoryId = new CategoryId(request.CategoryId);
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
            return Result.Failure("Không tìm thấy danh mục sản phẩm");
        product.ChangeCategory(categoryId);

        await _publishEndpoint.Publish(new UpdateProductEvent(
            product.Id,
            product.Name,
            product.Manufacture,
            product.Unit,
            product.UpdatedAt
        ), cancellationToken);

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success("Cập nhật sản phẩm thành công");
    }
}