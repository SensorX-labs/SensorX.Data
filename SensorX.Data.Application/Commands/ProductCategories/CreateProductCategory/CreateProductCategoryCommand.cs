using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.CreateProductCategory;

public class CreateProductCategoryCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; }
}