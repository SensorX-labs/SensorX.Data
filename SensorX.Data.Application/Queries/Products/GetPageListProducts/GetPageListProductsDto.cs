namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Manufacture { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Status { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    
    public ProductShowcaseDto? Showcase { get; set; }
    public List<ProductImageDto> Images { get; set; } = [];
    public List<ProductAttribute> Attributes { get; set; } = [];
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class ProductShowcaseDto
{
    public string Summary { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class ProductImageDto
{
    public string ImageUrl { get; set; } = string.Empty;
}
