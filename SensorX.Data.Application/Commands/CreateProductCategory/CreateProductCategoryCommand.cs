using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace Sensorx.Data.Application.Commands.CreateProductCategory;

public class CreateProductCategoryCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
}