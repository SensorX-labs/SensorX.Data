using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.Staffs.CreateStaff;

public class CreateStaffHandler(
    IRepository<Staff> _staffRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<CreateStaffCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var id = new StaffId(Guid.NewGuid());
        var code = Code.Create("STF");
        
        // Parse Department from string to Enum
        if (!Enum.TryParse<Department>(request.Department, true, out var department))
        {
            return Result<Guid>.Failure("Phòng ban không hợp lệ.");
        }

        var staff = new Staff(
            id,
            new AccountId(request.AccountId),
            code,
            request.Name,
            Phone.From(request.Phone),
            Email.From(request.Email),
            CitizenId.From(request.CitizenId),
            request.Biography,
            request.JoinDate,
            department
        );

        await _staffRepository.Add(staff, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(staff.Id.Value);
    }
}