using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Products.CreateProduct;

public class CreateProductHandler(
    IRepository<Product> _productRepository,
    IRepository<Category> _categoryRepository,
    ICloudinaryService _cloudinaryService,
    IPublishEndpoint _publishEndpoint
) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Category? category = null;
            if (request.CategoryId.HasValue)
            {
                var categoryId = new CategoryId(request.CategoryId.Value);
                category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
                if (category is null)
                {
                    if (request.Images != null && request.Images.Count > 0)
                        await _cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
                    return Result<Guid>.Failure("Danh mục sản phẩm không tồn tại");
                }
            }

            var code = Code.Create("PRD");

            var product = Product.Create(
                code,
                request.Name.Trim(),
                request.Manufacture.Trim(),
                category?.Id,
                ProductStatus.Active,
                request.Unit.Trim()
            );
            product.SetShowcase(request.Showcase);

            if (request.Images != null)
            {
                foreach (var imageDto in request.Images)
                {
                    product.AddImage(new ProductImage(imageDto));
                }
            }

            if (request.Attributes != null)
            {
                foreach (var attrDto in request.Attributes)
                {
                    product.AddProductAttribute(new ProductAttribute(attrDto.Name, attrDto.Value));
                }
            }

            await _publishEndpoint.Publish(new CreateProductEvent(
                product.Id,
                product.Code,
                product.Name,
                product.Manufacture,
                product.Unit,
                product.Status,
                product.CreatedAt
            ), cancellationToken);

            await _productRepository.AddAsync(product, cancellationToken);

            return Result<Guid>.Success(product.Id.Value);
        }
        catch (Exception ex)
        {
            if (request.Images != null && request.Images.Count > 0)
                await _cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
            return Result<Guid>.Failure(ex.Message);
        }
    }
}