using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Customers.GetCustomerById;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Queries.Customers.GetDetailCustomerByAccountId;

public sealed class GetDetailCustomerByAccountIdHandler(
    IQueryBuilder<Customer> customerBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetDetailCustomerByAccountIdQuery, Result<GetCustomerByIdResponse>>
{
    public async Task<Result<GetCustomerByIdResponse>> Handle(
        GetDetailCustomerByAccountIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = customerBuilder.QueryAsNoTracking
            .Where(x => x.AccountId == new AccountId(request.AccountId))
            .Select(x => new GetCustomerByIdResponse(
                x.Id.Value,
                x.Name,
                x.Code.Value,
                x.TaxCode,
                x.Email.Value,
                x.Phone != null ? x.Phone.Value : null,
                x.Address,
                x.CreatedAt
            ));

        var result = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

        if (result is null)
            return Result<GetCustomerByIdResponse>.Failure("Không tìm thấy thông tin khách hàng cho tài khoản này");

        return Result<GetCustomerByIdResponse>.Success(result);
    }
}
