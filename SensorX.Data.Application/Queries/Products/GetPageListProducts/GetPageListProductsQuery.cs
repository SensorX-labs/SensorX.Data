using MediatR;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public record GetPageListProductsQuery : CursorPagedQuery, IRequest<Result<ProductCursorPagedResult>>
{
    public string? SearchTerm { get; init; }
}