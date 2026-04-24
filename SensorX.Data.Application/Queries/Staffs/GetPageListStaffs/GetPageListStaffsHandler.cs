using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public class GetPageListStaffsHandler(
    IQueryBuilder<Staff> _staffBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListStaffsQuery, Result<StaffOffsetPagedResult>>
{
    public async Task<Result<StaffOffsetPagedResult>> Handle(
        GetPageListStaffsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery = _staffBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);
            
            var totalCount = await _queryExecutor.CountAsync(sourceQuery, cancellationToken);

            var pagedQuery = sourceQuery
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .ApplyOffsetPagination(request);

            var dtoQuery = pagedQuery.Select(x => new GetPageListStaffsResponse(
                x.Id.Value,
                x.Code.Value,
                x.Name,
                x.Email.Value,
                x.Phone.Value,
                x.CitizenId.Value,
                x.Department.ToString(),
                x.CreatedAt
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            var result = new StaffOffsetPagedResult
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            };

            return Result<StaffOffsetPagedResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<StaffOffsetPagedResult>.Failure(
                $"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
        }
    }
}
