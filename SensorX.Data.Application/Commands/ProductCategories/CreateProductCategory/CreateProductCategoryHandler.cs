using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.CreateProductCategory;

public class CreateProductCategoryHandler(
    IRepository<ProductCategory> _productCategoryRepository
) : IRequestHandler<CreateProductCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
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
        
        var productCategory = new ProductCategory(
            ProductCategoryId.New(), 
            request.Name,
            request.Description
        );
        productCategory.SetParent(parent);  
        
        await _productCategoryRepository.AddAsync(productCategory, cancellationToken);
        return Result<Guid>.Success(productCategory.Id.Value);
    }
}