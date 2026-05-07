using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Queries.Staffs.GetStaffByAccountId;

public class GetStaffByAccountIdHandler(
    IQueryBuilder<Staff> staffQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetStaffByAccountIdQuery, Result<GetDetailStaffByIdResponse>>
{
    public async Task<Result<GetDetailStaffByIdResponse>> Handle(
        GetStaffByAccountIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = staffQueryBuilder.QueryAsNoTracking
            .Where(x => x.AccountId == new AccountId(request.AccountId))
            .Select(x => new GetDetailStaffByIdResponse(
                x.Id.Value,
                x.Code.Value,
                x.Name,
                x.Email.Value,
                x.Phone != null ? x.Phone.Value : string.Empty,
                x.Department,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var staff = await queryExecutor.FirstOrDefaultAsync(query, cancellationToken);

        if (staff is null)
            return Result<GetDetailStaffByIdResponse>.Failure("Không tìm thấy thông tin nhân viên cho tài khoản này");

        return Result<GetDetailStaffByIdResponse>.Success(staff);
    }
}
