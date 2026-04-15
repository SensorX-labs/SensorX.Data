using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.SetParentProductCategory;

public class SetParentProductCategoryCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
}