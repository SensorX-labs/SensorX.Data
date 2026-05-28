using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public sealed record GetPageListProductsQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListProductsResponse>>>
{
    public string? SearchTerm { get; init; }
    public string? Code { get; init; }
    public string? Name { get; init; }
    public string? SupplierName { get; init; }
    public string? CategoryName { get; init; }
    public string? UnitOfQuantityName { get; init; }
    public decimal? RetailPriceFrom { get; init; }
    public decimal? RetailPriceTo { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public ProductStatus? Status { get; init; }
}
