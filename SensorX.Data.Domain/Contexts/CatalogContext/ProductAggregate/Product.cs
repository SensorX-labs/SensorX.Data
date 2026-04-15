using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public class Product : Entity<ProductId>, IAggregateRoot, ICreationTrackable, IUpdateTrackable
{
    public Product(
        ProductId id,
        Code code,
        string name,
        string manufacture,
        CategoryId categoryId,
        ProductStatus status,
        string unit
    ) : base(id)
    {
        Code = code;
        Name = name;
        Manufacture = manufacture;
        CategoryId = categoryId;
        Status = status;
        Unit = unit;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Code Code { get; private set; }
    public string Name { get; private set; }
    public string Manufacture { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public ProductStatus Status { get; private set; }
    public string Unit { get; private set; }

    private readonly List<ProductImage> _images = [];
    public IReadOnlyList<ProductImage> Images => _images.AsReadOnly();

    private readonly List<ProductAttribute> _attributes = [];
    public IReadOnlyList<ProductAttribute> Attributes => _attributes.AsReadOnly();

    private ProductShowcase? _showcase;
    public ProductShowcase? Showcase => _showcase;

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
        _images.Add(image);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void RemoveImage(ProductImage image)
    {
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
        _attributes.Add(newAttribute);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void RemoveProductAttribute(ProductAttribute attribute)
    {
        _attributes.Remove(attribute);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void SetShowcase(string summary, string body)
    {
        _showcase = new ProductShowcase(summary, body);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}