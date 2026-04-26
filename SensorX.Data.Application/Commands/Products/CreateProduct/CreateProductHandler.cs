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
    IRepository<Category> _categoryRepository
) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        CategoryId? categoryId = null;
        if (request.CategoryId.HasValue)
        {
            categoryId = new CategoryId(request.CategoryId.Value);
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category is null)
                return Result<Guid>.Failure("Danh mục sản phẩm không tồn tại");
        }

        var code = Code.Create("PRD");

        var product = Product.Create(
            code,
            request.Name.Trim(),
            request.Manufacture.Trim(),
            categoryId,
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

        await _productRepository.AddAsync(product, cancellationToken);

        return Result<Guid>.Success(product.Id.Value);
    }
}