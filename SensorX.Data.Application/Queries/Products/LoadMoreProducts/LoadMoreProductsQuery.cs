using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.KeysetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed record LoadMoreProductsQuery : KeysetPagedQuery, IRequest<Result<KeysetPagedResult<LoadMoreProductsResponse>>>
{
    public string? SearchTerm { get; init; }
    public Guid? CategoryId { get; init; }
    public bool SortByName { get; init; }
}

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
