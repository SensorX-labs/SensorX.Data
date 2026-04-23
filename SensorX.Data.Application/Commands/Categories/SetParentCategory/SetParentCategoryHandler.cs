using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Categories.SetParentCategory;

public class SetParentCategoryHandler(
    IRepository<Category> _categoryRepository,
    IQueryBuilder<Category> _categoryQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<SetParentCategoryCommand, Result>
{
    public async Task<Result> Handle(SetParentCategoryCommand request, CancellationToken cancellationToken)
    {
        var id = new CategoryId(request.Id);
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
            return Result.Failure("Không tìm thấy danh mục sản phẩm");

        Category? parent = null;

        if (request.ParentId.HasValue)
        {
            var newParentId = new CategoryId(request.ParentId.Value);

            if (newParentId == id)
                return Result.Failure("Danh mục cha không thể là chính nó.");

            var query = _categoryQueryBuilder.QueryAsNoTracking.Select(x => new { x.Id, x.ParentId });
            var categoryTree = await _queryExecutor.ToListAsync(query, cancellationToken);
            var parentMap = categoryTree.ToDictionary(x => x.Id, x => x.ParentId);

            CategoryId? currentIdToCheck = newParentId;
            while (currentIdToCheck != null)
            {
                if (parentMap.TryGetValue(currentIdToCheck, out var parentIdOfCurrent))
                {
                    if (parentIdOfCurrent == id)
                    {
                        return Result.Failure("Danh mục cha không thể là danh mục con của danh mục này.");
                    }
                    currentIdToCheck = parentIdOfCurrent;
                }
                else
                {
                    break;
                }
            }

            parent = await _categoryRepository.GetByIdAsync(newParentId, cancellationToken);
            if (parent is null)
                return Result.Failure("Không tìm thấy danh mục cha");
        }

        category.SetParent(parent);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return Result.Success("Cập nhật danh mục sản phẩm thành công.");
    }
}