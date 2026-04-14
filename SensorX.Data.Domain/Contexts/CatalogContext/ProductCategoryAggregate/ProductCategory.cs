using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.Common.Exceptions;

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
    public ProductCategoryId? ParentId { get; private set; }
    public ProductCategory? Parent { get; private set; }

    public void SetParent(ProductCategory? parent)
    {
        if (parent != null && parent.Id == Id)
        {
            throw new DomainException("Danh mục không thể là cha của chính nó.");
        }
        
        ParentId = parent?.Id;
        Parent = parent;
    }
}