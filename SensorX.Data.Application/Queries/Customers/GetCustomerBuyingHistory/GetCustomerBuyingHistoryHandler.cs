using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Customers.GetCustomerBuyingHistory;

public class GetCustomerBuyingHistoryHandler(
    IRepository<Customer> customerRepository
) : IRequestHandler<GetCustomerBuyingHistoryQuery, Result<GetCustomerBuyingHistoryResponse>>
{
    public async Task<Result<GetCustomerBuyingHistoryResponse>> Handle(
        GetCustomerBuyingHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var customerId = new CustomerId(request.CustomerId);
        var customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);

        if (customer is null)
            return Result<GetCustomerBuyingHistoryResponse>.Failure("Khách hàng không tồn tại");

        var response = new GetCustomerBuyingHistoryResponse(
            CustomerId: customer.Id.Value,
            CustomerCode: customer.Code.Value,
            CustomerName: customer.Name,
            Email: customer.Email.Value,
            Phone: customer.Phone.Value,
            Address: customer.Address,
            TaxCode: customer.TaxCode,
            CreatedAt: customer.CreatedAt.DateTime,
            UpdatedAt: customer.UpdatedAt?.DateTime
        );

        return Result<GetCustomerBuyingHistoryResponse>.Success(response);
    }
}
