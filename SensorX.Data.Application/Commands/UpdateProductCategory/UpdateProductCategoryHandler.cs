using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.UpdateProductCategory;

public class UpdateProductCategoryHandler(
    IRepository<ProductCategory> _productCategoryRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<UpdateProductCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        // kiểm tra tồn tại
        var id = new ProductCategoryId(request.Id);
        var productCategory = await _productCategoryRepository.GetByIdAsync(id, cancellationToken);
        if (productCategory == null)
        {
            return Result<Guid>.Failure("Không tìm thấy danh mục sản phẩm");
        }

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

        productCategory.Update(request.Name, request.Description, parent);

        await _productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var newproductCategory = await _productCategoryRepository.GetByIdAsync(id, cancellationToken);

        return Result<Guid>.Success(newproductCategory!.Id);
    }
}