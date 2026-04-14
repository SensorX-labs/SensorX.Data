using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}