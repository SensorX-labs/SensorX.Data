using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Customers.GetCustomerById;

public sealed record GetCustomerByIdResponse(
    Guid Id,
    string Name,
    string Code,
    string TaxCode,
    string Email,
    string? Phone,
    string? Address,
    DateTimeOffset CreatedAt
);

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<Result<GetCustomerByIdResponse>>;
