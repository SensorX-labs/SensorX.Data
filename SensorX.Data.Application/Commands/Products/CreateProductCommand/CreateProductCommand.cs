using MediatR;
using SensorX.Data.Application.Common.Dtos.Requests;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
namespace SensorX.Data.Application.Commands.Products.CreateProductCommand;

public class CreateProductCommand : IRequest<Result<Guid>>
{
    public required string Name { get; set; }
    public required string Manufacture { get; set; }
    public Guid CategoryId { get; set; }
    public required string Unit { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

    // show case
    public string? ShowcaseSummary { get; set; }
    public string? ShowcaseBody { get; set; }

    // images 
    public List<string> ImageUrls { get; set; } = [];
    // attributes
    public List<ProductAttributeRequest> Attributes { get; set; } = [];


}