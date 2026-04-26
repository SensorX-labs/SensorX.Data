using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.LoadMore;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public enum ProductSortBy
{
    CreatedAt,
    Name,
    Code,
    CategoryId
}

public sealed record LoadMoreProductsQuery : LoadMoreQuery, IRequest<Result<LoadMoreProductsResult>>
{
    public string? SearchTerm { get; init; }
    public ProductSortBy? OrderBy { get; init; }
}
