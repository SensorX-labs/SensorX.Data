using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Categories.CreateCategory;

public class CreateCategoryHandler(
    IRepository<Category> _CategoryRepository
) : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!request.ParentId.HasValue)
            return Result<Guid>.Failure("Không tìm thấy danh mục cha");

        var parentId = new CategoryId(request.ParentId.Value);
        var parent = await _CategoryRepository.GetByIdAsync(parentId, cancellationToken);
        if (parent is null)
            return Result<Guid>.Failure("Không tìm thấy danh mục cha");

        var category = Category.Create(
            request.Name,
            request.Description
        );
        category.SetParent(parent);

        await _CategoryRepository.AddAsync(category, cancellationToken);
        return Result<Guid>.Success(category.Id.Value);
    }
}