using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public class GetPageListStaffsHandler(
    IQueryBuilder<Staff> _staffBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListStaffsQuery, Result<StaffCursorPagedResult>>
{
    public async Task<Result<StaffCursorPagedResult>> Handle(
        GetPageListStaffsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery = _staffBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);
            var pagedQuery = sourceQuery.ApplyCursorPagination(
                request,
                x => x.CreatedAt,
                x => x.Id
            )
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id);

            var dtoQuery = pagedQuery.Select(x => new GetPageListStaffsResponse
            {
                Id = x.Id.Value,
                Code = x.Code.Value,
                Name = x.Name,
                Phone = x.Phone.Value,
                Email = x.Email.Value,
                CitizenId = x.CitizenId.Value,
                Department = x.Department.ToString(),
                CreatedAt = x.CreatedAt
            });

            var items = await _queryExecutor.ToListAsync(dtoQuery
                .Take(request.PageSize + 1), cancellationToken);

            var hasNext = items.Count > request.PageSize;
            if (hasNext) items.RemoveAt(request.PageSize);

            var result = new StaffCursorPagedResult
            {
                Items = items,
                HasNext = hasNext,
                HasPrevious = request.IsPrevious,
                FirstCreatedAt = items.FirstOrDefault()?.CreatedAt,
                FirstId = items.FirstOrDefault()?.Id,
                LastCreatedAt = items.LastOrDefault()?.CreatedAt,
                LastId = items.LastOrDefault()?.Id
            };

            return Result<StaffCursorPagedResult>.Success(result);
        }
        catch (Exception ex)
        {
            // Trả về lỗi nếu có exception
            return Result<StaffCursorPagedResult>.Failure(
                $"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
        }
    }
}
