using Ardalis.Specification;
namespace SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate.Specifications;

public class DuplicateCategoryNameSpec : Specification<Category>
{
    public DuplicateCategoryNameSpec(string name)
    {
        Query.Where(x => x.Name.ToLower() == name.ToLower());
    }
}