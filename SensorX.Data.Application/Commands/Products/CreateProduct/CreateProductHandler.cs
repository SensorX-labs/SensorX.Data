using MassTransit;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Products.CreateProduct;

public class CreateProductHandler(
    IRepository<Product> productRepository,
    IRepository<Category> categoryRepository,
    IRepository<Supplier> supplierRepository,
    IRepository<UnitOfQuantity> unitOfQuantityRepository,
    ICloudinaryService cloudinaryService,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var categoryId = new CategoryId(request.CategoryId);
            var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category is null)
            {
                if (request.Images is { Count: > 0 })
                    await cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
                return Result<Guid>.Failure("Danh mục sản phẩm không tồn tại");
            }

            var supplierId = new SupplierId(request.SupplierId);
            var supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);
            if (supplier is null)
            {
                if (request.Images is { Count: > 0 })
                    await cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
                return Result<Guid>.Failure("Nhà cung cấp không tồn tại");
            }

            var unitOfQuantityId = new UnitOfQuantityId(request.UnitOfQuantityId);
            var unitOfQuantity = await unitOfQuantityRepository.GetByIdAsync(unitOfQuantityId, cancellationToken);
            if (unitOfQuantity is null)
            {
                if (request.Images is { Count: > 0 })
                    await cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
                return Result<Guid>.Failure("Đơn vị tính không tồn tại");
            }

            var code = Code.Create("PRD");

            var product = Product.Create(
                code,
                request.Name,
                supplier.Id,
                category.Id,
                ProductStatus.Active,
                unitOfQuantity.Id
            );
            product.SetShowcase(request.Showcase);

            if (request.Images != null)
            {
                foreach (var imageUrl in request.Images)
                {
                    product.AddImage(new ProductImage(imageUrl));
                }
            }

            if (request.Attributes != null)
            {
                foreach (var attrDto in request.Attributes)
                {
                    product.AddProductAttribute(new ProductAttribute(attrDto.Name, attrDto.Value));
                }
            }

            await publishEndpoint.Publish(new CreateProductEvent(
                product.Id,
                product.Code,
                product.Name,
                supplier.Name,
                unitOfQuantity.Name,
                product.Status,
                product.CreatedAt
            ), cancellationToken);

            await productRepository.AddAsync(product, cancellationToken);

            return Result<Guid>.Success(product.Id.Value);
        }
        catch (Exception ex)
        {
            if (request.Images is { Count: > 0 })
                await cloudinaryService.DeleteImagesAsync(request.Images, cancellationToken);
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
