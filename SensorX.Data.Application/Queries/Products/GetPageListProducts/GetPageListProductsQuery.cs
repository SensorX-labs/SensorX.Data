using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public record GetPageListProductsQuery : OffsetPagedQuery, IRequest<Result<ProductOffsetPagedResult>>
{
    public string? SearchTerm { get; init; }
}