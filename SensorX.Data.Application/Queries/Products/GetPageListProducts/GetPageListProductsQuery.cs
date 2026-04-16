using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsQuery : IRequest<Result<PaginatedResult<GetPageListProductsDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }

    public GetPageListProductsQuery() { }

    public GetPageListProductsQuery(int pageNumber, int pageSize, string? searchTerm = null, Guid? categoryId = null)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 && pageSize <= 100 ? pageSize : 10;
        SearchTerm = searchTerm;
        CategoryId = categoryId;
    }
}
