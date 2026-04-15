using SensorX.Data.Domain.Common.Exceptions;
namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public record ProductAttribute
{
    public string AttributeName { get; private set; }
    public string AttributeValue { get; private set; }

    public ProductAttribute(string attributeName, string attributeValue)
    {
        if (string.IsNullOrWhiteSpace(attributeName))
            throw new DomainException("Tên thuộc tính không được để trống");
        if (string.IsNullOrWhiteSpace(attributeValue))
            throw new DomainException("Giá trị thuộc tính không được để trống");
        AttributeName = attributeName;
        AttributeValue = attributeValue;
    }
}