using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Commands.Customers.DeleteCustomer;

public class DeleteCustomerHandler(
    IRepository<Customer> _customerRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<DeleteCustomerCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerId = new CustomerId(request.Id);
        
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
        {
            return Result<bool>.Failure("Không tìm thấy khách hàng với ID tương ứng.");
        }

        await _customerRepository.Delete(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
