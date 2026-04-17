using SensorX.Data.Domain.Common.Exceptions;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public record ProductImage
{
    public string ImageUrl { get; private set; }
    public ProductImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("URL ảnh không được để trống");
        ImageUrl = imageUrl;
    }
}