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

            if (request.Status.HasValue)
            {
                sourceQuery = sourceQuery.Where(x => x.Status == request.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var code = request.Code.Trim();
                sourceQuery = sourceQuery.Where(x => ((string)x.Code).Contains(code));
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();
                sourceQuery = sourceQuery.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var email = request.Email.Trim();
                sourceQuery = sourceQuery.Where(x => ((string)x.Email).Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                var phone = request.Phone.Trim();
                sourceQuery = sourceQuery.Where(x => x.Phone != null && ((string)x.Phone).Contains(phone));
            }

            if (!string.IsNullOrWhiteSpace(request.CitizenId))
            {
                var citizenId = request.CitizenId.Trim();
                sourceQuery = sourceQuery.Where(x => x.CitizenId != null && ((string)x.CitizenId).Contains(citizenId));
            }

            if (!string.IsNullOrWhiteSpace(request.Department) &&
                Enum.TryParse<Department>(request.Department, true, out var department))
            {
                sourceQuery = sourceQuery.Where(x => x.Department == department);
            }

            if (request.JoinFrom.HasValue)
            {
                sourceQuery = sourceQuery.Where(x => x.JoinDate >= request.JoinFrom.Value);
            }

            if (request.JoinTo.HasValue)
            {
                sourceQuery = sourceQuery.Where(x => x.JoinDate <= request.JoinTo.Value);
            }

            if (request.CreatedFrom.HasValue)
            {
                sourceQuery = sourceQuery.Where(x => x.CreatedAt >= request.CreatedFrom.Value);
            }

            if (request.CreatedTo.HasValue)
            {
                sourceQuery = sourceQuery.Where(x => x.CreatedAt <= request.CreatedTo.Value);
            }

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
                    x.Department,
                    x.Status,
                    x.JoinDate,
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
