using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public sealed record GetPageListCustomersResponse(
    Guid Id,
    string Name,
    string Code,
    string TaxCode,
    string Email,
    string Phone,
    string Address,
    DateTimeOffset CreatedAt
);

