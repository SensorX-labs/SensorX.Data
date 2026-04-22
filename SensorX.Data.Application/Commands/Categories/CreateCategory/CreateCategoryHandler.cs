using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate.Specifications;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Categories.CreateCategory;

public class CreateCategoryHandler(
    IRepository<Category> _categoryRepository
) : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.AnyAsync(new DuplicateCategoryNameSpec(request.Name), cancellationToken))
            return Result<Guid>.Failure("Danh mục đã tồn tại");

        var category = Category.Create(
            request.Name,
            request.Description ?? ""
        );

        // Chỉ set parent nếu ParentId được cung cấp
        if (request.ParentId.HasValue)
        {
            var parentId = new CategoryId(request.ParentId.Value);
            var parent = await _categoryRepository.GetByIdAsync(parentId, cancellationToken);
            if (parent is null)
                return Result<Guid>.Failure("Không tìm thấy danh mục cha");

            category.SetParent(parent);
        }

        await _categoryRepository.AddAsync(category, cancellationToken);
        return Result<Guid>.Success(category.Id.Value);
    }
}