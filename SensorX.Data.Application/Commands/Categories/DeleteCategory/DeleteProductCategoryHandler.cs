using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryHandler(
    IRepository<Category> _categoryRepository
) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var id = new CategoryId(request.Id);
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null)
            return Result.Failure("Không tìm thấy danh mục sản phẩm");

        await _categoryRepository.DeleteAsync(category, cancellationToken);

        return Result.Success("Xóa danh mục sản phẩm thành công.");
    }
}