using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;

public class Category : Entity<CategoryId>, IAggregateRoot
{
    public Category(CategoryId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public CategoryId? ParentId { get; private set; }
    public Category? Parent { get; private set; }

    public void SetParent(Category? parent)
    {
        if (parent != null && parent.Id == Id)
        {
            throw new DomainException("Danh mục không thể là cha của chính nó.");
        }

        ParentId = parent?.Id;
        Parent = parent;
    }
}