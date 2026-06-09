using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.LoadMore;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed record LoadMoreProductsQuery : LoadMoreQuery, IRequest<Result<LoadMoreResult<LoadMoreProductsResponse>>>
{
    public string? SearchTerm { get; init; }
    public Guid? CategoryId { get; init; }
    public bool SortByName { get; init; }
}

public sealed record LoadMoreProductsResponse(
    Guid Id,
    string Code,
    string Name,
    string SupplierName,
    string UnitOfQuantityName,
    Guid? CategoryId,
    string? CategoryName,
    DateTimeOffset CreatedAt,
    List<string> Images
);
