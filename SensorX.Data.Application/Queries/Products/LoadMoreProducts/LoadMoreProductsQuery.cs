using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.LoadMore;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.LoadMoreProducts;

public sealed record LoadMoreProductsQuery : LoadMoreQuery, IRequest<Result<LoadMoreProductsResult>>
{
    public string? SearchTerm { get; init; }
}
