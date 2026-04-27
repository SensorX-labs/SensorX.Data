using SensorX.Data.Application.Common.QueryExtensions.KeysetPagination;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed record LoadMoreProductsResponse(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    Guid? CategoryId,
    string? CategoryName,
    DateTimeOffset CreatedAt,
    List<string> Images
);
