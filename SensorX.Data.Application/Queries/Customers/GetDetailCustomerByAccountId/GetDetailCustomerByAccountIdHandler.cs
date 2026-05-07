using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Queries.Customers.GetDetailCustomerByAccountId;

public sealed class GetDetailCustomerByAccountIdHandler(
    IQueryBuilder<Customer> customerBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetDetailCustomerByAccountIdQuery, Result<GetDetailCustomerByAccountIdResponse>>
{
    public async Task<Result<GetDetailCustomerByAccountIdResponse>> Handle(
        GetDetailCustomerByAccountIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = customerBuilder.QueryAsNoTracking
            .Where(x => x.AccountId == new AccountId(request.AccountId))
            .Select(x => new GetDetailCustomerByAccountIdResponse(
                x.Id.Value,
                x.Name,
                x.Code.Value,
                x.TaxCode,
                x.Email.Value,
                x.Phone != null ? x.Phone.Value : null,
                x.Address,
                x.CreatedAt,
                x.ShippingInfo != null ? new ShippingInfoResponse(
                    x.ShippingInfo.ProvinceId.Value,
                    x.ShippingInfo.WardId.Value,
                    x.ShippingInfo.ShippingAddress,
                    x.ShippingInfo.ReceiverName,
                    x.ShippingInfo.ReceiverPhone.Value
                ) : null
            ));

        var result = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

        if (result is null)
            return Result<GetDetailCustomerByAccountIdResponse>.Failure("Không tìm thấy thông tin khách hàng cho tài khoản này");

        return Result<GetDetailCustomerByAccountIdResponse>.Success(result);
    }
}
