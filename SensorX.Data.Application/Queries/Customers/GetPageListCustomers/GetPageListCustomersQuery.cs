using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersQuery : IRequest<Result<PaginatedResult<GetPageListCustomersResponse>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? CustomerId { get; set; }

    public GetPageListCustomersQuery() { }

    public GetPageListCustomersQuery(int pageNumber, int pageSize, string? searchTerm = null, Guid? customerId = null)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 && pageSize <= 100 ? pageSize : 10;
        SearchTerm = searchTerm;
        CustomerId = customerId;
    }
}
