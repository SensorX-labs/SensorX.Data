using SensorX.Data.Application.Common.Dtos.Responses;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Manufacture { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Status { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    
    public ProductShowcaseResponse? Showcase { get; set; }
    public List<string> Images { get; set; } = [];
    public List<ProductAttributeResponse> Attributes { get; set; } = [];
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
