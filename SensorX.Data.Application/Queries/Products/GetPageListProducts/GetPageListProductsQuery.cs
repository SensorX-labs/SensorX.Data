using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public sealed record GetPageListProductsQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListProductsResponse>>>
{
    public string? SearchTerm { get; init; }
    public ProductStatus? Status { get; init; }
}