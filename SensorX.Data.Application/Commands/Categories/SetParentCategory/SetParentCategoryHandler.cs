using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Categories.SetParentCategory;

public class SetParentCategoryHandler(
    IRepository<Category> _categoryRepository
) : IRequestHandler<SetParentCategoryCommand, Result>
{
    public async Task<Result> Handle(SetParentCategoryCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra danh mục tồn tại
        var id = new CategoryId(request.Id);
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
            return Result.Failure("Không tìm thấy danh mục sản phẩm");

        // Set parent nếu có
        Category? parent = null;
        if (request.ParentId.HasValue)
        {
            var parentId = new CategoryId(request.ParentId.Value);
            parent = await _categoryRepository.GetByIdAsync(parentId, cancellationToken);
            if (parent is null)
                return Result.Failure("Không tìm thấy danh mục cha");
        }

        category.SetParent(parent);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return Result.Success("Cập nhật danh mục sản phẩm thành công.");
    }
}