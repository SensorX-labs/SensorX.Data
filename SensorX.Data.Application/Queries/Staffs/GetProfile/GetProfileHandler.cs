using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Queries.Staffs.GetProfile;

public class GetProfileHandler(
    IQueryBuilder<Staff> staffQueryBuilder,
    IQueryExecutor queryExecutor,
    ICurrentUser _currentUser
) : IRequestHandler<GetProfileQuery, Result<GetProfileResponse>>
{
    public async Task<Result<GetProfileResponse>> Handle(
        GetProfileQuery request,
        CancellationToken cancellationToken)
    {
        var query = staffQueryBuilder.QueryAsNoTracking
            .Where(x => x.AccountId == _currentUser.UserId)
            .Select(x => new GetProfileResponse(
                x.Id,
                x.Code,
                x.Name,
                x.Phone,
                x.Email,
                x.CitizenId,
                x.Biography,
                x.JoinDate,
                x.Department,
                x.Status,
                x.CreatedAt,
                x.AvatarUrl
            ));

        var staff = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

        if (staff is null)
            return Result<GetProfileResponse>.Failure("Không tìm thấy thông tin nhân viên cho tài khoản này");

        return Result<GetProfileResponse>.Success(staff);
    }
}
