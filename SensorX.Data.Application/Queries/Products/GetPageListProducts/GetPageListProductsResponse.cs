using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public record GetPageListProductsResponse
(
    Guid Id,
    string Code,
    string Name,
    string Manufacture,
    string? CategoryName,
    decimal SuggestedPrice,
    ProductStatus Status,
    DateTimeOffset CreatedAt,
    List<string> Images
);

/// <summary>
/// This is a wrapper around paginated data, not a plain DTO.
/// 
/// We use a class (not record) because:
/// - It represents pagination context (HasNext, HasPrevious, cursors, etc.)
/// - It holds state, not just data
/// - It wraps another object (CursorPagedResult<T>)
///
/// Rule of thumb:
/// - Use record for pure DTOs (data only, no behavior/state)
/// - Use class for wrappers, results, or objects with state
/// </summary>
public class ProductCursorPagedResult : CursorPagedResult<GetPageListProductsResponse> { }