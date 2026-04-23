using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public record GetPageListProductsResponse(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    string CategoryName,
    decimal Price,
    ProductStatus Status,
    DateTimeOffset CreatedAt,
    List<string> Images
);

public class ProductOffsetPagedResult : OffsetPagedResult<GetPageListProductsResponse> { }