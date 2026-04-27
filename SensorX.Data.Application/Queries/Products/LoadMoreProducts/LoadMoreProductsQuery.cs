using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.KeysetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed record LoadMoreProductsQuery : KeysetPagedQuery, IRequest<Result<KeysetPagedResult<LoadMoreProductsResponse>>>
{
    public string? SearchTerm { get; init; }
}
