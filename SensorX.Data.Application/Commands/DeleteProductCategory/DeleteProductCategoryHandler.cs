
namespace SensorX.Data.Application.Commands.DeleteProductCategory;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.SeedWork;

public class DeleteProductCategoryHandler(
    IRepository<ProductCategory> _productCategoryRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<DeleteProductCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var id = new ProductCategoryId(request.Id);
        var productCategory = await _productCategoryRepository.GetByIdAsync(id, cancellationToken);
        if (productCategory == null)
        {
            return Result<Guid>.Failure("Không tìm thấy danh mục sản phẩm");
        }

        await _productCategoryRepository.DeleteAsync(productCategory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(id);
    }
}