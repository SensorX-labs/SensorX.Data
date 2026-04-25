using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Categories.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    Guid? ParentId,
    string? Description
) : IRequest<Result<Guid>>;
