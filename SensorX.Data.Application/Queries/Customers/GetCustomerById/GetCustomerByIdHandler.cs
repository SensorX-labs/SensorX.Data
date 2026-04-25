using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;

namespace SensorX.Data.Application.Queries.Customers.GetCustomerById;

public sealed class GetCustomerByIdHandler(
    IQueryBuilder<Customer> _customerBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetCustomerByIdQuery, Result<GetCustomerByIdResponse>>
{
    public async Task<Result<GetCustomerByIdResponse>> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _customerBuilder.QueryAsNoTracking
                .Where(x => x.Id == new CustomerId(request.Id))
                .Select(x => new GetCustomerByIdResponse(
                    x.Id.Value,
                    x.Name,
                    x.Code.Value,
                    x.TaxCode,
                    x.Email.Value,
                    x.Phone.Value,
                    x.Address,
                    x.CreatedAt
                ));

            var result = await _queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

            if (result == null)
            {
                return Result<GetCustomerByIdResponse>.Failure("Không tìm thấy khách hàng.");
            }

            return Result<GetCustomerByIdResponse>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<GetCustomerByIdResponse>.Failure($"Lỗi khi lấy thông tin khách hàng: {ex.Message}");
        }
    }
}
