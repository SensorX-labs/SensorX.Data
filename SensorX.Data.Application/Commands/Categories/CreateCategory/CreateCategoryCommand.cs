using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Categories.CreateCategory;

public class CreateCategoryCommand : IRequest<Result<Guid>>
{
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public required string Description { get; set; }
}