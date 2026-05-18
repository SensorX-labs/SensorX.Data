using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Customers.GetCustomerById;

namespace SensorX.Data.Application.Queries.Customers.GetDetailCustomerByAccountId;

public sealed record GetDetailCustomerByAccountIdQuery(Guid AccountId) : IRequest<Result<GetDetailCustomerByAccountIdResponse>>;

public sealed record GetDetailCustomerByAccountIdResponse(
    Guid Id,
    string Name,
    string Code,
    string TaxCode,
    string Email,
    string? Phone,
    string? Address,
    string? AvatarUrl,
    DateTimeOffset CreatedAt,
    ShippingInfoResponse? ShippingInfo
);

public sealed record ShippingInfoResponse(
    Guid? ProvinceId,
    Guid? WardId,
    string? ShippingAddress,
    string? ReceiverName,
    string? ReceiverPhone
);
