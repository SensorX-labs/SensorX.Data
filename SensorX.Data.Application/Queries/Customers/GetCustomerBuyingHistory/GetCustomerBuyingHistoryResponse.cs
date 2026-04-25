namespace SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;

public sealed record GetCustomerBuyingHistoryResponse(
    Guid CustomerId,
    string CustomerCode,
    string CustomerName,
    string Email,
    string Phone,
    string Address,
    string TaxCode,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
