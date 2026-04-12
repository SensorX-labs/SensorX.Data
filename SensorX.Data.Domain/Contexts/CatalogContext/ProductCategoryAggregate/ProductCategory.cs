using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;

public class ProductCategory : Entity<ProductCategoryId>, IAggregateRoot
{
    public ProductCategory(ProductCategoryId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public ProductCategory? Parent { get; private set; }

    public ProductCategory CreateSubCategory(string name, string description)
    {
        return new ProductCategory(ProductCategoryId.New(), name, description) { Parent = this };
    }
}