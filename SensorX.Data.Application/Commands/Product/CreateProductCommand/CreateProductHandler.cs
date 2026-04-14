using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.CreateProductCommand;

public class CreateProductHandler(
    IRepository<Product> _productRepository,
    IRepository<ProductCategory> _productCategoryRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
{
    // Validate Status
    if (!Enum.TryParse<ProductStatus>(request.Status, ignoreCase: true, out var status))
    {
        return Result<Guid>.Failure($"Status '{request.Status}' không hợp lệ. Dùng: Active, Inactive");
    }

    // Validate Name
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Result<Guid>.Failure("Tên sản phẩm không được để trống");
    }

    // Validate Manufacture
    if (string.IsNullOrWhiteSpace(request.Manufacture))
    {
        return Result<Guid>.Failure("Nhà sản xuất không được để trống");
    }

    // Validate Unit
    if (string.IsNullOrWhiteSpace(request.Unit))
    {
        return Result<Guid>.Failure("Đơn vị tính không được để trống");
    }

    // Validate CategoryId exists
    var categoryId = new ProductCategoryId(request.CategoryId);
    var category = await _productCategoryRepository.GetByIdAsync(categoryId, cancellationToken);
    if (category == null)
    {
        return Result<Guid>.Failure("Danh mục sản phẩm không tồn tại");
    }

    var code = Code.Create("PRD");
    
    var product = new Product(
        ProductId.New(),
        code,
        request.Name.Trim(),
        request.Manufacture.Trim(),
        categoryId,
        status,
        request.Unit.Trim()
    );

    // thêm show case nếu có
    if (!string.IsNullOrWhiteSpace(request.ShowcaseSummary) || !string.IsNullOrWhiteSpace(request.ShowcaseBody))
    {
        product.SetShowcase(request.ShowcaseSummary, request.ShowcaseBody);
    }

    // thêm image
    foreach (var imageDto in request.ImageUrls)
    {
        product.AddImage(new ProductImage(imageDto));
    }

    // thêm attributes
    foreach (var attrDto in request.Attributes)
    {
        product.AddProductAttribute(new ProductAttribute(attrDto.AttributeName, attrDto.AttributeValue));
    }

    await _productRepository.AddAsync(product, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result<Guid>.Success(product.Id.Value);
}
}