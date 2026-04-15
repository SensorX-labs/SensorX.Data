using SensorX.Data.Domain.Common.Exceptions;

namespace SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

public record ProductShowcase
{
    public string Summary { get; private set; }
    public string Body { get; private set; }

    public ProductShowcase(string summary, string body)
    {
        if (string.IsNullOrWhiteSpace(summary))
            throw new DomainException("Tóm tắt không được để trống");
        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Nội dung không được để trống");
        Summary = summary;
        Body = body;
    }
}