using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public class Product : Entity<ProductId>, IAggregateRoot, ICreationTrackable, IUpdateTrackable
{
    private Product(
        ProductId id,
        Code code,
        string name,
        SupplierId supplierId,
        CategoryId categoryId,
        ProductStatus status,
        UnitOfQuantityId unitOfQuantityId
    ) : base(id)
    {
        Code = code;
        Name = name;
        SupplierId = supplierId;
        CategoryId = categoryId;
        Status = status;
        UnitOfQuantityId = unitOfQuantityId;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Product Create(
        Code code,
        string name,
        SupplierId supplierId,
        CategoryId categoryId,
        ProductStatus status,
        UnitOfQuantityId unitOfQuantityId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên sản phẩm không được để trống");
        if (supplierId.Value == Guid.Empty)
            throw new DomainException("Nhà cung cấp không được để trống");
        if (unitOfQuantityId.Value == Guid.Empty)
            throw new DomainException("Đơn vị tính không được để trống");

        return new Product(ProductId.New(), code, name.Trim(), supplierId, categoryId, status, unitOfQuantityId);
    }

    public Code Code { get; private set; }
    public string Name { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public UnitOfQuantityId UnitOfQuantityId { get; private set; }
    public string? Showcase { get; private set; }

    private readonly List<ProductImage> _images = [];
    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    private readonly List<ProductAttribute> _attributes = [];
    public IReadOnlyList<ProductAttribute> Attributes => _attributes.AsReadOnly();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Activate() => Status = ProductStatus.Active;
    public void Inactivate() => Status = ProductStatus.Inactive;

    public void UpdateProduct(string name, SupplierId supplierId, UnitOfQuantityId unitOfQuantityId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên sản phẩm không được để trống");
        if (supplierId.Value == Guid.Empty)
            throw new DomainException("Nhà cung cấp không được để trống");
        if (unitOfQuantityId.Value == Guid.Empty)
            throw new DomainException("Đơn vị tính không được để trống");

        Name = name.Trim();
        SupplierId = supplierId;
        UnitOfQuantityId = unitOfQuantityId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void AddImage(ProductImage image)
    {
        if (_images.Contains(image))
            return;
        _images.Add(image);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RemoveImage(ProductImage image)
    {
        if (!_images.Contains(image))
            return;
        _images.Remove(image);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangeCategory(CategoryId newCategoryId)
    {
        CategoryId = newCategoryId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void AddProductAttribute(ProductAttribute newAttribute)
    {
        if (_attributes.Contains(newAttribute))
            return;
        _attributes.Add(newAttribute);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RemoveProductAttribute(ProductAttribute attribute)
    {
        if (!_attributes.Contains(attribute))
            return;
        _attributes.Remove(attribute);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetShowcase(string? showcase)
    {
        Showcase = showcase;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
