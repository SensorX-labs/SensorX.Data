using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public record GetPageListCustomersQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    string? Code
) : IRequest<Result<PaginatedResult<GetPageListCustomersResponse>>>
{
    public int PageNumber { get; } = PageNumber > 0 ? PageNumber : 1;
    public int PageSize { get; } = PageSize > 0 && PageSize <= 100 ? PageSize : 10;
}
