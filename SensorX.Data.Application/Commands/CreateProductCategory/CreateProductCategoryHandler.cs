using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.SeedWork;

namespace Sensorx.Data.Application.Commands.CreateProductCategory;

public class CreateProductCategoryHandler(
    IRepository<ProductCategory> _productCategoryRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<CreateProductCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = new ProductCategory(
            ProductCategoryId.New(), 
            request.Name,
            request.Description
        );
        await _productCategoryRepository.AddAsync(productCategory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(productCategory.Id.Value);
    }
}