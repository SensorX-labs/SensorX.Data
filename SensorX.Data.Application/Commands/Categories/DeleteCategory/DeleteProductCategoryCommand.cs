using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}