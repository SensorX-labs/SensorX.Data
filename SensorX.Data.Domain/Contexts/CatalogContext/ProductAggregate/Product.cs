using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public class Product : Entity<ProductId>, IAggregateRoot, ICreationTrackable, IUpdateTrackable
{
    private Product(
        ProductId id,
        Code code,
        string name,
        string manufacture,
        ProductStatus status,
        string unit
    ) : base(id)
    {
        Code = code;
        Name = name;
        Manufacture = manufacture;
        Status = status;
        Unit = unit;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Product Create(
        Code code,
        string name,
        string manufacture,
        CategoryId? categoryId,
        ProductStatus status,
        string unit
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Tên sản phẩm không được để trống");
        if (string.IsNullOrWhiteSpace(manufacture))
            throw new DomainException("Hãng sản xuất không được để trống");
        if (string.IsNullOrWhiteSpace(unit))
            throw new DomainException("Đơn vị tính không được để trống");

        var product = new Product(ProductId.New(), code, name, manufacture, status, unit);
        if (categoryId != null)
            product.ChangeCategory(categoryId);
        return product;
    }

    public Code Code { get; private set; }
    public string Name { get; private set; }
    public string Manufacture { get; private set; }
    public CategoryId? CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public string Unit { get; private set; }
    public string? Showcase { get; private set; }

    private readonly List<ProductImage> _images = [];
    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    private readonly List<ProductAttribute> _attributes = [];
    public IReadOnlyList<ProductAttribute> Attributes => _attributes.AsReadOnly();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Activate() => Status = ProductStatus.Active;
    public void Inactivate() => Status = ProductStatus.Inactive;

    public void UpdateProduct(string name, string manufacture, string unit)
    {
        Name = name;
        Manufacture = manufacture;
        Unit = unit;
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