using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.Products.GetProductDetail;

public sealed class GetProductDetailHandler(
    IQueryBuilder<Product> productQueryBuilder,
    IQueryBuilder<Category> categoryQueryBuilder,
    IQueryBuilder<Supplier> supplierQueryBuilder,
    IQueryBuilder<UnitOfQuantity> unitOfQuantityQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetProductDetailQuery, Result<GetProductDetailResponse>>
{
    public async Task<Result<GetProductDetailResponse>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        var query = from product in productQueryBuilder.QueryAsNoTracking.Where(x => x.Id == request.Id)
                    join c in categoryQueryBuilder.QueryAsNoTracking on product.CategoryId equals c.Id into categoryList
                    from category in categoryList.DefaultIfEmpty()
                    join s in supplierQueryBuilder.QueryAsNoTracking on product.SupplierId equals s.Id into supplierList
                    from supplier in supplierList.DefaultIfEmpty()
                    join u in unitOfQuantityQueryBuilder.QueryAsNoTracking on product.UnitOfQuantityId equals u.Id into unitList
                    from unit in unitList.DefaultIfEmpty()
                    select new GetProductDetailResponse(
                        product.Id,
                        product.Code,
                        product.Name,
                        product.SupplierId.Value,
                        supplier != null ? supplier.Name : "",
                        category != null ? category.Id : Guid.Empty,
                        category != null ? category.Name : "",
                        product.UnitOfQuantityId.Value,
                        unit != null ? unit.Name : "",
                        product.Showcase,
                        product.Attributes.Select(x => new ProductAttributeResponse(x.AttributeName, x.AttributeValue)).ToList(),
                        product.Status,
                        product.CreatedAt,
                        product.UpdatedAt,
                        product.Images.Select(x => x.ImageUrl).ToList()
                    );

        var result = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);
        if (result == null)
            return Result<GetProductDetailResponse>.Failure("Sản phẩm không tồn tại!");

        return Result<GetProductDetailResponse>.Success(result);
    }
}
