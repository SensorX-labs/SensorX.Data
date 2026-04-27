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
) : IRequestHandler<GetPageListStaffsQuery, Result<OffsetPagedResult<GetPageListStaffsResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListStaffsResponse>>> Handle(
        GetPageListStaffsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery = _staffBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);
            
            var totalCount = await _queryExecutor.CountAsync(sourceQuery, cancellationToken);

            var dtoQuery = sourceQuery
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .ApplyOffsetPagination(request)
                .Select(x => new GetPageListStaffsResponse(
                    x.Id.Value,
                    x.Code.Value,
                    x.Name,
                    x.Email.Value,
                    x.Phone != null ? x.Phone.Value : string.Empty,
                    x.CitizenId != null ? x.CitizenId.Value : string.Empty,
                    x.Department != null ? x.Department.ToString() : string.Empty,
                    x.CreatedAt
                ));

            var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            var result = new OffsetPagedResult<GetPageListStaffsResponse>
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            };

            return Result<OffsetPagedResult<GetPageListStaffsResponse>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<OffsetPagedResult<GetPageListStaffsResponse>>.Failure(
                $"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
        }
    }
}
