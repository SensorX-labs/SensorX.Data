using MediatR;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public record GetPageListProductsQuery(
    string? SearchTerm
) : CursorPagedQuery, IRequest<Result<ProductCursorPagedResult>>;