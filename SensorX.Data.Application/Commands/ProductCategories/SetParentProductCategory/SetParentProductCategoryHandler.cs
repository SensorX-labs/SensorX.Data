using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.SetParentProductCategory;

public class SetParentProductCategoryHandler(
    IRepository<ProductCategory> _productCategoryRepository
) : IRequestHandler<SetParentProductCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SetParentProductCategoryCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra danh mục tồn tại
        var id = new ProductCategoryId(request.Id);
        var productCategory = await _productCategoryRepository.GetByIdAsync(id, cancellationToken);
        if (productCategory == null)
        {
            return Result<Guid>.Failure("Không tìm thấy danh mục sản phẩm");
        }

        // Set parent nếu có
        ProductCategory? parent = null;
        if (request.ParentId.HasValue)
        {
            var parentId = new ProductCategoryId(request.ParentId.Value);
            parent = await _productCategoryRepository.GetByIdAsync(parentId, cancellationToken);
            if (parent == null)
            {
                return Result<Guid>.Failure("Không tìm thấy danh mục cha");
            }
        }

        productCategory.SetParent(parent);

        await _productCategoryRepository.UpdateAsync(productCategory, cancellationToken);

        return Result<Guid>.Success(productCategory.Id.Value);
    }
}