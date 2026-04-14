using MediatR;
using SensorX.Data.Application.Common.Dtos;
using SensorX.Data.Application.Common.ResponseClient;
namespace SensorX.Data.Application.Commands.CreateProductCommand;
public class CreateProductCommand : IRequest<Result<Guid>>
{
    public String Name { get; set; }
    public String Manufacture { get; set; }
    public Guid CategoryId { get; set; }
    public String Unit { get; set; }

    public string Status { get; set; } = "Active";

    // show case
    public String? ShowcaseSummary { get; set; }
    public String? ShowcaseBody { get; set; }

    // images 
    public List<String> ImageUrls { get; set; } = [];
    // attributes
    public List<ProductAttributeDto> Attributes { get; set; } = [];


}