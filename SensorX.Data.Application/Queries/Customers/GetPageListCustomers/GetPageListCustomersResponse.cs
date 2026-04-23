using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public record GetPageListCustomersResponse(
    Guid Id,
    string Name,
    string Code,
    string TaxCode,
    string Email,
    string PhoneNumber,
    string Address,
    DateTimeOffset CreatedAt
);

public class CustomerOffsetPagedResult : OffsetPagedResult<GetPageListCustomersResponse> { }