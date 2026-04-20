using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;

public class GetCustomerBuyingHistoryHandler(
    IQueryBuilder<Customer> customerQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetCustomerBuyingHistoryQuery, Result<GetCustomerBuyingHistoryResponse>>
{
    public async Task<Result<GetCustomerBuyingHistoryResponse>> Handle(
        GetCustomerBuyingHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var query = customerQueryBuilder.QueryAsNoTracking
            .Where(x => x.Id == new CustomerId(request.CustomerId))
            .Select(x => new GetCustomerBuyingHistoryResponse(
                x.Id.Value,
                x.Code.Value,
                x.Name,
                x.Email.Value,
                x.Phone.Value,
                x.Address,
                x.TaxCode,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var response = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

        if (response is null)
            return Result<GetCustomerBuyingHistoryResponse>.Failure("Không tìm thấy khách hàng");

        return Result<GetCustomerBuyingHistoryResponse>.Success(response);
    }
}
