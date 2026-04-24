using System.Net;
using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetProductDetail;

public sealed class GetProductDetailHandler(
    IQueryBuilder<Product> _productQueryBuilder,
    IQueryBuilder<Category> _categoryQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetProductDetailQuery, Result<GetProductDetailResponse>>
{
    public async Task<Result<GetProductDetailResponse>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        var query = from product in _productQueryBuilder.QueryAsNoTracking.Where(x => x.Id == request.Id)
                    join c in _categoryQueryBuilder.QueryAsNoTracking on product.CategoryId equals c.Id into categoryList
                    from category in categoryList.DefaultIfEmpty()
                    select new GetProductDetailResponse(
                        product.Id,
                        product.Code,
                        product.Name,
                        product.Manufacture,
                        category.Name,
                        product.Unit,
                        product.Showcase,
                        product.Attributes.Select(x => new ProductAttributeResponse(x.AttributeName, x.AttributeValue)).ToList(),
                        product.Status,
                        product.CreatedAt,
                        product.UpdatedAt,
                        product.Images.Select(x => x.ImageUrl).ToList()
                    );
        var result = await _queryExecutor.FirstOrDefaultAsync(query, cancellationToken);
        if (result == null)
            return Result<GetProductDetailResponse>.Failure("Sản phẩm không tồn tại!");

        return Result<GetProductDetailResponse>.Success(result);
    }
}