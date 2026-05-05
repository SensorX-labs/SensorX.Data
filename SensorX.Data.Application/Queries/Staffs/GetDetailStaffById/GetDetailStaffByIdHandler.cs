using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Staffs.GetDetailStaffById;

public class GetDetailStaffByIdHandler(
    IQueryBuilder<Staff> staffQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetDetailStaffByIdQuery, Result<GetDetailStaffByIdResponse>>
{
    public async Task<Result<GetDetailStaffByIdResponse>> Handle(
        GetDetailStaffByIdQuery request,
        CancellationToken cancellationToken)
    {
        var query = staffQueryBuilder.QueryAsNoTracking
            .Where(x => x.Id == new StaffId(request.StaffId))
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
            return Result<GetDetailStaffByIdResponse>.Failure("Nhân viên không tồn tại");

        return Result<GetDetailStaffByIdResponse>.Success(staff);
    }
}
