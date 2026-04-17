using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Categories.SetParentCategory;

public class SetParentCategoryCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
}